namespace API.Models.Entities
{
    public class Priority
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //k f-Key
        public int DocumentId { get; set; } 
        //k nav propt
        public Document Document { get; set; }
        //public PriorityStatus Status { get; set; }
        public PriorityStatus Status { get;set; }   
    }
    public enum PriorityStatus
    {
        Low = '3',
        High = '2',
        Critical= '1'
    }
}
