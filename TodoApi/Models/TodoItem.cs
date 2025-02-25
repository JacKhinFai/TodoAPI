namespace TodoApi.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Completed { get; set; } = false;
    }
}