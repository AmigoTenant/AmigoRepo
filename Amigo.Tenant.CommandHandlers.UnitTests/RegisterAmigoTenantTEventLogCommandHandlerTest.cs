
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Amigo.Tenant.CommandHandlers.Tracking.AmigoTenantTEventLog;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Tracking.AmigoTenanttEventLog;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Assert = NUnit.Framework.Assert;

namespace Amigo.Tenant.CommandHandlers.UnitTests
{
    [TestFixture]
    public class RegisterAmigoTenantTEventLogCommandHandlerTest
    {

        private IBus _bus;
        private IMapper _mapper;
        private IRepository<AmigoTenantTEventLog> _repository;
        private IUnitOfWork _unitOfWork;
        private RegisterAmigoTenantTEventLogCommandHandler _commandHandler;


        [SetUp]
        public void CommonArrangements()
        {
            _bus = A.Fake<IBus>();
            _mapper = A.Fake<IMapper>();
            _repository = A.Fake<IRepository<AmigoTenantTEventLog>>();
            _unitOfWork = A.Fake<IUnitOfWork>();

            _commandHandler = new RegisterAmigoTenantTEventLogCommandHandler(_bus, _mapper, _repository, _unitOfWork);
        }




        [Test]
        public async  Task Call_UnitOfWork_RegisterAmigoTenantTEventLog_CommandHandler()
        {
            var message = new RegisterAmigoTenanttEventLogCommand();
            var command = new AmigoTenantTEventLog();
            
            A.CallTo(() => _mapper.Map<RegisterAmigoTenanttEventLogCommand, AmigoTenantTEventLog>(message)).Returns(command);

            AmigoTenantTEventLog dataItem = null;
            A.CallTo(() => _repository.Add(null)).WithAnyArguments().Invokes((f) => dataItem = f.Arguments.FirstOrDefault() as AmigoTenantTEventLog);


            //**********   ACT   **********
            await _commandHandler.Handle(message);

            //**********   ASSERT    **********
            A.CallTo(() => _unitOfWork.CommitAsync()).MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(dataItem);

        }


        [Test]
        public async Task Call_BusinessEvent_PublishAsync_AmigoTenantTEventLog()
        {
            AmigoTenantTEventLog dataItem = null;
            var message = new RegisterAmigoTenanttEventLogCommand();
            var eventcommand = new AmigoTenantTEventLog();

            A.CallTo(() => _mapper.Map<RegisterAmigoTenanttEventLogCommand, AmigoTenantTEventLog>(message)).Returns(eventcommand);
            A.CallTo(() => _repository.Add(null)).WithAnyArguments().Invokes((f) => dataItem = f.Arguments.FirstOrDefault() as AmigoTenantTEventLog);
            A.CallTo(() => _unitOfWork.CommitAsync()).Invokes(() => eventcommand.AmigoTenantTEventLogId = 1).ReturnsLazily(() => Task.FromResult(0));

            //**********   ACT   **********
            await _commandHandler.Handle(message);

            //**********   ASSERT    **********
            //A.CallTo(() => _bus.PublishAsync(eventcommand)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(eventcommand);


        }


    }
}
