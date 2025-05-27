# Ledger.Importer

**Transaction ingestion & streaming for FinTech backends.**
Parses CSV files into typed transaction entries using Clean Architecture, CQRS, and test-first principles.

---

## ✨ Use Cases

Ledger.Importer exposes **two distinct ingestion flows**:

| Flow     | Method                             | Description                                        |
| -------- | ---------------------------------- | -------------------------------------------------- |
| Sync     | `POST /transactions/import`        | Upload a CSV file, get parsed transactions as JSON |
| Streamed | `GET  /transactions/import/stream` | Streams import events (SSE) line by line           |

Both rely on a shared validation pipeline and domain encapsulation.

---

## 🗕 Example Input

```csv
description,amount,date
Uber Eats Paris,29.90,2025-05-08T12:45:00Z
Deliveroo Bruxelles,18.40,2025-05-10T19:00:00Z
Uber Airport,45.00,2025-05-11T09:30:00Z
```

---

## ✅ Business Rules

* CSV **must include headers**: `description`, `amount`, `date`
* `amount` must be a **valid decimal**
* `date` must be a **valid ISO 8601** timestamp
* Invalid lines are **skipped or narrated as failures**
* Transactions are parsed and held **in-memory only**
* All validation is encapsulated in the **domain layer**

---

## 🧱 Architecture

> Clean Archi stricte + CQRS + Narration SSE

```
Ledger.Importer/
├── Api/              # ASP.NET Core API (not Minimal)
├── Application/      # Commands, Use Cases, Narrators, ReadModels
├── Domain/           # Entities, Value Objects, Validators
├── Infrastructure/   # Future storage strategies (not yet used)
├── Presentation/     # Controllers, Presenters, HTTP Narrators
└── Tests/            # Full unit + integration suite (xUnit)
```

---

## 🧪 Test Coverage

Full coverage via [xUnit](https://xunit.net/) with:

* ✅ Domain parsing & validation
* ✅ Application orchestration (CQRS)
* ✅ Narration through `INarrateTransactionsImportLive`
* ✅ End-to-end integration (including file upload and streaming)

```bash
dotnet test
```

---

## 💥 How to Run

```bash
dotnet run --project Ledger.Importer.Api
```

Then:

#### 🔁 Sync Import

```http
POST /transactions/import
Content-Type: multipart/form-data
Field: file=sample.csv
```

#### 🌊 Streamed Import (SSE)

```http
GET /transactions/import/stream
```

> Must have `storage/sample-transactions.csv` present at runtime

---

## 📦 Output Contracts

### ✅ Sync Response

```json
{
  "count": 3,
  "items": [
    {
      "description": "Uber Eats Paris",
      "amount": 29.9,
      "date": "2025-05-08T12:45:00Z"
    },
    ...
  ]
}
```

### 🌊 SSE Stream

```
event: TransactionImported
data: { "description": "...", ... }

event: TransactionFailed
data: { "line": 6, "reason": "Invalid or unparseable line." }

event: ImportCompleted
data: { "total": 6, "failed": 1 }
```

---

## 🚣 Roadmap

* [x] Line-by-line validation
* [x] In-memory import flow
* [x] Narration via SSE (Server-Sent Events)

---

## 📜 License

MIT — Use freely, no warranty.
