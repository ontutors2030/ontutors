namespace OPTFS.Models.PaymentModels
{
    public class PSource
    {
        public string id { get; set; }
        public string? type { get; set; }
        public string? company { get; set; }
        public string? name { get; set; }
        public string? number { get; set; }
        public string? gateway_id { get; set; }
        public string? reference_number { get; set; }
        public string? token { get; set; }
        public string? message { get; set; }
        public string? transaction_url { get; set; }
        public string? response_code { get; set; }
        public string? authorization_code { get; set; }
        public string? issuer_name { get; set; }
        public string? issuer_country { get; set; }
        public string? issuer_card_type { get; set; }
        public string? issuer_card_category { get; set; }
    }
}
