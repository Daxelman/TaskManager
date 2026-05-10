# Task Manager API — daily notes

Keep one entry per day. Short is fine. The point is to articulate what clicked.

---

## Day 1 — Project setup & solution structure

**What I built:**
- 3-project solution: Api / Domain / Infrastructure
- AppDbContext with TaskItem + Category entities configured via Fluent API
- SQLite dev database, auto-migrated on startup
- /health endpoint to verify everything is wired up

**Key concepts:**
- `DbContextOptions<T>` injected via DI rather than hardcoded in the context
- `ApplyConfigurationsFromAssembly` — lets you split entity config into separate files later
- `HasConversion<string>()` — stores enum as a readable string in the DB column
- `EnableSensitiveDataLogging()` — shows parameter values in logged SQL (dev only!)
- `MigrateAsync()` on startup vs. running migrations manually: trade-offs?

**Questions to revisit:**
- When should I use `EnsureCreated()` vs `MigrateAsync()`?
  → `EnsureCreated` skips the migrations table entirely — fine for tests, wrong for prod.
- What does `DeleteBehavior.SetNull` mean on the Category FK?
  → When a Category is deleted, CategoryId on its Tasks becomes null rather than cascading.

---

<!-- Copy this template for each new day -->
## Day N — Title

**What I built:**

**Key concepts:**

**Questions to revisit:**
