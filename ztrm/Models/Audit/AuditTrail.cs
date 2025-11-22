using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ztrm.Models.Audit
{
    [Table("audit_trail", Schema = "public")]
    public class AuditTrail
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("time_stamp")]
        public DateTimeOffset Timestamp { get; set; }

        [Required]
        [MaxLength(39)]
        [Column("ip_address")]
        public string IpAddress { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        [Column("http_method")]
        public string HttpMethod { get; set; } = null!;

        [Required]
        [MaxLength(2048)]
        [Column("request_url")]
        public string RequestUrl { get; set; } = null!;

        [Required]
        [Range(100, 599)]
        [Column("response_status_code")]
        public int ResponseStatusCode { get; set; }

        [Required]
        [Column("duration_milli_seconds")]
        public int DurationMilliseconds { get; set; }

        [MaxLength(256)]
        [Column("user_name")]
        public string? UserName { get; set; }

        [MaxLength(512)]
        [Column("user_agent")]
        public string? UserAgent { get; set; }

        [MaxLength(2000)]
        [Column("request_body")]
        public string? RequestBody { get; set; }

        [MaxLength(2000)]
        [Column("response_body")]
        public string? ResponseBody { get; set; }

        [MaxLength(128)]
        [Column("session_id")]
        public string? SessionId { get; set; }

        [MaxLength(2048)]
        [Column("referrer_url")]
        public string? ReferrerUrl { get; set; }

        [MaxLength(1000)]
        [Column("request_headers")]
        public string? RequestHeaders { get; set; }

        [MaxLength(1000)]
        [Column("response_headers")]
        public string? ResponseHeaders { get; set; }

        [MaxLength(1000)]
        [Column("exception")]
        public string? Exception { get; set; }

        [MaxLength(256)]
        [Column("action_name")]
        public string? ActionName { get; set; }

        [MaxLength(256)]
        [Column("controller_name")]
        public string? ControllerName { get; set; }

    }
}
