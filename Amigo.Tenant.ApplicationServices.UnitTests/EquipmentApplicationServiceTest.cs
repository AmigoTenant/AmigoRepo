using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Tracking;
using Amigo.Tenant.Commands.Security.AmigoTenantTUsers;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{

    [TestFixture]
    public class EquipmentApplicationServiceTest
    {

        IBus _bus;
        IMapper _mapper;

        private IQueryDataAccess<EquipmentDTO> repo;
        private EquipmentApplicationService appService;

        
        [SetUp]
        public void CommonArrangements()
        {
            _bus = A.Fake<IBus>();
            _mapper = A.Fake<IMapper>();

            repo = A.Fake<IQueryDataAccess<EquipmentDTO>>();
            appService = new EquipmentApplicationService(_bus, repo, _mapper);

        }

        [Test]
        public async Task SearchEquipment_Call_PagedList_in_repository()
        {

            var request = new EquipmentSearchRequest() { EquipmentNo = "PAPU0000004", Page = 1, PageSize = 20};

           A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().Returns(Task.FromResult(new Query.Common.PagedList<EquipmentDTO>() { PageSize = 1, Page = 1, Total = 1, Items = new List<EquipmentDTO>() { new EquipmentDTO() { EquipmentId = 666} } }));

            //Act
            var resp = await appService.SearchEquipmentAsync(request);

            //Assert

            Assert.NotNull(resp);
            // A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened();
            Assert.AreEqual( resp.Messages.First().Key, "Info");
           
        }

        [Test]
        public async Task SearchEquipment_Call_PagedList_in_repository_Empty()
        {

            var request = new EquipmentSearchRequest() { EquipmentNo = "PAPU0000004", Page = 1, PageSize = 20 };

            A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().Returns(Task.FromResult(new Query.Common.PagedList<EquipmentDTO>() { PageSize = 1, Page = 1, Total = 1, Items = new List<EquipmentDTO>() { } }));

            //Act
            var resp = await appService.SearchEquipmentAsync(request);

            //Assert
            
            // A.CallTo(() => repo.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened();
            Assert.AreEqual(resp.Messages.First().Key, "Info");
            Assert.AreEqual(resp.Messages.First().Message, "No records found for this request");
        }


    }
}
