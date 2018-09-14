using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Security;
using Amigo.Tenant.Commands.Security.Device;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    [TestFixture]
    public class DeviceApplicationServiceTest
    {


        #region Properties

        IBus bus;
        IMapper mapper;

        IQueryDataAccess<DeviceDTO> deviceDataAccess;
        IQueryDataAccess<ModelDTO> modelDataAccess;
        IQueryDataAccess<OSVersionDTO> osVersionDataAccess;
        IQueryDataAccess<AppVersionDTO> appVersionDataAccess;

        DeviceApplicationService deviceService;

        #endregion


        [Test]
        public async Task SearchDevice_Call()
        {
            //--------------    Arrange     -------------

            CommonArrangements();
            var searchRequest = new DeviceSearchRequest();

            //--------------    Act     -------------

            var resp = await deviceService.SearchDeviceAsync(searchRequest);

            //--------------    Assert     -------------

            A.CallTo(() => deviceDataAccess.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened();
        }



        [Test]
        public async Task RegisterDevice_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            var request = new RegisterDeviceRequest();
            var command = new RegisterDeviceCommand();

            A.CallTo(() => mapper.Map<RegisterDeviceRequest, RegisterDeviceCommand>(request)).Returns(command);

            //--------------    Act     -------------
            var resp = deviceService.RegisterDeviceAsync(request);

            //--------------    Assert     -------------

            A.CallTo(() => bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);

        }


        [Test]
        public async Task UpdateDevice_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            var request = new UpdateDeviceRequest();
            var command = new UpdateDeviceCommand();

            A.CallTo(() => mapper.Map<UpdateDeviceRequest, UpdateDeviceCommand>(request)).Returns(command);

            //--------------    Act     -------------
            var resp = deviceService.UpdateDeviceAsync(request);

            //--------------    Assert     -------------

            A.CallTo(() => bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public async Task DeleteDevice_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            var request = new DeleteDeviceRequest();
            var command = new DeleteDeviceCommand();

            A.CallTo(() => mapper.Map<DeleteDeviceRequest, DeleteDeviceCommand>(request)).Returns(command);


            //--------------    Act     -------------
            var resp = deviceService.DeleteDeviceAsync(request);

            //--------------    Assert     -------------

            A.CallTo(() => bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);
        }



        [Test]
        public async Task GetModels_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            //--------------    Act     -------------
            var resp = await deviceService.GetModelsAsync();

            //--------------    Assert     -------------

            A.CallTo(() => modelDataAccess.ListAsync(null, null, null)).WithAnyArguments().MustHaveHappened();
        }




        [Test]
        public async Task GetOSVersions_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            //--------------    Act     -------------
            var resp = await deviceService.GetOSVersionsAsync();

            //--------------    Assert     -------------

            A.CallTo(() => osVersionDataAccess.ListAsync(null, null, null)).WithAnyArguments().MustHaveHappened();
        }

        [Test]
        public async Task GetAppVersions_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            //--------------    Act     -------------
            var resp = await deviceService.GetAppVersionsAsync();

            //--------------    Assert     -------------

            A.CallTo(() => appVersionDataAccess.ListAsync(null, null, null)).WithAnyArguments().MustHaveHappened();
        }

        #region helper

        public void CommonArrangements()
        {
            bus = A.Fake<IBus>();
            mapper = A.Fake<IMapper>();

            deviceDataAccess = A.Fake<IQueryDataAccess<DeviceDTO>>();
            modelDataAccess = A.Fake<IQueryDataAccess<ModelDTO>>();
            osVersionDataAccess = A.Fake<IQueryDataAccess<OSVersionDTO>>();
            appVersionDataAccess = A.Fake<IQueryDataAccess<AppVersionDTO>>();

            deviceService = new DeviceApplicationService(bus, deviceDataAccess, modelDataAccess, osVersionDataAccess,
                                                            appVersionDataAccess, mapper);
        }

        #endregion

    }
}
