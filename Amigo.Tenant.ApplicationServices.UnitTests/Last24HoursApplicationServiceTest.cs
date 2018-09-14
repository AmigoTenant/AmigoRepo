using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    [TestFixture]
    public class Last24HoursApplicationServiceTest
    {
        #region Properties

        IBus bus;
        IMapper mapper;
        IQueryDataAccess<Last24HoursDTO> last24HoursDataAccess;
        Last24HoursApplicationService last24HoursApplicationService;

        #endregion


        [Test]
        public async Task Search_Call()
        {
            //--------------    Arrange     -------------

            CommonArrangements();
            var searchRequest = new Last24HoursRequest();

            //--------------    Act     -------------
            var resp = await last24HoursApplicationService.SearchAsync(searchRequest);


            //--------------    Assert     -------------
            A.CallTo(() => last24HoursDataAccess.ListAsync(null, null, null)).WithAnyArguments().MustHaveHappened();

        }


        #region helper

        public void CommonArrangements()
        {
            bus = A.Fake<IBus>();
            mapper = A.Fake<IMapper>();
            last24HoursDataAccess = A.Fake<IQueryDataAccess<Last24HoursDTO>>();

            last24HoursApplicationService = new Last24HoursApplicationService(bus, last24HoursDataAccess, mapper);
        }

        #endregion
    }
}
