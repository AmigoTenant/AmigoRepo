using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class MainMenuMapping : Map<MainMenuDTO>
    {
        public MainMenuMapping()
        {

            TableName("vwMainMenu");

            Columns(x =>
            {
                x.Column(y => y.UserId);
                x.Column(y => y.RoleId);
                x.Column(y => y.ActionId);
                x.Column(y => y.ActionCode);
                x.Column(y => y.ModuleId);
                x.Column(y => y.ModuleName);
                x.Column(y => y.ParentModuleId);
                x.Column(y => y.ParentModuleName);
                x.Column(y => y.Url);
                x.Column(y => y.SortOrder);
                x.Column(y => y.ParentSortOrder);
                x.Column(y => y.ModuleCode);
                x.Column(y => y.ParentModuleCode);
            });
        }
    }
}
