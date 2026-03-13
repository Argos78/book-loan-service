# 📚 Book Loan Service

![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)

A lightweight, domain‑driven service that manages book borrowing operations through a clean and extensible API.

Managing the lifecycle of borrowed books may seem simple at first glance, yet it hides a surprisingly rich set of rules:  
books can be unavailable, customers may reach borrowing limits, loans can expire, and overdue items must be handled gracefully.  
**Book Loan Service** was created to explore this domain in depth — not as a full library system, but as a clean, expressive, and testable foundation for book‑borrowing logic.

This project demonstrates how a well‑designed domain model, a thin application layer, and lightweight infrastructure can work together to form a robust, maintainable service.  
It is intentionally minimalistic, focusing on clarity, correctness, and architectural quality rather than feature breadth.

---


## 🎯 Project Goals

This project serves as a focused exploration of domain‑driven design applied to a simple but meaningful problem: managing the lifecycle of borrowed books.

The goals are:

- To model a clear and expressive domain with well‑defined business rules  
- To demonstrate how a thin application layer orchestrates domain behavior  
- To keep infrastructure lightweight, replaceable, and free of business logic  
- To provide a fully testable architecture (unit + integration)  
- To offer a clean, readable codebase suitable for learning, teaching, or portfolio use  

Rather than building a full library management system, this project isolates the core borrowing logic to highlight architectural clarity and maintainability.

---


## ✨ What this project offers

- A **domain‑driven** model for book loans  
- Borrowing, returning, and validating book availability  
- Overdue detection and loan limit enforcement  
- A clean, extensible **application service**  
- In‑memory repositories for fast development  
- A fully tested core (unit + integration tests)  
- A structure designed for long‑term maintainability

---


## 🏛️ Architecture Overview

The project follows a domain‑centric, layered architecture:

### **Domain**

- Entities (Book, Customer, Loan)  
- Business rules (availability, overdue logic, loan limits)

### **Application**

- Commands (BorrowBooksCommand)  
- Services (LibraryService)  
- Error messages and result handling

### **Infrastructure**

- In‑memory repositories  
- Dependency injection setup

### **Test**

- Unit tests (mocked repositories)  
- Integration tests (real in‑memory repos + scoped DI fixture)  
- Test helpers (RandomHelper, LoanTestHelper)

The **Domain** layer is pure and independent.  
The **Application** layer orchestrates use cases.  
The **Infrastructure** layer provides replaceable technical details.  
The **Test** layer ensures correctness at every level.

---


## 🧱 Requirements

- .NET 8 SDK (or later)  
- Git (for cloning the repository)  
- A terminal or command prompt (Windows, macOS, or Linux)

No external database or message broker is required — everything runs in memory.

---


## 🚀 Getting Started

Clone the repository:

```bash
git clone https://github.com/Argos78/book-loan-service.git
cd book-loan-service
```

Run the test suite:

```bash
dotnet test
```

---


## ▶️ Running the API

To start the API locally:

```bash
dotnet run --project src/BookLoanService.Api
```

Once running, the service will be available at:

[http://localhost:5160](http://localhost:5160)


(Or another port depending on your environment.)

---


## 📘 Swagger UI

Once the API is running, you can explore the available endpoints through the built‑in Swagger UI:

[http://localhost:5160/swagger](http://localhost:5160/swagger)


This interface provides a complete, interactive description of the API, including request/response schemas and example payloads.

---


## 📡 API Overview (high‑level)

Even though a Swagger UI is available, here is a brief overview of the main capabilities:

### **Borrow books**

Submit a list of book IDs for a given customer.  
The service returns:

- **BorrowedBooks** → successfully borrowed items  
- **RejectedBooks** → items rejected with reason codes  
  (`NOT_FOUND`, `NOT_AVAILABLE`, `LOAN_OVERDUE`, `LIMIT_REACHED`)

### **List all books**

Retrieve the current catalog, including books added at runtime.

### **Add a book**

Add a new book with an auto‑assigned ID.

This section intentionally stays concise to avoid duplicating the Swagger contract.


## 🧪 Testing Strategy

Testing is central to this project.

### **Unit Tests**

- Mocked repositories  
- Validate business rules and service behavior  
- Fast and deterministic  

### **Integration Tests**

- Real in‑memory repositories  
- Scoped DI container via `AppTestFixture`  
- End‑to‑end scenarios (borrowing, rejecting, overdue logic, etc.)

### **Test Helpers**

- `RandomHelper` for stable random IDs  
- `LoanTestHelper` for generating active, overdue, or returned loans  


## 🗺️ Roadmap

- [ ] Add persistence layer (SQL or NoSQL)
- [ ] Add authentication / authorization
- [ ] Add book return endpoint
- [ ] Add overdue notifications
- [ ] Add CI/CD pipeline and build badge


## 🤝 Contributing

Contributions, suggestions, and improvements are welcome.  
Feel free to open an issue or submit a pull request.


## 📄 License

This project is licensed under the **MIT License**.  
You are free to use, modify, and distribute it with attribution.
