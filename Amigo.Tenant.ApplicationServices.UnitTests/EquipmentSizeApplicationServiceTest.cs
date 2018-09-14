using FakeItEasy;
using NUnit.Framework;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.Services.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using System.Threading.Tasks;
namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    [TestFixture]
    public class EquipmentSizeApplicationServiceTest
    {

        [Test]
        public async Task SearchEquipmentSize_Call_PagedList_in_Repository() {

            var bus = A.Fake<IBus>();
            var mapper = A.Fake<IMapper>();
            var repo = A.Fake<IQueryDataAccess<EquipmentSizeDTO>>();

            var appService = new EquipmentSizeApplicationService(bus, repo, mapper);
            var page = 1;
            var pageSize = 10;

            var request = new EquipmentSizeSearchRequest() { Name = "equi", Page = page, PageSize = pageSize };

            A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().Returns(Task.FromResult(new Query.Common.PagedList<EquipmentSizeDTO>()));

            //Act
            var resp = await appService.SearchEquipmentSizeAsync(request);

            //Assert
            Assert.NotNull(resp);
            A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened(Repeated.NoMoreThan.Once);

        }


    }
}
