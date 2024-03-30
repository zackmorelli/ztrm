using Audit.Core;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Threading.Tasks;

using Newtonsoft.Json;


using ztrm.Models;
using Newtonsoft.Json.Linq;
using ztrm.Models.Audit;
using Audit.WebApi;





namespace ztrm.Services.Audit
{
    public class SqlServerAuditDataProvider : AuditDataProvider
    {
        private readonly AuditDbContext _auditDbContext;

        public SqlServerAuditDataProvider(AuditDbContext auditDbContext)
        {
            _auditDbContext = auditDbContext;
        }

        public override object InsertEvent(AuditEvent auditEvent)
        {
            var auditTrail = ConvertToAuditTrail(auditEvent);
            _auditDbContext.AuditTrails.Add(auditTrail);
            _auditDbContext.SaveChanges();
            return auditTrail.AuditTrailId; // Return the ID of the inserted audit log
        }

        private AuditTrail ConvertToAuditTrail(AuditEvent auditEvent)
        {
            AuditApiAction auditApiAction = auditEvent.GetWebApiAuditAction();

            return new AuditTrail
            {
                Timestamp = auditEvent.StartDate, 
                IPAddress = auditApiAction.IpAddress,



                //HttpMethod = detailsObject["HttpMethod"]?.ToString(),
                //RequestUrl = detailsObject["RequestUrl"]?.ToString(),
                //ResponseStatusCode = detailsObject["ResponseStatusCode"] != null ? int.Parse(detailsObject["ResponseStatusCode"].ToString()) : 0,
                //DurationMilliseconds = detailsObject["DurationMilliseconds"] != null ? int.Parse(detailsObject["DurationMilliseconds"].ToString()) : 0,
                //UserName = auditEvent.Environment.UserName.ToString(),
                //UserAgent = detailsObject["UserAgent"]?.ToString(),
                //RequestBody = detailsObject["RequestBody"]?.ToString(),
                //ResponseBody = detailsObject["ResponseBody"]?.ToString(),
                //SessionId = detailsObject["SessionId"]?.ToString(),
                //ReferrerUrl = detailsObject["ReferrerUrl"]?.ToString(),
                //RequestHeaders = detailsObject["RequestHeaders"]?.ToString(),
                //ResponseHeaders = detailsObject["ResponseHeaders"]?.ToString(),
                //Exception = detailsObject["Exception"]?.ToString(),
                //ActionName = detailsObject["ActionName"]?.ToString(),
                //ControllerName = detailsObject["ControllerName"]?.ToString()
            };
        }



    }
}
