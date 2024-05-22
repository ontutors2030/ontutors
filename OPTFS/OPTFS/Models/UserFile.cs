namespace OPTFS.Models
{
    public class UserFile
    {
        public int Id { get; set; }
        public string? FileName {  get; set; }
        public string? Url { get; set; }
        public string? UserId { get; set; }
        public long SizeBytes { get; set; }
        public int FileTypeId { get; set; }// نوع الملف - 1- صورة - 2 مستند
        public int AttachmentTypeId { get; set; } // نوع المستند - 1- صورة الحساب 2- مؤهلات 
        public DateTime? UploadDate { get; set; }
        public virtual ApplicationUser? User { get; set; }        
    }
}
