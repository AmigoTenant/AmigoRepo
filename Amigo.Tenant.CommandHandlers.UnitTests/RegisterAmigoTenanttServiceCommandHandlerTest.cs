
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using NUnit.Framework;
using Amigo.Tenant.Application.DTOs.Responses.Move;
using Amigo.Tenant.CommandHandlers.Tracking.Moves;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Events.Tracking;

namespace Amigo.Tenant.CommandHandlers.UnitTests
{

    [TestFixture]
    public class RegisterAmigoTenanttServiceCommandHandlerTest
    {
        private IBus _bus;
        private IMapper _mapper;
        private IRepository<AmigoTenantTService> _repository;
        private IUnitOfWork _unitOfWork;
        private RegisterAmigoTenanttServiceCommandHandler _commandHandler;


        [SetUp]
        public void CommonArrangements()
        {
            _bus = A.Fake<IBus>();
            _mapper = A.Fake<IMapper>();
            _repository = A.Fake<IRepository<AmigoTenantTService>>();
            _unitOfWork = A.Fake<IUnitOfWork>();

            _commandHandler = new RegisterAmigoTenanttServiceCommandHandler(_bus, _mapper, _repository, _unitOfWork);
        }

        [Test]
        public async Task Call_UnitOfWork_and_include_audit_fields()
        {
            //**********  ARRANGE  **********
            var message = new RegisterAmigoTenanttServiceCommand
            {
                AmigoTenantTUserId = 1,
                UserId = 1
            };
            AmigoTenantTService dataItem = null;
            A.CallTo(() => _repository.Add(null))
                .WithAnyArguments()
                .Invokes((f) => dataItem = f.Arguments.FirstOrDefault() as AmigoTenantTService);

            //**********   ACT   **********
            await _commandHandler.Handle(message);

            //**********   ASSERT    **********
            A.CallTo(() => _unitOfWork.CommitAsync()).MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(dataItem);
            Assert.AreEqual(dataItem.AmigoTenantTUserId, message.AmigoTenantTUserId);
        }

        [Test]
        public async Task Call_BusinessEvent_PublishAsync_RegisterMoveEvent()
        {
            var eventcommand = new RegisterMoveEvent();
            var command = new AmigoTenantTService()
            {
                ChargeType = "FAKE"
            };
            //---->  Include Changes
            var message = new RegisterAmigoTenanttServiceCommand
            {
                AmigoTenantTUserId = 1,
                ActivityTypeId = 1,
                Username = "root",
                AmigoTenantTServiceId = 1
            };

            A.CallTo(() => _mapper.Map<RegisterAmigoTenanttServiceCommand, AmigoTenantTService>(message)).Returns(command);
            A.CallTo(() => _mapper.Map<RegisterAmigoTenanttServiceCommand, RegisterMoveEvent>(message)).Returns(eventcommand);
            A.CallTo(() => _unitOfWork.CommitAsync())
                .Invokes(() => command.AmigoTenantTServiceId = 1)
                .ReturnsLazily(() => Task.FromResult(0));

            //**********   ACT   **********
            await _commandHandler.Handle(message);

            //**********   ASSERT    **********
            A.CallTo(() => _bus.PublishAsync(eventcommand)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);
            Assert.NotNull(eventcommand);
            Assert.IsNotNull(message);

        }

        

    }
}
