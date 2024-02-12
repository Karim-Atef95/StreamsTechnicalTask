namespace API.Models.Entities
{
    public class Document 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreationDate { get; set; }
        //public DateTime CreationDate { get; set; }
        public Priority Priority { get; set; }
        public string DueDate { get; set; }
        //public DateTime DueDate { get; set; }
        public List<FileModel> FileModels { get; set; }

   
    }

}
