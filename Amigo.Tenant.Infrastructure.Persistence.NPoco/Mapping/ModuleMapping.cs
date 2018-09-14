using NPoco.FluentMappings;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ModuleMapping : Map<ModuleDTO>
    {
        public ModuleMapping()
        {

            TableName("vwModule");

            Columns(x =>
            {
                x.Column(y => y.Code);
                x.Column(y => y.Name);
                x.Column(y => y.Url);
                x.Column(y => y.ParentModuleCode);
                x.Column(y => y.ParentModuleName);
                x.Column(y => y.SortOrder);
                x.Column(y => y.OnlyParents).Ignore();
                
            });
        }
    }
}
