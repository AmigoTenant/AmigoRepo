using System;
using FakeItEasy;
using NUnit.Framework;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.Services.Tracking;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    [TestFixture]
    public class MovesApplicationServiceTest
    {
        [Test]
        public void RegisterMove_SendCommand_to_Bus()
        {
            //Arrange
            var bus = A.Fake<IBus>();
            var mapper = A.Fake<IMapper>();            
            var appService = new MovesApplicationService(bus,mapper);
            var request = new RegisterMoveRequest();
            var command = new RegisterMoveCommand();

            A.CallTo(() => mapper.Map<RegisterMoveRequest,RegisterMoveCommand>(request)).Returns(command);

            //Act
            var resp = appService.RegisterAsync(request);

            //Assert
            A.CallTo(() => bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);            
        }
    }
}
