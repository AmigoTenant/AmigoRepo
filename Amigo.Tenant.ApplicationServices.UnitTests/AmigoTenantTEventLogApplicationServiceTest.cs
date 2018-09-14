using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.Services.Tracking;
using Amigo.Tenant.Commands.Tracking.AmigoTenanttEventLog;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    [TestFixture]
    public class AmigoTenantTEventLogApplicationServiceTest
    {

        #region Properties

        IBus _bus;
        IMapper _mapper;
        IQueryDataAccess<AmigoTenantTEventLogDTO> logDataAccess;
        IQueryDataAccess<AmigoTenantTEventLogSearchResultDTO> logSearchDataAccess;
        IActivityTypeApplicationService activityTypeApplicationService;
        AmigoTenantTEventLogApplicationService amigoTenantTEventLogApplicationService;

        #endregion

        [SetUp]
        public void CommonArrangements()
        {
            _bus = A.Fake<IBus>();
            _mapper = A.Fake<IMapper>();

            logDataAccess = A.Fake<IQueryDataAccess<AmigoTenantTEventLogDTO>>();
            logSearchDataAccess = A.Fake<IQueryDataAccess<AmigoTenantTEventLogSearchResultDTO>>();
            activityTypeApplicationService = A.Fake<IActivityTypeApplicationService>();
            amigoTenantTEventLogApplicationService = new AmigoTenantTEventLogApplicationService(_bus, logDataAccess, logSearchDataAccess, activityTypeApplicationService, _mapper);

        }


        [Test]
        public async Task Search_Call()
        {
            //--------------    Arrange     -------------

            //CommonArrangements();
            var searchRequest = new AmigoTenantTEventLogSearchRequest();

            //--------------    Act     -------------
            var resp = await amigoTenantTEventLogApplicationService.SearchAsync(searchRequest);


            //--------------    Assert     -------------
            A.CallTo(() => logSearchDataAccess.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened();

        }

        [Test]
        public async  Task RegisterAmigoTenanttServiceAsync_SendCommand_To_Bus_SendAsync()
        {
            DateTime baseTime = new DateTime(2017, 01, 11, 7, 0, 0);
            DateTimeOffset sourceTime = new DateTimeOffset(baseTime, TimeZoneInfo.Local.GetUtcOffset(baseTime));

            var request = new AmigoTenantTEventLogDTO() { ActivityCode  ="001" , ReportedActivityTimeZone = "America/New_York", ReportedActivityDate= sourceTime };
            var command = new RegisterAmigoTenanttEventLogCommand();

            var model =  activityTypeApplicationService.SearchActivityByCodeAsync(request.ActivityCode).Result.ActivityTypeId;

            DateTimeZone tz = DateTimeZoneProviders.Tzdb.GetZoneOrNull(request.ReportedActivityTimeZone);


            A.CallTo(() => _mapper.Map<AmigoTenantTEventLogDTO, RegisterAmigoTenanttEventLogCommand>(request)).Returns(command);

            //**********   ACT   **********
            var resp = amigoTenantTEventLogApplicationService.RegisterAmigoTenantTEventLogAsync(request);

            //**********   ASSERT    **********
            A.CallTo(() => _bus.SendAsync(command)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(resp);




        }



    }
}
