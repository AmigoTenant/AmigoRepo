using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.Services.Security;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System.Threading.Tasks;
using Amigo.Tenant.Commands.Security.AmigoTenantTUsers;
using Amigo.Tenant.Commands.Tracking.Approve;
using Amigo.Tenant.ServiceAgent.IdentityServer;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    [TestFixture]
    public class AmigoTenantTUserApplicationServiceTest
    {

        IBus _bus;
        IMapper _mapper;

        private  IQueryDataAccess<AmigoTenantTUserDTO> _userDataAccess;
        private  IQueryDataAccess<AmigoTenantTUserBasicDTO> _userBasicDataAccess;
        private  IAmigoTenantTRoleApplicationService _AmigoTenantTRoleApplicationService;
        private  IActivityTypeApplicationService _activityTypeService;
        private AmigoTenantTUserApplicationService _appService;
        private IIdentitySeverAgent  _repoIdentitySeverAgent;


        [SetUp]
        public void CommonArrangements()
        {
            _bus = A.Fake<IBus>();
            _mapper = A.Fake<IMapper>();

            _userDataAccess = A.Fake<IQueryDataAccess<AmigoTenantTUserDTO>>();
            _userBasicDataAccess = A.Fake<IQueryDataAccess<AmigoTenantTUserBasicDTO>>();
            _AmigoTenantTRoleApplicationService = A.Fake<IAmigoTenantTRoleApplicationService>();
            _activityTypeService = A.Fake<IActivityTypeApplicationService>();
            _repoIdentitySeverAgent = A.Fake<IIdentitySeverAgent>();

            _appService = new AmigoTenantTUserApplicationService(_bus, _userDataAccess, _userBasicDataAccess, _mapper, _AmigoTenantTRoleApplicationService, _activityTypeService, _repoIdentitySeverAgent);

        }


        [Test]
        public async Task RegisterAmigoTenanttServiceAsync_SendCommand_To_Bus_SendAsync_WhenUserExistsInIS()
        {
            var request = new UserSearchRequest() {AmigoTenantTUserId = 1, UserName = "root"};
            var command = new RegisterAmigoTenantTUserCommand() { };


            A.CallTo(() => _userDataAccess.AnyAsync( null)).WithAnyArguments().Returns(true);
            

            //**********   ACT   **********
            var resp = _appService.Register(new AmigoTenantTUserDTO());


            A.CallTo(() => _bus.SendAsync(command)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);
        }




    }
}
