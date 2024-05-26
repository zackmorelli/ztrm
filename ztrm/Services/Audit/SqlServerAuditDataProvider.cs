using Audit.Core;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Threading.Tasks;

using Newtonsoft.Json;


using ztrm.Models;
using Newtonsoft.Json.Linq;
using ztrm.Models.Audit;
using Audit.WebApi;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components.RenderTree;





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
            string referrerUrl;
            
            if (auditApiAction.Headers.ContainsKey("Referer") == true) 
            {
                referrerUrl = auditApiAction.Headers["Referer"];
            }
            else
            {
                referrerUrl = null;
            }
            

            return new AuditTrail
            {
                Timestamp = auditEvent.StartDate, 
                IPAddress = auditApiAction.IpAddress,
                HttpMethod = auditApiAction.HttpMethod,
                RequestUrl = auditApiAction.RequestUrl,
                ActionName = auditApiAction.ActionName,
                ControllerName = auditApiAction.ControllerName,
                ResponseStatusCode = auditApiAction.ResponseStatusCode,
                DurationMilliseconds = auditEvent.Duration,
                UserName = auditApiAction.UserName,
                UserAgent = auditApiAction.Headers["User-Agent"],
                RequestBody = auditApiAction.RequestBody != null ? auditApiAction.RequestBody.ToString() : null ,
                ResponseBody = auditApiAction.ResponseBody != null ? auditApiAction.ResponseBody.ToString(): null,
                SessionId = auditApiAction.TraceId,
                ReferrerUrl = referrerUrl,

                //RequestHeaders = auditEvent.Environment.request,
                //ResponseHeaders = auditApiAction.ResponseHeaders

                Exception = auditApiAction.Exception,
            };
        }



    }
}
