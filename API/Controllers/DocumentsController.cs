using API.Models.Entities;
using API.Models.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private DocumentsRepository _repos;
        private readonly IWebHostEnvironment _environment;

        public DocumentsController(DocumentsRepository repos, IWebHostEnvironment environment)
        {
            _repos = repos;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Document>>> GetDocuments()
        {
            return Ok(await _repos.GetDocumentsAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            return Ok(await _repos.GetDocumentByIdAsync(id));
        }

        [HttpPost("file"), DisableRequestSizeLimit]
        public async Task<ActionResult> InsertFileAsync()
        {
            try
            {
                var form = await Request.ReadFormAsync();
                var file = form.Files.FirstOrDefault();

                if (file != null && file.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Ok(new { FilePath = filePath });
                }

                return BadRequest("File not provided or empty.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("document"), DisableRequestSizeLimit]
        public async Task<ActionResult> InsertDocumentAsync()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();

                var name = formCollection["name"];
                var file = formCollection.Files["file"];
                var creationDate = formCollection["creationDate"];
                var dueDate = formCollection["dueDate"];
                var priority = formCollection["priority"];
                var document = new Document
                {
                    Name = name,
                    CreationDate = creationDate,
                    DueDate = dueDate,
                };

                var fileObj = new FileModel { FilePath="path"};
                var priorityObj = new Priority { Name = "Prior", Status= (PriorityStatus)priority.Count };

                document.FileModels = new List<FileModel> { fileObj };
                document.Priority = priorityObj;
                _repos.InsertDocumentAsync(document);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        //=================================================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            await _repos.DeleteDocumentAsync(id);
            return NoContent();
        }

        //=================================================================

        [HttpPut("{id}")]
        public async Task<IActionResult> EditDocument(int id,[FromBody] Document updatedDocument)
        {
            await _repos.EditDocumentAsync(id, updatedDocument);
            return Ok("from put in controller");
        }

        //================================================

    }
}
