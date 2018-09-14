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
    public class LatestPositionApplicationServiceTest
    {

        #region Properties

        IBus bus;
        IMapper mapper;
        IQueryDataAccess<LatestPositionDTO> latestPositionDataAccess;
        LatestPositionApplicationService latestPositionApplicationService;

        #endregion


        [Test]
        public async Task Search_Call()
        {
            //--------------    Arrange     -------------

            CommonArrangements();
            var searchRequest = new LatestPositionRequest();

            //--------------    Act     -------------
            var resp = await latestPositionApplicationService.SearchAsync(searchRequest);


            //--------------    Assert     -------------
            A.CallTo(() => latestPositionDataAccess.ListAsync(null, null, null)).WithAnyArguments().MustHaveHappened();

        }


        #region helper

        public void CommonArrangements()
        {
            bus = A.Fake<IBus>();
            mapper = A.Fake<IMapper>();
            latestPositionDataAccess = A.Fake<IQueryDataAccess<LatestPositionDTO>>();
            latestPositionApplicationService = new LatestPositionApplicationService(bus, latestPositionDataAccess, mapper);

        }

        #endregion
    }
}
