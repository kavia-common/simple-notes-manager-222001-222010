# Notes Backend (ASP.NET Core 8 - Simple Notes Manager)

Modern REST API (Ocean Professional style) for CRUD operations on notes.

- Port: 3001
- Swagger UI: http://localhost:3001/docs
- OpenAPI JSON: http://localhost:3001/openapi.json
- Base URL: /api/notes

## Endpoints

- GET /api/notes
- GET /api/notes/{id}
- POST /api/notes
- PUT /api/notes/{id}
- DELETE /api/notes/{id}

### Model (JSON)
```json
{
  "id": "guid",
  "title": "string (required, max 200)",
  "content": "string | null",
  "createdAt": "ISO8601",
  "updatedAt": "ISO8601"
}
```

## Run (Development)

From the `notes_backend` directory:

```bash
dotnet restore
dotnet run
```

The API will be available at http://localhost:3001 and docs at http://localhost:3001/docs.

## Example curl

List notes:
```bash
curl -s http://localhost:3001/api/notes | jq .
```

Create:
```bash
curl -s -X POST http://localhost:3001/api/notes \
  -H "Content-Type: application/json" \
  -d '{"title":"First note","content":"Hello"}' | jq .
```

Get by id:
```bash
curl -s http://localhost:3001/api/notes/{id} | jq .
```

Update:
```bash
curl -s -X PUT http://localhost:3001/api/notes/{id} \
  -H "Content-Type: application/json" \
  -d '{"title":"Updated title","content":"Updated content"}' | jq .
```

Delete:
```bash
curl -s -X DELETE http://localhost:3001/api/notes/{id} -i
```

## Validation and Status Codes

- 200 OK: Successful GET/PUT
- 201 Created: Successful POST (with Location header)
- 204 No Content: Successful DELETE
- 400 Bad Request: Invalid input (e.g., missing title, title > 200 chars)
- 404 Not Found: Note not found

## Environment

This service currently uses an in-memory repository and does not require environment variables. For future database integration, copy `.env.example` and set values accordingly.
