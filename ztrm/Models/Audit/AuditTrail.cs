using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ztrm.Models.Audit
{
    [Table("audittrail", Schema = "public")]
    public class AuditTrail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long audittrailid { get; set; }

        [Required]
        public DateTime timestamp { get; set; }

        [Required]
        [MaxLength(39)]
        public string ipaddress { get; set; }

        [Required]
        [MaxLength(10)]
        public string httpmethod { get; set; }

        [Required]
        [MaxLength(2048)]
        public string requesturl { get; set; }

        [Required]
        [Range(100, 599)]
        public int responsestatuscode { get; set; }

        [Required]
        public int durationmilliseconds { get; set; }

        [MaxLength(256)]
        public string username { get; set; }

        [MaxLength(512)]
        public string useragent { get; set; }

        [MaxLength(3000)]
        public string requestbody { get; set; }

        [MaxLength(3000)]
        public string responsebody { get; set; }

        [MaxLength(128)]
        public string sessionid { get; set; }

        [MaxLength(2048)]
        public string referrerurl { get; set; }

        [MaxLength(3000)]
        public string requestheaders { get; set; }

        [MaxLength(3000)]
        public string responseheaders { get; set; }

        [MaxLength(3000)]
        public string exception { get; set; }

        [MaxLength(256)]
        public string actionname { get; set; }

        [MaxLength(256)]
        public string controllername { get; set; }

    }
}
