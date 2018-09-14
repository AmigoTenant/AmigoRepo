using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class GeneralTableMapping : Map<GeneralTableDTO>
    {
        public GeneralTableMapping()
        {
            TableName("GeneralTable");

            //Columns(x =>
            //{
            //    x.Column(y => y.AmigoTenantTUserId);
            //    x.Column(y => y.EntityStatus).Ignore();
            //    x.Column(y => y.FirstName).Ignore();
            //    x.Column(y => y.LastName).Ignore();
            //    x.Column(y => y.Email).Ignore();
            //    x.Column(y => y.Password).Ignore();
            //    x.Column(y => y.Id).Ignore();
            //    x.Column(y => y.PhoneNumber).Ignore();
            //    x.Column(y => y.PayByName).Ignore();
            //    x.Column(y => y.LocationName);
            //    x.Column(y => y.AmigoTenantTRoleName);
            //    x.Column(y => y.UserTypeName).Ignore();
            //    //x.Column(y => y.RowStatus);
            //});
        }
    }
}
