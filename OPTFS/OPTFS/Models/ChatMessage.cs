namespace OPTFS.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string? Text { get;set; }
        public string? SenderId {  get; set; }
        public virtual ApplicationUser? Sender { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        public string? RecieverId { get; set; }
        public virtual ApplicationUser? Reciever { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public int? StatusId { get; set; } = (int)MessageStatus.New;
        public int? ChatId { get; set; }
        public virtual Chat? Chat { get; set; }
    }
}
