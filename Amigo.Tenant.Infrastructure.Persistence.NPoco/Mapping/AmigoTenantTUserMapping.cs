using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class AmigoTenantTUserMapping: Map<AmigoTenantTUserDTO>
    {
        public AmigoTenantTUserMapping()
        {
            PrimaryKey(x => x.AmigoTenantTUserId);

            TableName("vwAmigoTenantTUser");

            Columns(x =>
            {
                x.Column(y => y.AmigoTenantTUserId);
                x.Column(y => y.TableStatus).Ignore();
                x.Column(y => y.FirstName);
                x.Column(y => y.LastName);
                x.Column(y => y.Email).Ignore();
                x.Column(y => y.Password).Ignore();
                x.Column(y => y.Id).Ignore();
                x.Column(y => y.PhoneNumber).Ignore();
                x.Column(y => y.PayByName).Ignore();
                x.Column(y => y.LocationName);
                x.Column(y => y.AmigoTenantTRoleName);
                x.Column(y => y.UserTypeName).Ignore();
                x.Column(y => y.CustomUsername).Ignore();
                x.Column(y => y.DeviceId);
                x.Column(y => y.CellphoneNumber);
                x.Column(y => y.IsAdmin);
                x.Column(y => y.IsAdminModifiedUser).Ignore();
            });
        }
    }
}
