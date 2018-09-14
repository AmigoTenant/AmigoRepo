using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.Services.Tracking;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    /// <summary>
    /// Summary description for DispatchingPartyApplicationServiceTest
    /// </summary>
    [TestFixture]
    public class DispatchingPartyApplicationServiceTest
    {

        
        [Test]
        public async Task SearchDispatchingPartyAsync_PagedList_in_repository()
        {
            var bus = A.Fake<IBus>();
            var mapper = A.Fake<IMapper>();
            var repo = A.Fake<IQueryDataAccess<DispatchingPartyDTO>>();

            var appService = new DispatchingPartyApplicationService(bus, repo, mapper);
            var page = 1;
            var pageSize = 10;


            var request = new DispatchingPartySearchRequest() { Name = "DOW", Page = page, PageSize = pageSize };

            A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null))
                      .WithAnyArguments().Returns(Task.FromResult(new Query.Common.PagedList<DispatchingPartyDTO>()));

            //Act
            var resp = await appService.SearchDispatchingPartyAsync(request);

            //Assert
            Assert.NotNull(resp);
            A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened(Repeated.NoMoreThan.Once);


        }

        [Test]
        public async Task DispatchingPartAllAsync_in_repository()
        {

            var bus = A.Fake<IBus>();
            var mapper = A.Fake<IMapper>();
            var repo = A.Fake<IQueryDataAccess<DispatchingPartyDTO>>();

            var  appService = new DispatchingPartyApplicationService(bus, repo, mapper);

            //arrange
            A.CallTo(() => repo.ListAllAsync(null, null)).WithAnyArguments().Returns(new List<DispatchingPartyDTO>());            

            //Act
            var resp = await appService.GetAllAsync();

            //Assert
            Assert.NotNull(resp);
            A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened(Repeated.NoMoreThan.Once);


        }






    }
}
