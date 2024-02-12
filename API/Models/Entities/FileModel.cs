namespace API.Models.Entities
{
    public class FileModel
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        //k nav prop
        public Document Document { get; set; }
        //k f-key
        public int DocumentId { get; set; }
    }
}
