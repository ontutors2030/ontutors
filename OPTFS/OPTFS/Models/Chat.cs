namespace OPTFS.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string? User1Id { get; set; }
        public ApplicationUser? User1 { get; set; }
        public string? User2Id { get; set; }
        public ApplicationUser? User2 { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;        
        public virtual List<ChatMessage>? Messages { get; set; }
    }
}
