using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    [TestFixture]
    public class ActivityEventLogApplicationServiceTest
    {

        IBus _bus;
        IMapper _mapper;

        private IQueryDataAccess<ActivityEventLogDTO> _repo;
        private ActivityEventLogApplicationService appService;


        [SetUp]
        public void CommonArrangements()
        {
            _bus = A.Fake<IBus>();
            _mapper = A.Fake<IMapper>();

            _repo = A.Fake<IQueryDataAccess<ActivityEventLogDTO>>();
            appService = new ActivityEventLogApplicationService(_bus, _repo, _mapper);

        }


        [Test]
        public async Task ActivityEventLogSearchRequest_Call_PagedList_in_repository()
        {


            var request = new ActivityEventLogSearchRequest()
            {
                //ActivityTypeIds = 0,
                UserName = "root",
                ReportedActivityDateFrom = new DateTime(2017, 2, 3),
                PageSize = 10,
                Page = 1
            };

            A.CallTo(() => _repo.ListPagedAsync(null, 0, 0, null, null))
                .WithAnyArguments()
                .Returns(Task.FromResult(new Query.Common.PagedList<ActivityEventLogDTO>()));
            //Act
            var resp = await appService.SearchActivityEventLogAll(request);

            //Assert
            Assert.NotNull(resp);
            A.CallTo(() => _repo.ListPagedAsync(null, 0, 0, null, null))
                .WithAnyArguments()
                .MustHaveHappened(Repeated.NoMoreThan.Once);

        }
    }

}