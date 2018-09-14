using FakeItEasy;
using NUnit.Framework;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.Services.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Security;
using Amigo.Tenant.Application.DTOs.Requests.Security;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    [TestFixture]
    public class AmigoTenantTRoleApplicationServiceTest
    {

        [Test]
        public async Task SearchAmigoTenantTRoleByCriteriaAsync() {


            var bus = A.Fake<IBus>();
            var mapper = A.Fake<IMapper>();
            var repo = A.Fake<IQueryDataAccess<AmigoTenantTRoleDTO>>();
            var repoBasic = A.Fake<IQueryDataAccess<AmigoTenantTRoleBasicDTO>>();

           // var appService = new AmigoTenantTRoleApplicationService(bus, repo, repoBasic, mapper);
            var page = 1;
            var pageSize = 10;

            var request = new AmigoTenantTRoleSearchRequest() { Name = "test",
                Page = page,
                PageSize = pageSize };

            A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().Returns(Task.FromResult(new Query.Common.PagedList<AmigoTenantTRoleDTO>()));

            //Act
            //var resp = await appService.SearchAmigoTenantTRoleByCriteriaAsync(request);

            //Assert
            //Assert.NotNull(resp);
            A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened(Repeated.NoMoreThan.Once);

        }


    }
}
