using Microsoft.EntityFrameworkCore;
using API.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace API.Models.Config
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.Property(d => d.CreationDate).HasColumnType("datetime2");
            builder.Property(d => d.DueDate).HasColumnType("datetime2");
        }
    }
}
