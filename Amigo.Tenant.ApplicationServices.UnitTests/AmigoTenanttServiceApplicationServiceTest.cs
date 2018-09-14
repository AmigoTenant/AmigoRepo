
using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Move;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Tracking;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Application.DTOs.Responses.Approve;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Common;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Tracking.Approve;
using Amigo.Tenant.Common;
using AmigoTenantTServiceStatus = Amigo.Tenant.Application.DTOs.Requests.Tracking.AmigoTenantTServiceStatus;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    [TestFixture]
    public class AmigoTenanttServiceApplicationServiceTest
    {

        IBus _bus;
        IMapper _mapper;

        IQueryDataAccess<AmigoTenanttServiceDTO> _amigoTenanttServicesDataAccess;
        IQueryDataAccess<AmigoTenantTServiceReportDTO> _repoReport;
        IQueryDataAccess<AmigoTenantTServiceApproveRateDTO> _repoApproveRate;
        IQueryDataAccess<AmigoTenantParameterDTO> _amigoTenantParameter;
        IActivityTypeApplicationService _activityTypeService;
        AmigoTenantServiceApplicationService _appService;
        IQueryDataAccess<CostCenterDTO> _costCenterDataAccess;
        IQueryDataAccess<AmigoTenantTEventLogPerHourDTO> _amigoTenantTEventLogPerHourDataAccess;
        IQueryDataAccess<RateDTO> _rateDataAccess;
        IQueryDataAccess<ProductDTO> _productDataAcces;
        IQueryDataAccess<LocationDTO> _locationDataAccess;

        [SetUp]
        public void CommonArrangements()
        {
            _bus = A.Fake<IBus>();
            _mapper = A.Fake<IMapper>();

            _amigoTenanttServicesDataAccess = A.Fake<IQueryDataAccess<AmigoTenanttServiceDTO>>();
            _repoReport = A.Fake<IQueryDataAccess<AmigoTenantTServiceReportDTO>>();
            _repoApproveRate = A.Fake<IQueryDataAccess<AmigoTenantTServiceApproveRateDTO>>();
            _amigoTenantParameter = A.Fake<IQueryDataAccess<AmigoTenantParameterDTO>>();
            _activityTypeService = A.Fake<IActivityTypeApplicationService>();
            _costCenterDataAccess = A.Fake<IQueryDataAccess<CostCenterDTO>>();
            _amigoTenantTEventLogPerHourDataAccess = A.Fake<IQueryDataAccess<AmigoTenantTEventLogPerHourDTO>>(); //amigoTenantTEventLogPerHourDataAccess;
            _rateDataAccess = A.Fake<IQueryDataAccess<RateDTO>>(); //rateDataAccess;
            _productDataAcces = A.Fake<IQueryDataAccess<ProductDTO>>(); 
            _locationDataAccess = A.Fake<IQueryDataAccess<LocationDTO>>(); 
            _appService = new AmigoTenantServiceApplicationService(_bus, _amigoTenanttServicesDataAccess, _repoReport, _repoApproveRate, _amigoTenantParameter, _activityTypeService, _costCenterDataAccess, _mapper, _amigoTenantTEventLogPerHourDataAccess, _rateDataAccess, _productDataAcces, _locationDataAccess);
        }


        [Test]
        public  void RegisterAmigoTenanttServiceAsync_SendCommand_To_Bus_SendAsync()
        {

            DateTime baseTime = new DateTime(2017, 01, 11, 7, 0, 0);
            DateTimeOffset sourceTime = new DateTimeOffset(baseTime, TimeZoneInfo.Local.GetUtcOffset(baseTime));

            var request = new AmigoTenanttServiceDTO();
            var command = new RegisterAmigoTenanttServiceCommand();
            request.ServiceStartDate = sourceTime;
            request.ServiceStartDateTZ = "America/New_York";
            request.ReportedActivityDate = sourceTime;


            A.CallTo(() => _mapper.Map<AmigoTenanttServiceDTO, RegisterAmigoTenanttServiceCommand>(request)).Returns(command);

            //**********   ACT   **********
            var resp = _appService.RegisterAmigoTenanttServiceAsync(request);

            //**********   ASSERT    **********
            Assert.NotNull(resp);

            A.CallTo(() => _bus.SendAsync(command)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);            
        }

        /*
        [Test]
        public void RegisterAmigoTenanttService_SendCommand_to_Bus()
        {           
            var request = new AmigoTenanttServiceDTO();
            var command = new RegisterAmigoTenanttServiceCommand();
            A.CallTo(() => mapper.Map<AmigoTenanttServiceDTO,RegisterAmigoTenanttServiceCommand>(request)).Returns(command);
            var resp = appService.RegisterAmigoTenanttServiceAsync(request);
           // A.CallTo(() => bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);
        }*/

        [Test]
        public void UpdateAmigoTenantServiceAsync_SendCommand_To_Bus_SendAsync()
        {
            var request = new UpdateAmigoTenantServiceRequest();
            var command = new UpdateAmigoTenantServiceCommand();


            DateTime baseTime = new DateTime(2017, 01, 11, 7, 0, 0);
            DateTimeOffset sourceTime = new DateTimeOffset(baseTime,TimeZoneInfo.Local.GetUtcOffset(baseTime));

            request.AmigoTenantTServiceId = 89;
            request.ServiceFinishDate = sourceTime;
            request.ServiceFinishDateTZ = "America/New_York";
            request.ReportedActivityDate = sourceTime;

            A.CallTo(() => _mapper.Map<UpdateAmigoTenantServiceRequest,UpdateAmigoTenantServiceCommand>(request)).Returns(command);

            //**********   ACT   **********
            var resp = _appService.UpdateAmigoTenantServiceAsync(request);

            //**********   ASSERT    **********
            Assert.NotNull(resp);

            A.CallTo(() => _bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);


        }

        [Test]
        public void UpdateAmigoTenantTServiceForApproveAsync_SendCommand_To_Bus_SendAsync()
        {
            // save SidePanel
            var maintenance = new AmigoTenantTServiceRequest();
            var command = new UpdateAmigoTenantTServiceApproveCommand() {};

            A.CallTo(() => _mapper.Map<AmigoTenantTServiceRequest, UpdateAmigoTenantTServiceApproveCommand>(maintenance)).Returns(command);

            A.CallTo(() => _activityTypeService.SearchActivityByCodeAsync(Constants.ActivityTypeCode.EditbeforeApproval)).Invokes(() => command.ActivityTypeId = 1)
                .ReturnsLazily(()=> Task.FromResult(new ActivityTypeDTO()));

            //**********   ACT   **********
            var resp = _appService.UpdateAmigoTenantTServiceForApproveAsync(maintenance);


            //**********   ASSERT    **********
            Assert.NotNull(resp);

            A.CallTo(() => _bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);

        }

        [Test]
        public async Task ApproveAmigoTenantTServiceAsync_Calculate_By_Hour()
        {
            //Arrange
            IEnumerable<AmigoTenantTServiceApproveRateDTO> rates = new List<AmigoTenantTServiceApproveRateDTO>()
            {
                new AmigoTenantTServiceApproveRateDTO()
                {
                    RateId = 99,
                    AmigoTenantTServiceId = "1",
                    PayDriver = 10,
                    BillCustomer = 100,
                    TotalHours = 1,
                    IsPerHour = 1,
                    PayBy = Constants.RowStatusString.Active
                },
                new AmigoTenantTServiceApproveRateDTO()
                {
                    RateId = 99,
                    AmigoTenantTServiceId = "2",
                    BillCustomer = 100,
                    TotalHours = 1,
                    PayDriver = 10,
                    IsPerHour = 1,
                    PayBy = Constants.RowStatusString.Active
                }
            };
            A.CallTo(() => _repoApproveRate.ListAsync(null,null,null)).WithAnyArguments().ReturnsLazily((c)=> Task.FromResult(rates));

            var request = new AmigoTenantTServiceApproveRequest()
            {
                ApprovedBy = "FMORON",
                UserId = 1,
                ReportDate = DateTime.Now,
                AmigoTenantTServiceIdsListStatus = new List<AmigoTenantTServiceStatus>()
                {
                    new AmigoTenantTServiceStatus()
                    {
                        AmigoTenantTServiceId = "1",                        
                        ServiceStatus = true
                    },
                    new AmigoTenantTServiceStatus()
                    {
                        AmigoTenantTServiceId = "2",
                        ServiceStatus = true
                    }
                }
            };

            RegisterDriverReportCommand commandResult = null;

            A.CallTo(() => _bus.SendAsync(new RegisterDriverReportCommand())).WithAnyArguments().ReturnsLazily(
                (o) =>
                {
                    commandResult = o.Arguments.FirstOrDefault() as RegisterDriverReportCommand;
                    return Task.FromResult(new CommandResult(new List<string>()));
                });

            //Act
            var resp = await _appService.ApproveAmigoTenantTServiceAsync(request);

            //Assert
            Assert.True(resp.IsValid);
            Assert.NotNull(commandResult);
            Assert.AreEqual(commandResult.DayBillCustomerTotal,200);
        }

        [Test]
        public async Task UpdateAmigoTenantTServiceAckAsync_SendCommand_To_Bus_SendAsync()
        {

            var request = new UpdateAmigoTenantTServiceAckRequest()
            {
                ServiceAcknowledgeDateTZ = "America/Lima",
                ServiceAcknowledgeDate = DateTimeOffset.Now
            };
            var command = new UpdateAmigoTenantServiceAckCommand()
            {
                ServiceAcknowledgeDate = new DateTimeOffset(2001, 9, 11, 6, 6, 6, 6, TimeSpan.Zero),
                IsAknowledged = true
            };

            A.CallTo(() => _mapper.Map<UpdateAmigoTenantTServiceAckRequest, UpdateAmigoTenantServiceAckCommand>(request)).Returns(command);
        

            //**********   ACT   **********
           // var resp = _appService.UpdateAmigoTenantTServiceAckAsync(request);

            //**********   ASSERT    **********
       //     Assert.NotNull(resp);

            A.CallTo(() => _bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);



        }


    }

}
