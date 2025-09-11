# Copilot Instructions

## Code Style Rules

### Line Length
- All `.cs` source files must adhere to the following rule:
  - No line of code should exceed **120 characters** in length.
  - This includes comments, string literals, and code.
  - Exception: automatically generated files may be ignored if they cannot be reformatted safely.

### Code Formatting
- Single-line instructions must follow each other with **no blank lines** in between.
- Multi-line instructions must always be preceded by **exactly one blank line**.
- If a multi-line instruction is followed by further instructions, it must also be followed by **exactly one blank line**.
- Any C# `return` statement must be preceded by **exactly one blank line** if it comes after other instructions.

### Enforcement
- Copilot should **not generate or suggest code** that exceeds the 120-character line limit.
- When writing new C# code, Copilot should:
  - Break up long method calls across multiple lines.
  - Use string interpolation or verbatim strings with proper line breaks if a literal would otherwise exceed 120 characters.
  - Format long LINQ queries across multiple lines.
  - Suggest wrapping parameters and arguments for readability.
  - Insert a blank line before any `return` statement that follows other instructions.

### Review Guidelines
- When reviewing or completing code suggestions, Copilot should:
  - Scan `.cs` files for lines longer than 120 characters.
  - Highlight or flag any violations.
  - Recommend a multiline formatting fix for flagged lines.
  - Flag missing blank lines before `return` statements.

### Examples

#### ✅ Correct (return with blank line)
```csharp
var user = users.FirstOrDefault(u => u.Id == id);

return user;
```

#### ❌ Incorrect (missing blank line before return)
```csharp
var user = users.FirstOrDefault(u => u.Id == id);
return user;
```

---

### Code Formatting Rule Examples

#### ✅ Correct
```cs
var activeUsers = users.Where(u => u.IsActive == false).Select(u => new { u.Id, u.Name }).ToList();
var activeUsers = users.Where(u => u.IsActive).Select(u => new { u.Id, u.Name }).ToList();

var filteredUsers = users
    .Where(u => u.IsActive && u.LastLoginDate >= DateTime.UtcNow.AddDays(-30))
    .OrderByDescending(u => u.LastLoginDate)
    .Select(u => new
    {
        u.Id,
        u.Name,
        u.Email,
        LastSeen = u.LastLoginDate.ToString("yyyy-MM-dd HH:mm:ss")
    })
    .ToList();

var x = 1 + 2;
var y = 2 + 2;

return y;
```

#### ❌ Incorrect
```cs
var activeUsers = users.Where(u => u.IsActive == false).Select(u => new { u.Id, u.Name }).ToList();
var activeUsers = users.Where(u => u.IsActive).Select(u => new { u.Id, u.Name }).ToList();
var filteredUsers = users
    .Where(u => u.IsActive && u.LastLoginDate >= DateTime.UtcNow.AddDays(-30))
    .OrderByDescending(u => u.LastLoginDate)
    .Select(u => new
    {
        u.Id,
        u.Name,
        u.Email,
        LastSeen = u.LastLoginDate.ToString("yyyy-MM-dd HH:mm:ss")
    })
    .ToList();
var x = 1 + 2;
var y = 2 + 2;
return y;
```
