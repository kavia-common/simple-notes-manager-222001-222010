using Microsoft.AspNetCore.Mvc;
using NotesBackend.Models;
using NotesBackend.Repositories;

namespace NotesBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class NotesController : ControllerBase
    {
        private readonly INotesRepository _repository;
        private readonly ILogger<NotesController> _logger;

        public NotesController(INotesRepository repository, ILogger<NotesController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Get all notes.
        /// </summary>
        /// <returns>List of notes</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Note>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Note>> GetAll()
        {
            var notes = _repository.GetAll();
            return Ok(notes);
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Get a note by id.
        /// </summary>
        /// <param name="id">Note Id (GUID)</param>
        /// <returns>Note if found</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Note), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Note> GetById(Guid id)
        {
            var note = _repository.GetById(id);
            if (note is null)
            {
                return NotFound(new { message = "Note not found." });
            }
            return Ok(note);
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Create a new note.
        /// </summary>
        /// <param name="request">Note details</param>
        /// <returns>Created note</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Note), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Note> Create([FromBody] CreateNoteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var note = new Note
            {
                Title = request.Title.Trim(),
                Content = string.IsNullOrWhiteSpace(request.Content) ? null : request.Content
            };

            var created = _repository.Create(note);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Update an existing note.
        /// </summary>
        /// <param name="id">Note Id (GUID)</param>
        /// <param name="request">Updated fields</param>
        /// <returns>Updated note</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Note), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Note> Update(Guid id, [FromBody] UpdateNoteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var existing = _repository.GetById(id);
            if (existing is null)
            {
                return NotFound(new { message = "Note not found." });
            }

            existing.Title = request.Title.Trim();
            existing.Content = string.IsNullOrWhiteSpace(request.Content) ? null : request.Content;

            var updated = _repository.Update(existing);
            if (updated is null)
            {
                // Should not happen due to check above, but guard regardless
                return NotFound(new { message = "Note not found." });
            }

            return Ok(updated);
        }

        // PUBLIC_INTERFACE
        /// <summary>
        /// Delete a note by id.
        /// </summary>
        /// <param name="id">Note Id (GUID)</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            var deleted = _repository.Delete(id);
            if (!deleted)
            {
                return NotFound(new { message = "Note not found." });
            }

            return NoContent();
        }
    }
}
