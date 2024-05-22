using Elfie.Serialization;

namespace OPTFS.Models.PaymentModels
{
    public class Payment
    {
        public string id { get; set; }
        public string? status { get; set; }
        public decimal? amount { get; set; }
        public decimal? fee { get; set; }
        public string? currency { get; set; }
        public decimal? refunded { get; set; }
        public string? refunded_at { get; set; }
        public decimal ? captured { get; set; }
        public string? captured_at { get; set; }
        public string? voided_at { get; set; }
        public string? description { get; set; }
        public string? amount_format { get; set; }
        public string? fee_format { get; set; }
        public string? refunded_format { get; set; }
        public string? captured_format { get; set; }
        public string? invoice_id { get; set; }
        public string? ip { get; set; }
        public string? callback_url { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string? PSourceId { get; set; }
        public virtual PSource? source { get; set; }
        public int? courseId { get; set; }
        public int? requestId {  set; get; } 
    }
}
