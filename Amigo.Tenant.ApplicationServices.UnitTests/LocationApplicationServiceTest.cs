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
using Amigo.Tenant.Commands.Tracking.Location;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.ApplicationServices.UnitTests
{
    [TestFixture]
    public class LocationApplicationServiceTest
    {
        #region Properties

        IBus bus;
        IMapper mapper;
        IQueryDataAccess<LocationDTO> locationDataAccess;
        IQueryDataAccess<LocationCoordinateDTO> locationCoordinateDataAccess;
        IQueryDataAccess<LocationTypeDTO> locationTypeDataAccess;
        IQueryDataAccess<ParentLocationDTO> parentlocationDataAccess;
        IQueryDataAccess<CityDTO> cityDataAccess;
        LocationApplicationService locationService;

        #endregion

        [Test]
        public async Task SearchLocation_Call()
        {
            //--------------    Arrange     -------------

            CommonArrangements();
            var searchRequest = new LocationSearchRequest();

            //--------------    Act     -------------
            var resp = await locationService.SearchLocationAsync(searchRequest);


            //--------------    Assert     -------------
            A.CallTo(() => locationDataAccess.ListPagedAsync(null, 0, 0, null, null)).WithAnyArguments().MustHaveHappened();

        }



        //[Test]
        //public async Task GetLocation_Call()
        //{
        //    //--------------    Arrange     -------------


        //    //--------------    Act     -------------


        //    //--------------    Assert     -------------

        //    Assert.AreEqual(1, 2);
        //}



        [Test]
        public async Task ListCoordinates_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();
            var searchRequest = new LocationCoordinatesListRequest();

            //--------------    Act     -------------
            var resp = await locationService.ListCoordinatesAsync(searchRequest);


            //--------------    Assert     -------------

            A.CallTo(() => locationCoordinateDataAccess.ListAsync(null, null, null)).WithAnyArguments().MustHaveHappened();
        }



        [Test]
        public async Task GetCities_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            //--------------    Act     -------------
            var resp = await locationService.GetCitiesAsync();

            //--------------    Assert     -------------

            A.CallTo(() => cityDataAccess.ListAsync(null, null, null)).WithAnyArguments().MustHaveHappened();
        }


        [Test]
        public async Task GetLocationTypes_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            //--------------    Act     -------------
            var resp = await locationService.GetLocationTypesAsync();

            //--------------    Assert     -------------

            A.CallTo(() => locationTypeDataAccess.ListAsync(null, null, null)).WithAnyArguments().MustHaveHappened();
        }


        [Test]
        public async Task GetParentLocations_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            //--------------    Act     -------------
            var resp = await locationService.GetParentLocationsAsync();

            //--------------    Assert     -------------

            A.CallTo(() => parentlocationDataAccess.ListAsync(null, null, null)).WithAnyArguments().MustHaveHappened();
        }



        [Test]
        public async Task RegisterLocation_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            var request = new RegisterLocationRequest();
            var command = new RegisterLocationCommand();

            A.CallTo(() => mapper.Map<RegisterLocationRequest, RegisterLocationCommand>(request)).Returns(command);

            //--------------    Act     -------------
            var resp = locationService.RegisterLocationAsync(request);

            //--------------    Assert     -------------

            A.CallTo(() => bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);
        }



        [Test]
        public async Task UpdateLocation_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            var request = new UpdateLocationRequest();
            var command = new UpdateLocationCommand();

            A.CallTo(() => mapper.Map<UpdateLocationRequest, UpdateLocationCommand>(request)).Returns(command);

            //--------------    Act     -------------
            var resp = locationService.UpdateLocationAsync(request);

            //--------------    Assert     -------------

            A.CallTo(() => bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);
        }



        [Test]
        public async Task DeleteLocation_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            var request = new DeleteLocationRequest();
            var command = new DeleteLocationCommand();

            A.CallTo(() => mapper.Map<DeleteLocationRequest, DeleteLocationCommand>(request)).Returns(command);


            //--------------    Act     -------------
            var resp = locationService.DeleteLocationAsync(request);

            //--------------    Assert     -------------

            A.CallTo(() => bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);
        }



        [Test]
        public async Task RegisterLocationCoordinates_Call()
        {
            //--------------    Arrange     -------------
            CommonArrangements();

            var request = new RegisterLocationCoordinatesRequest();
            request.RegisterLocationCoordinateList = new List<Application.DTOs.Requests.Tracking.RegisterLocationCoordinateItem>();
            request.RegisterLocationCoordinateList.Add(new Application.DTOs.Requests.Tracking.RegisterLocationCoordinateItem() { Latitude = 1, Longitude = 2, LocationCode = "TEST" });

            var command = new RegisterLocationCoordinatesCommand();

            A.CallTo(() => mapper.Map<RegisterLocationCoordinatesRequest, RegisterLocationCoordinatesCommand>(request)).Returns(command);


            //--------------    Act     -------------
            var resp = locationService.RegisterLocationCoordinatesAsync(request);

            //--------------    Assert     -------------
            A.CallTo(() => bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);
        }




        //[Test]
        //public async Task DeleteLocationCoordinates_Call()
        //{
        //    //--------------    Arrange     -------------
        //    CommonArrangements();

        //    var request = new DeleteLocationCoordinatesRequest() {Code = "123" };
        //    var command = new DeleteLocationCoordinatesCommand() { Code = "123" };

        //    //--------------    Act     -------------
        //    A.CallTo(() => mapper.Map<DeleteLocationCoordinatesRequest, DeleteLocationCoordinatesCommand>(request)).Returns(command);

        //    //--------------    Assert     -------------
        //    A.CallTo(() => bus.SendAsync(command)).MustHaveHappened(Repeated.Exactly.Once);

        //}


        #region helper

        public void CommonArrangements()
        {
            bus = A.Fake<IBus>();
            mapper = A.Fake<IMapper>();
            locationDataAccess = A.Fake<IQueryDataAccess<LocationDTO>>();
            locationCoordinateDataAccess = A.Fake<IQueryDataAccess<LocationCoordinateDTO>>();
            parentlocationDataAccess = A.Fake<IQueryDataAccess<ParentLocationDTO>>();
            locationTypeDataAccess = A.Fake<IQueryDataAccess<LocationTypeDTO>>();
            cityDataAccess = A.Fake<IQueryDataAccess<CityDTO>>();

            locationService = new LocationApplicationService(bus, locationDataAccess, parentlocationDataAccess,
                                                                    locationCoordinateDataAccess, locationTypeDataAccess,
                                                                    cityDataAccess, mapper);
        }

        #endregion

    }
}
