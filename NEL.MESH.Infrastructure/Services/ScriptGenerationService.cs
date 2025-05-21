// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV3s;

namespace NEL.MESH.Infrastructure.Services
{
    internal class ScriptGenerationService
    {
        private readonly ADotNetClient adotNetClient;

        public ScriptGenerationService() =>
            this.adotNetClient = new ADotNetClient();

        public void GenerateBuildScript(string branchName, string projectName, string dotNetVersion)
        {
            var githubPipeline = new GithubPipeline
            {
                Name = "Build",

                OnEvents = new Events
                {
                    Push = new PushEvent { Branches = [branchName] },

                    PullRequest = new PullRequestEvent
                    {
                        Types = ["opened", "synchronize", "reopened", "closed"],
                        Branches = [branchName]
                    }
                },



                Jobs = new Dictionary<string, Job>
                {
                    {
                        "build",
                        new Job
                        {
                            RunsOn = BuildMachines.UbuntuLatest,

                            EnvironmentVariables = new Dictionary<string, string>
                            {
                                { "NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__MAILBOXID", "${{ secrets.NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__MAILBOXID }}" },
                                { "NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__PASSWORD", "${{ secrets.NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__PASSWORD }}" },
                                { "NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__KEY", "${{ secrets.NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__KEY }}" },
                                { "NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__ROOTCERTIFICATE", "${{ secrets.NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__ROOTCERTIFICATE }}" },
                                { "NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__INTERMEDIATECERTIFICATES__0", "${{ secrets.NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__INTERMEDIATECERTIFICATES__0 }}" },
                                { "NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__CLIENTCERTIFICATE", "${{ secrets.NEL_MESH_CLIENT_ACCEPTANCE_MESHCONFIGURATION__CLIENTCERTIFICATE }}" },
                                { "NEL_MESH_CLIENT_INTEGRATION_MESHCONFIGURATION__MAILBOXID", "${{ secrets.NEL_MESH_CLIENT_INTEGRATION_MESHCONFIGURATION__MAILBOXID }}" },
                                { "NEL_MESH_CLIENT_INTEGRATION_MESHCONFIGURATION__PASSWORD", "${{ secrets.NEL_MESH_CLIENT_INTEGRATION_MESHCONFIGURATION__PASSWORD }}" },
                                { "NEL_MESH_CLIENT_INTEGRATION_MESHCONFIGURATION__KEY", "${{ secrets.NEL_MESH_CLIENT_INTEGRATION_MESHCONFIGURATION__KEY }}" },
                                { "NEL_MESH_CLIENT_INTEGRATION_MESHCONFIGURATION__ROOTCERTIFICATE", "${{ secrets.NEL_MESH_CLIENT_INTEGRATION_MESHCONFIGURATION__ROOTCERTIFICATE }}" },
                                { "NEL_MESH_CLIENT_INTEGRATION_MESHCONFIGURATION__INTERMEDIATECERTIFICATES__0", "${{ secrets.NEL_MESH_CLIENT_INTEGRATION_MESHCONFIGURATION__INTERMEDIATECERTIFICATES__0 }}" },
                                { "NEL_MESH_CLIENT_INTEGRATION_MESHCONFIGURATION__CLIENTCERTIFICATE", "${{ secrets.NEL_MESH_CLIENT_INTEGRATION_MESHCONFIGURATION__CLIENTCERTIFICATE }}" },
                            },

                            Steps = new List<GithubTask>
                            {
                                new CheckoutTaskV3
                                {
                                    Name = "Check Out"
                                },

                                new SetupDotNetTaskV3
                                {
                                    Name = "Setup Dot Net Version",

                                    With = new TargetDotNetVersionV3
                                    {
                                        DotNetVersion = dotNetVersion
                                    }
                                },

                                new RestoreTask
                                {
                                    Name = "Restore"
                                },

                                new DotNetBuildTask
                                {
                                    Name = "Build"
                                },

                                new TestTask
                                {
                                    Name = "Test"
                                },

                                new TestTask
                                {
                                    Name = "Run Unit Tests",
                                    Run = "dotnet test NEL.Mesh.Tests.Unit/NEL.Mesh.Tests.Unit.csproj --no-build --verbosity normal"
                                },

                                new TestTask
                                {
                                    Name = "Run Unit Tests",
                                    Run = "dotnet test NEL.MESH.Tests.Acceptance/NEL.MESH.Tests.Acceptance.csproj --no-build --verbosity normal"
                                },
                            }
                        }
                    },
                    {
                        "add_tag",
                        new TagJob(
                            runsOn: BuildMachines.UbuntuLatest,
                            dependsOn: "build",
                            projectRelativePath: $"{projectName}/{projectName}.csproj",
                            githubToken: "${{ secrets.PAT_FOR_TAGGING }}",
                            branchName: branchName)
                        {
                            Name = "Tag and Release"
                        }
                    },
                    {
                        "publish",
                        new PublishJobV2(
                            runsOn: BuildMachines.UbuntuLatest,
                            dependsOn: "add_tag",
                            dotNetVersion: dotNetVersion,
                            nugetApiKey: "${{ secrets.NUGET_ACCESS }}")
                        {
                            Name = "Publish to NuGet"
                        }
                    }
                }
            };

            string buildScriptPath = "../../../../.github/workflows/build.yml";
            string directoryPath = Path.GetDirectoryName(buildScriptPath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            this.adotNetClient.SerializeAndWriteToFile(
                githubPipeline,
                path: buildScriptPath);
        }

        public void GeneratePrLintScript(string branchName)
        {
            var githubPipeline = new GithubPipeline
            {
                Name = "PR Linter",

                OnEvents = new Events
                {
                    PullRequest = new PullRequestEvent
                    {
                        Types = ["opened", "edited", "synchronize", "reopened", "closed"],
                        Branches = [branchName]
                    }
                },

                Jobs = new Dictionary<string, Job>
                {
                    {
                        "label",
                        new LabelJobV2(runsOn: BuildMachines.UbuntuLatest)
                        {
                            Name = "Add Label(s)",
                        }
                    },
                    {
                        "requireIssueOrTask",
                        new RequireIssueOrTaskJob()
                        {
                            Name = "Require Issue Or Task Association",
                        }
                    },
                }
            };

            string buildScriptPath = "../../../../.github/workflows/prLinter.yml";
            string directoryPath = Path.GetDirectoryName(buildScriptPath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            adotNetClient.SerializeAndWriteToFile(
                adoPipeline: githubPipeline,
                path: buildScriptPath);
        }
    }
}
