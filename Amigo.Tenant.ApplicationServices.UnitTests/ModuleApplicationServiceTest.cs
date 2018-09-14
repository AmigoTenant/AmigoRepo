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
using Amigo.Tenant.Commands.Security.Module;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    [TestFixture]
    public class ModuleApplicationServiceTest
    {

        #region Properties

        IBus _bus;
        IMapper _mapper;
        IQueryDataAccess<ModuleDTO> _moduleDataAccess;
        IQueryDataAccess<ActionDTO> actionDataAccess;
        ModuleApplicationService moduleService;

        #endregion

        [Test]
        public async Task SearchModules_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();
            var searchRequest = new ModuleSearchRequest();

            //--------------    Act     -------------
            var resp = await moduleService.SearchModulesAsync(searchRequest);

            //--------------    Assert     -------------

            A.CallTo(() => _moduleDataAccess.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened();
        }




        //[Test]
        //public async Task GetModule_Call()
        //{
        //    //--------------    Arrange     -------------


        //    //--------------    Act     -------------


        //    //--------------    Assert     -------------

        //    Assert.AreEqual(1, 2);
        //}



        [Test]
        public async Task RegisterModule_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            var request = new RegisterModuleRequest();
            var command = new RegisterModuleCommand();

            A.CallTo(() => _mapper.Map<RegisterModuleRequest, RegisterModuleCommand>(request)).Returns(command);


            //--------------    Act     -------------
            var resp = moduleService.RegisterModuleAsync(request);

            //--------------    Assert     -------------

            A.CallTo(() => _bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);
        }



        [Test]
        public async Task UpdateModule_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            var request = new UpdateModuleRequest();
            var command = new UpdateModuleCommand();

            A.CallTo(() => _mapper.Map<UpdateModuleRequest, UpdateModuleCommand>(request)).Returns(command);


            //--------------    Act     -------------
            var resp = moduleService.UpdateModuleAsync(request);

            //--------------    Assert     -------------

            A.CallTo(() => _bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);
        }




        [Test]
        public async Task DeleteModule_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            var request = new DeleteModuleRequest();
            var command = new DeleteModuleCommand();

            A.CallTo(() => _mapper.Map<DeleteModuleRequest, DeleteModuleCommand>(request)).Returns(command);


            //--------------    Act     -------------
            var resp = moduleService.DeleteModuleAsync(request);

            //--------------    Assert     -------------

            A.CallTo(() => _bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);
        }


        #region helper

        public void CommonArrangements()
        {
            _bus = A.Fake<IBus>();
            _mapper = A.Fake<IMapper>();
            _moduleDataAccess = A.Fake<IQueryDataAccess<ModuleDTO>>();
            actionDataAccess = A.Fake<IQueryDataAccess<ActionDTO>>();
            moduleService = new ModuleApplicationService(_bus, _moduleDataAccess, actionDataAccess, _mapper);
        }

        #endregion
    }
}
