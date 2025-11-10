using NotesBackend.Models;

namespace NotesBackend.Repositories
{
    /// <summary>
    /// Abstraction for persisting and retrieving notes.
    /// </summary>
    public interface INotesRepository
    {
        // PUBLIC_INTERFACE
        /// <summary>
        /// Retrieves all notes.
        /// </summary>
        /// <returns>IEnumerable of Note</returns>
        IEnumerable<Note> GetAll();

        // PUBLIC_INTERFACE
        /// <summary>
        /// Retrieves a note by its id.
        /// </summary>
        /// <param name="id">Note Id</param>
        /// <returns>Note or null</returns>
        Note? GetById(Guid id);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Creates a new note.
        /// </summary>
        /// <param name="note">Note to create</param>
        /// <returns>Created Note</returns>
        Note Create(Note note);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Updates an existing note.
        /// </summary>
        /// <param name="note">Note with updated values</param>
        /// <returns>Updated Note or null if not found</returns>
        Note? Update(Note note);

        // PUBLIC_INTERFACE
        /// <summary>
        /// Deletes a note by id.
        /// </summary>
        /// <param name="id">Note Id</param>
        /// <returns>True if deleted, false if not found</returns>
        bool Delete(Guid id);
    }
}
