using API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Data
{
    public class DirectoryContext : DbContext
    {
        public DirectoryContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<FileModel> FileModels { get; set; }
    }
}
