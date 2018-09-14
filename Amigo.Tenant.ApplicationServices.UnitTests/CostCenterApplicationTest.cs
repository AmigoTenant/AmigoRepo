
using FakeItEasy;
using NUnit.Framework;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.Services.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    [TestFixture]
    public class CostCenterApplicationTest
    {

        [Test]
        public async Task SearchCostCenter_Call_PagedList_in_repository()
        {

            //Arrange
            var bus = A.Fake<IBus>();
            var mapper = A.Fake<IMapper>();
            var repo = A.Fake<IQueryDataAccess<CostCenterDTO>>();

            var appService = new CostCenterApplicationService(bus, repo, mapper);
            var page = 1;
            var pageSize = 10;

            var request = new CostCenterSearchRequest() {Name="xxxxx", Page=page,PageSize=pageSize};            
            A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().Returns(Task.FromResult(new Query.Common.PagedList<CostCenterDTO>()));

            //Act
            var resp = await appService.SearchCostCenterByNameAsync(request);

            //Assert
            Assert.NotNull(resp);
            A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened(Repeated.NoMoreThan.Once);
        }    
        
        
        
           

    }
}
