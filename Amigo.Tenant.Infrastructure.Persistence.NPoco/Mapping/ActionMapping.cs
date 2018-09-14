using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ActionMapping : Map<ActionDTO>
    {
        public ActionMapping()
        {

            TableName("vwAction");

            Columns(x =>
            {
                x.Column(y => y.ActionId);
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.Description);
                x.Column(y => y.Type);
                x.Column(y => y.ModuleCode);

                x.Column(y => y.TableStatus).Ignore();
            });
        }
    }
}
