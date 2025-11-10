using System.Collections.Concurrent;
using NotesBackend.Models;

namespace NotesBackend.Repositories
{
    /// <summary>
    /// Thread-safe in-memory repository for notes.
    /// </summary>
    public class InMemoryNotesRepository : INotesRepository
    {
        private readonly ConcurrentDictionary<Guid, Note> _store = new();

        public InMemoryNotesRepository()
        {
            // Seed with one example note for convenience
            var sample = new Note
            {
                Title = "Welcome to Simple Notes",
                Content = "Use the API at /api/notes to manage notes."
            };
            _store[sample.Id] = sample;
        }

        public Note Create(Note note)
        {
            note.Id = note.Id == Guid.Empty ? Guid.NewGuid() : note.Id;
            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = note.CreatedAt;
            _store[note.Id] = note;
            return note;
        }

        public bool Delete(Guid id)
        {
            return _store.TryRemove(id, out _);
        }

        public IEnumerable<Note> GetAll()
        {
            return _store.Values
                .OrderByDescending(n => n.UpdatedAt)
                .ToList();
        }

        public Note? GetById(Guid id)
        {
            _store.TryGetValue(id, out var note);
            return note;
        }

        public Note? Update(Note note)
        {
            if (!_store.ContainsKey(note.Id))
            {
                return null;
            }

            note.UpdatedAt = DateTime.UtcNow;
            _store[note.Id] = note;
            return note;
        }
    }
}
