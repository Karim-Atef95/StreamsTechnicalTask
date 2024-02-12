using API.Models.Data;
using API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Repository
{
    public class DocumentsRepository
    {
        private readonly DirectoryContext _context;

        public DocumentsRepository(DirectoryContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<Document>> GetDocumentsAsync()
        {
            return await _context.Documents
                .Include(d => d.Priority)
                .Include(d => d.FileModels)
                .ToListAsync();
        }
        public async Task<Document> GetDocumentByIdAsync(int id)
        {
            return await _context.Documents
                .Include(d => d.Priority)
                .Include(d => d.FileModels)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
        //public async Task<IReadOnlyList<FileModel>> GetFilesAsync()
        //{
        //    return await _context.FileModels.ToListAsync();
        //}
        //public async Task<FileModel> GetFileByIdAsync(int id)
        //{
        //    return await _context.FileModels.FirstOrDefaultAsync(f=>f.Id==id);
        //}
        //public async Task DeleteDocumentAsync(int id)
        //{
        //    var file = await _context.FileModels.FirstOrDefaultAsync(f => f.Id == id);
        //    if (file != null)
        //    {
        //        _context.FileModels.Remove(file);
        //        await _context.SaveChangesAsync();
        //    }
        //}
        public async Task InsertDocumentAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteDocumentAsync(int id)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id == id);
            _context.Documents.Remove(document); 
            await _context.SaveChangesAsync();  
        }
        public async Task EditDocumentAsync(int id, [FromBody] Document updatedDocument)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(f => f.Id == id);
            document.Name = updatedDocument.Name;
            document.CreationDate = updatedDocument.CreationDate;
            document.DueDate = updatedDocument.DueDate;
            document.Priority = updatedDocument.Priority;
            //document.FileModels = updatedDocument.FileModels;
            await _context.SaveChangesAsync();
        }
        public async Task InsertFileAsync()
        {

        }

    }


}

