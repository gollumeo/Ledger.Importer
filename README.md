# Ledger.Importer

**Transaction ingestion and validation engine for FinTech applications.**
Parses raw CSV files into valid transaction entries using strict validation rules and Clean Architecture principles.

---

## 🚀 Use Case

Given a CSV file containing raw financial transactions, this module parses, validates, and stores them in memory for further analysis or categorization.

---

## 🔍 Example

**CSV Input**

```csv
description,amount,date
Uber Eats Paris,29.90,2025-05-08T12:45:00Z
Wirecard Transfer,-1050.00,2025-05-07T08:30:00Z
Deliveroo,18.40,2025-05-06T19:00:00Z
```

**Parsed Output**

```json
[
  {
    "description": "Uber Eats Paris",
    "amount": 29.90,
    "date": "2025-05-08T12:45:00Z"
  },
  {
    "description": "Wirecard Transfer",
    "amount": -1050.00,
    "date": "2025-05-07T08:30:00Z"
  },
  {
    "description": "Deliveroo",
    "amount": 18.40,
    "date": "2025-05-06T19:00:00Z"
  }
]
```

---

## 🧠 Business Rules

* CSV file **must have** `description`, `amount`, `date` headers
* `amount` must be a **valid decimal number**
* `date` must be a **valid ISO 8601 date**
* Invalid lines are **skipped or logged** depending on config
* No persistence layer: transactions are stored **in-memory only**

> All validation logic is encapsulated in the domain and fully testable.

---

## 🛡 Architecture

This module follows **Clean Architecture** and **CQRS**, exposing a dedicated ingestion endpoint.

```
Ledger.Importer/
├── Api/            # ASP.NET Core API (not Minimal API)
├── Application/    # Use cases, commands, handlers
├── Domain/         # Entities, Validators, ValueObjects
├── Infrastructure/ # CSV Reader, Parser, Storage (in-memory)
└── Tests/          # xUnit test suite
```

---

## ✅ Test Coverage

Unit tested with [xUnit](https://xunit.net/).
Run all tests:

```bash
dotnet test
```

---

## 🛠 How to Run

1. Clone the repo
2. Navigate to `Api/` and run the API:

```bash
dotnet run --project Api
```

3. Send a `multipart/form-data` request to:

```
POST /transactions/import
```

With a `file` field containing your CSV.

---

## 🛣 Roadmap

* [ ] In-memory CSV parsing and validation
* [ ] Line-by-line error reporting (configurable)
* [ ] Support for CSV delimiters and encodings
* [ ] Pluggable storage strategy (in-memory, file, or DB)
* [ ] Bulk import job runner (CLI or background service)
* [ ] CSV format autodetection (future)

---

## © License

MIT — Free to use, modify, and distribute.
