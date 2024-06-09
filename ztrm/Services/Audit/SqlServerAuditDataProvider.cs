using Audit.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using ztrm.Models;
using ztrm.Models.Audit;
using Audit.WebApi;
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
            return auditTrail.audittrailid; // Return the ID of the inserted audit log
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
                timestamp                   = auditEvent.StartDate, 
                ipaddress                   = auditApiAction.IpAddress,
                httpmethod                  = auditApiAction.HttpMethod,
                requesturl                  = auditApiAction.RequestUrl,
                actionname                  = auditApiAction.ActionName,
                controllername              = auditApiAction.ControllerName,
                responsestatuscode          = auditApiAction.ResponseStatusCode,
                durationmilliseconds        = auditEvent.Duration,
                username                    = auditApiAction.UserName,
                useragent                   = auditApiAction.Headers["User-Agent"],
                requestbody                 = auditApiAction.RequestBody != null ? auditApiAction.RequestBody.ToString() : null ,
                responsebody                = auditApiAction.ResponseBody != null ? auditApiAction.ResponseBody.ToString(): null,
                sessionid                   = auditApiAction.TraceId,
                referrerurl                 = referrerUrl,

                //RequestHeaders            = auditEvent.Environment.request,
                //ResponseHeaders           = auditApiAction.ResponseHeaders

                exception                   = auditApiAction.Exception,
            };
        }



    }
}
