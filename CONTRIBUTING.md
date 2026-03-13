# 🤝 Contributing to Book Loan Service

Thank you for your interest in contributing!  
This project is intentionally minimalistic and domain‑focused, and contributions that preserve clarity, architectural quality, and testability are especially welcome.

Whether you want to fix a bug, improve documentation, refine the domain model, or propose a new feature, this guide explains how to contribute effectively.

---

## 🧭 Guiding Principles

Before contributing, please keep in mind the core values of this project:

- **Domain clarity first** — business rules belong in the domain layer  
- **Thin application layer** — orchestrates use cases, nothing more  
- **Replaceable infrastructure** — no business logic in repositories or technical adapters  
- **Testability** — every change should be covered by unit or integration tests  
- **Minimalism** — avoid unnecessary abstractions or premature generalization  

Contributions that reinforce these principles are strongly encouraged.

---

## 🛠️ How to Contribute

### 1. Fork the repository

```bash
git fork https://github.com/<your-username>/book-loan-service.git
```

Clone your fork:

```bash
git clone https://github.com/<your-username>/book-loan-service.git
cd book-loan-service
```

### 2. Create a feature branch

```bash
git checkout -b feature/my-improvement
```

Use a clear, descriptive branch name.

### 3. Make your changes

- Follow the architectural boundaries (Domain / Application / Infrastructure / Tests)

- Keep code readable and expressive

- Add or update tests as needed

- Ensure the test suite passes:

```bash
dotnet test
```

### 4. Commit using conventional messages

This project uses Conventional Commits:

- feat: — new feature

- fix: — bug fix

- docs: — documentation changes

- test: — test improvements

- refactor: — code changes without behavior change

- chore: — tooling, CI, formatting

```txt
feat: add return book endpoint
fix: correct overdue loan detection logic
docs: improve architecture overview section
```

### 5. Push and open a Pull Request

Push your branch:

```bash
git push origin feature/my-improvement
```

Then open a Pull Request on GitHub.

Your PR should include:

- A clear description of the change

- The motivation behind it

- Any relevant screenshots or examples

- A note about added/updated tests

## 🧪 Testing Guidelines

All contributions must maintain or improve the project's test coverage.  
The testing strategy is intentionally simple and mirrors the architecture:

- **Unit tests** validate domain rules and application logic  
- **Integration tests** validate end‑to‑end flows using in‑memory infrastructure  
- No test should depend on external services or databases

### Unit Tests

Unit tests should:

- Mock repositories and external dependencies  
- Focus on business rules and domain behavior  
- Avoid testing infrastructure or framework internals  
- Use expressive test names and clear Arrange/Act/Assert structure  
- Prefer deterministic, stable test data (e.g., via `RandomHelper`)

Examples of what belongs in unit tests:

- Loan overdue detection  
- Borrowing limit enforcement  
- Book availability rules  
- Application service orchestration logic

### Integration Tests

Integration tests should:

- Use the real in‑memory repositories  
- Use the `AppTestFixture` to create a scoped DI container  
- Validate complete flows (borrowing, rejecting, returning, overdue scenarios)  
- Avoid mocking — the goal is to test the system as a whole

Examples of integration scenarios:

- Borrowing multiple books with mixed outcomes  
- Borrowing when a customer already has overdue loans  
- Returning a book and borrowing again  
- Adding books at runtime and borrowing them

### Test Helpers

This project includes helpers to keep tests clean and expressive:

- `RandomHelper` — generates stable random IDs  
- `LoanTestHelper` — creates active, overdue, or returned loans for setup

If you add new domain concepts, consider adding corresponding helpers to keep tests readable.

### Running Tests

Before submitting a pull request, ensure all tests pass:

```bash
dotnet test
```

## 🧱 Code Style

This project follows a clean, expressive, domain‑centric coding style.  
Contributions should preserve readability, architectural boundaries, and intention‑revealing code.

### General Principles

- Prefer **expressive names** over comments  
- Keep methods **small, focused, and predictable**  
- Avoid unnecessary abstractions or premature generalization  
- Favor **immutability** where possible  
- Keep responsibilities clearly separated across layers  
- Avoid leaking infrastructure concerns into the domain

### Domain Layer

- Must remain **pure** and free of technical dependencies  
- Should contain all business rules and invariants  
- Entities should enforce their own consistency  
- Value objects should be immutable  
- Domain services should be rare and focused

### Application Layer

- Orchestrates use cases, nothing more  
- Should not contain business rules  
- Should not depend on infrastructure details  
- Should return clear, explicit results (e.g., success/failure objects)

### Infrastructure Layer

- Contains technical details (repositories, DI, adapters)  
- Must not contain business logic  
- Should be replaceable without affecting domain or application layers

### Tests

- Should be expressive and intention‑revealing  
- Should avoid testing framework internals  
- Should focus on behavior, not implementation details  
- Should use helpers (`RandomHelper`, `LoanTestHelper`) to keep setup clean

Following these conventions ensures the project remains consistent, maintainable, and aligned with its architectural goals.

## 💡 Proposing Enhancements

If you want to suggest a new feature, architectural improvement, or domain refinement, please follow this process to ensure productive discussion and alignment with the project’s goals.

### 1. Open an Issue

Before writing any code, open an issue describing:

- The problem you want to solve  
- Why it matters  
- How it fits the project’s philosophy (domain‑driven, minimalistic, testable)  
- Any alternatives you considered  

This helps avoid duplicated work and ensures the idea aligns with the project direction.

### 2. Discuss the Proposal

Once the issue is open:

- Provide examples or pseudo‑code if helpful  
- Reference relevant domain rules or architectural constraints  
- Be open to feedback and iteration  
- Keep the discussion focused and constructive  

### 3. Wait for Approval Before Implementing

To maintain coherence and avoid wasted effort:

- Do not start implementing the feature until the proposal is validated  
- Large changes may require architectural discussion  
- Small improvements may be approved quickly  

### 4. Implement with Care

If the proposal is accepted:

- Follow the architectural boundaries (Domain / Application / Infrastructure)  
- Add or update tests accordingly  
- Keep the implementation minimal and expressive  
- Document the change in the PR description  

Enhancements that reinforce clarity, maintainability, and domain correctness are especially welcome.

## 🧼 Clean Commit History

A clean and readable commit history makes the project easier to maintain and review.  
When preparing a Pull Request, please ensure your commits follow these guidelines:

### Keep commits meaningful

Each commit should represent a clear, self‑contained change:

- Avoid mixing unrelated modifications  
- Group small related changes together  
- Do not include temporary or experimental commits in the final PR  

### Squash noisy commits

Before opening a PR, squash or reorganize commits such as:

- “fix typo”  
- “oops”  
- “temporary change”  
- “debugging”  
- “WIP”  

These should not appear in the final history.

### Follow Conventional Commits

Use the standard prefixes:

- `feat:` — new feature  
- `fix:` — bug fix  
- `docs:` — documentation updates  
- `test:` — test improvements  
- `refactor:` — code changes without behavior change  
- `chore:` — tooling, CI, formatting  

Examples:

```txt
feat: add endpoint to return borrowed books
fix: correct loan overdue calculation
docs: update architecture overview
```


### Keep the history linear

When possible:

- Rebase your branch on top of `main`  
- Avoid unnecessary merge commits  
- Resolve conflicts before opening the PR  

A linear history makes it easier to track changes and understand the evolution of the project.

## 🛡️ Code of Conduct

To maintain a welcoming and productive environment, all contributors are expected to follow these principles:

### Be respectful

- Treat others with kindness and professionalism  
- Assume good intentions  
- Avoid personal attacks, sarcasm, or dismissive behavior  

### Be constructive

- Focus on the issue, not the person  
- Offer actionable suggestions  
- Provide clear reasoning behind feedback  

### Be collaborative

- Listen to others’ perspectives  
- Seek consensus when possible  
- Help maintain a positive, solution‑oriented atmosphere  

### Be clear and honest

- Communicate openly about challenges or uncertainties  
- Ask questions when something is unclear  
- Acknowledge mistakes and correct them promptly  

### Zero tolerance for harmful behavior

The following are not allowed under any circumstances:

- Harassment or discrimination  
- Hate speech or abusive language  
- Threats or intimidation  
- Deliberate disruption of discussions  

Violations may result in warnings or removal from participation.

By contributing to this project, you agree to uphold this Code of Conduct and help foster a respectful, inclusive community.

## 🙏 Thank You

Thank you for taking the time to contribute to this project.  
Your ideas, improvements, and feedback help make the codebase clearer, more robust, and more valuable for everyone who studies or uses it.

Whether you submit a pull request, open an issue, refine the documentation, or simply explore the architecture, your participation is genuinely appreciated.

This project exists to promote clean design, thoughtful domain modeling, and maintainable software — and every contribution helps move it forward.
