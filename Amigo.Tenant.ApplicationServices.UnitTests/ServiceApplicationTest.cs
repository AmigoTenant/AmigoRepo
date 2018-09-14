
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
    public class ServiceApplicationTest
    {

        [Test]
        public async Task SearchService_Call_PagedList_in_repository()
        {
            //Arrange
            var bus = A.Fake<IBus>();
            var mapper = A.Fake<IMapper>();
            var repo = A.Fake<IQueryDataAccess<ServiceDTO>>();

            var appService = new ServiceApplicationService(bus, repo, mapper);
            var page = 1;
            var pageSize = 10;

            var request = new ServiceSearchRequest() { ServiceTypeCode = "service", Page=page,PageSize=pageSize};            

            A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().Returns(Task.FromResult(new Query.Common.PagedList<ServiceDTO>()));

            //Act
            var resp = await appService.SearchServiceByNameAsync(request);                        

            //Assert
            Assert.NotNull(resp);
            A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened(Repeated.NoMoreThan.Once);
        }       

    }
}
