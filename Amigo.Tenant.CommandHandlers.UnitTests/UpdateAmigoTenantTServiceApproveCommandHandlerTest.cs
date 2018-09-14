
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Amigo.Tenant.CommandHandlers.Tracking.Approve;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Tracking.Approve;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.UnitTests
{

    [TestFixture]
    public class UpdateAmigoTenantTServiceApproveCommandHandlerTest
    {

        private IBus _bus;
        private IMapper _mapper;
        private IRepository<AmigoTenantTService> _repository;
        private IUnitOfWork _unitOfWork;
        private UpdateAmigoTenantTServiceApproveCommandHandler _commandHandler;

        [SetUp]
        public void CommonArrangements()
        {
            _bus = A.Fake<IBus>();
            _mapper = A.Fake<IMapper>();
            _repository = A.Fake<IRepository<AmigoTenantTService>>();
            _unitOfWork = A.Fake<IUnitOfWork>();

            _commandHandler = new UpdateAmigoTenantTServiceApproveCommandHandler(_bus, _mapper, _repository, _unitOfWork);
        }

        [Test]
        public async Task Call_UnitOfWork_To_UpdateAmigoTenantTServiceApproveCommandHandler()
        {
            var command = new AmigoTenantTService();
            var message = new UpdateAmigoTenantTServiceApproveCommand();

            A.CallTo(() => _mapper.Map<UpdateAmigoTenantTServiceApproveCommand, AmigoTenantTService>(message)).Returns(command);                       

            //**********   ACT   **********
            await _commandHandler.Handle(message);

            //**********   ASSERT    **********
            A.CallTo(() => _repository.UpdatePartial(null)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => _unitOfWork.CommitAsync()).MustHaveHappened(Repeated.Exactly.Once);            
        }






    }
}
