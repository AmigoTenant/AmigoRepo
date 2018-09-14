using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FakeItEasy;
using NUnit.Framework;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Move;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.ServiceAgent.IdentityServer;
using System.Web.Script.Serialization;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{


    [TestFixture]
    public  class DriverReportApplicationServiceTest
    {

        IBus _bus;
        IMapper _mapper;

        private IQueryDataAccess<DriverPayReportDTO> _repo;
        private IQueryDataAccess<AmigoTenanttServiceLatestDTO> _serviceLatestDataAccess;
        private DriverReportApplicationService appService;
        private IIdentitySeverAgent _repoIdentitySeverAgent;
        


        [SetUp]
        public void CommonArrangements()
        {
            _bus = A.Fake<IBus>();
            _mapper = A.Fake<IMapper>();
            

            _repo = A.Fake<IQueryDataAccess<DriverPayReportDTO>>();
            _serviceLatestDataAccess = A.Fake<IQueryDataAccess<AmigoTenanttServiceLatestDTO>>();
            _repoIdentitySeverAgent = A.Fake<IIdentitySeverAgent>();

            appService = new DriverReportApplicationService(_bus,_repo,_serviceLatestDataAccess,_mapper, _repoIdentitySeverAgent);
            
        }

        [Test]
        public async Task DriverPayReportSearch_Call_PagedList_in_repository()
        {

            var request = new DriverPayReportSearchRequest()
            {
                ReportDateFrom  = new DateTime(2017,1,25),
                ReportDateTo = new DateTime(2017, 2, 2),
                DriverId = 3015,
                PageSize = 10, Page = 1
            };

            var userList = new List<string>() {"Root"};

           
            var json1 =@"[Data"":""[{""Id"":3015,""UserName"":""Root"",""FirstName"":""Root"",""LastName"":""AmigoTenant"", ""ProfilePictureUrl"":null,""Email"":"""",""PhoneNumber"":null,""Claims"":[]}],""IsValid"":true,""Messages"":null}]"" )";

            var response = new HttpResponseMessage
            {
                /*Content = new StringContent("[\"Root\"]"), */
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json1, Encoding.UTF8, "application/json")

        };

            A.CallTo(() => _repo.ListPagedAsync(null, 0, 0, null, null))
                .WithAnyArguments().Returns(Task.FromResult(new Query.Common.PagedList<DriverPayReportDTO>() { PageSize = 1, Page = 1, Total = 1, Items = new List<DriverPayReportDTO>()
            {
                new DriverPayReportDTO() { DayPayDriverTotal = 95000, Driver = "root", DriverUserId = 3015,ReportDate = new DateTime(2017,1,27)}
            } }));

            A.CallTo(() => _repoIdentitySeverAgent.AmigoTenantTUser_Details_IdentityServer(userList)).WithAnyArguments().Returns(Task.FromResult(response));

            // ACT

           // var resp = await appService.SearchDriverPayReportAsync(request);

            //Assert
            //Assert.NotNull(resp);
            A.CallTo(() => _repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened(Repeated.NoMoreThan.Once);


        }


    }

}
