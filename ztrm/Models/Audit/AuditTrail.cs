using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ztrm.Models.Audit
{
    public class AuditTrail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AuditTrailId { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [MaxLength(39)]
        public string IPAddress { get; set; }

        [Required]
        [MaxLength(10)]
        public string HttpMethod { get; set; }

        [Required]
        [MaxLength(2048)]
        public string RequestUrl { get; set; }

        [Required]
        [Range(100, 599)]
        public int ResponseStatusCode { get; set; }

        [Required]
        public int DurationMilliseconds { get; set; }

        [MaxLength(256)]
        public string UserName { get; set; }

        [MaxLength(512)]
        public string UserAgent { get; set; }

        [MaxLength(3000)]
        public string RequestBody { get; set; }

        [MaxLength(3000)]
        public string ResponseBody { get; set; }

        [MaxLength(128)]
        public string SessionId { get; set; }

        [MaxLength(2048)]
        public string ReferrerUrl { get; set; }

        [MaxLength(3000)]
        public string RequestHeaders { get; set; }

        [MaxLength(3000)]
        public string ResponseHeaders { get; set; }

        [MaxLength(3000)]
        public string Exception { get; set; }

        [MaxLength(256)]
        public string ActionName { get; set; }

        [MaxLength(256)]
        public string ControllerName { get; set; }

    }
}
