using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using Amigo.Tenant.Commands.Tracking.Location;
using System.Linq;
using Amigo.Tenant.Application.Services.Common.Extensions;

namespace Amigo.Tenant.Application.Services.Tracking
{
    public class LocationApplicationService : ILocationApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<LocationDTO> _locationDataAccess;
        private readonly IQueryDataAccess<ParentLocationDTO> _parentLocationDataAccess;
        private readonly IQueryDataAccess<CityDTO> _cityDataAccess;
        private readonly IQueryDataAccess<LocationTypeDTO> _locationTypeDataAccess;
        private readonly IQueryDataAccess<LocationCoordinateDTO> _locationCoordinatesDataAccess;


        public LocationApplicationService(IBus bus,
            IQueryDataAccess<LocationDTO> locationDataAccess,
            IQueryDataAccess<ParentLocationDTO> parentLocationDataAccess,
            IQueryDataAccess<LocationCoordinateDTO> locationCoordinateDataAccess,
            IQueryDataAccess<LocationTypeDTO> locationTypeDataAccess,
            IQueryDataAccess<CityDTO> cityDataAccess,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _locationDataAccess = locationDataAccess;
            _locationCoordinatesDataAccess = locationCoordinateDataAccess;
            _locationTypeDataAccess = locationTypeDataAccess;
            _parentLocationDataAccess = parentLocationDataAccess;
            _cityDataAccess = cityDataAccess;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<List<LocationDTO>>> SearchLocationAll()
        {
            Expression<Func<LocationDTO, bool>> queryFilter = c => true;
            List<OrderExpression<LocationDTO>> orderExpressionList = new List<OrderExpression<LocationDTO>>();
            orderExpressionList.Add(new OrderExpression<LocationDTO>(OrderType.Asc, p => p.Name));

            var list = (await _locationDataAccess.ListAsync(queryFilter, orderExpressionList.ToArray())).ToList();

            return ResponseBuilder.Correct(list);

        }

        public async Task<ResponseDTO<List<LocationTypeAheadDTO>>> SearchLocationAllTypeAhead()
        {
            Expression<Func<LocationDTO, bool>> queryFilter = c => true;

            var list = (await _locationDataAccess.ListAsync(queryFilter)).ToList();
            var typeAheadList = list.Select(x => new LocationTypeAheadDTO() { LocationId = x.LocationId, Code = x.Code, Name = x.Name }).ToList();
            return ResponseBuilder.Correct(typeAheadList);

        }

        public async Task<ResponseDTO<List<LocationTypeAheadDTO>>> SearchLocationAllTypeAhead(string name)
        {
            Expression<Func<LocationDTO, bool>> queryFilter = c => true;
            if (!string.IsNullOrEmpty(name))
            {
                queryFilter = queryFilter.And(p => p.Name.Contains(name));
            }

            var list = (await _locationDataAccess.ListAsync(queryFilter)).ToList();
            var typeAheadList = list.Select(x => new LocationTypeAheadDTO() { LocationId = x.LocationId, Code = x.Code, Name = x.Name }).ToList();
            return ResponseBuilder.Correct(typeAheadList);

        }

        public async Task<ResponseDTO<PagedList<LocationDTO>>> SearchLocationAsync(LocationSearchRequest search)
        {
            List<OrderExpression<LocationDTO>> orderExpressionList = new List<OrderExpression<LocationDTO>>();
            orderExpressionList.Add(new OrderExpression<LocationDTO>(OrderType.Asc, p => p.Name));

            var queryFilter = GetQueryFilter(search);
            
            var location = await _locationDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());
            
            var pagedResult = new PagedList<LocationDTO>()
            {

                Items = location.Items,
                PageSize = location.PageSize,
                Page = location.Page,
                Total = location.Total

            };

            return ResponseBuilder.Correct(pagedResult);
        }


        public async Task<ResponseDTO<LocationWithCoordinatesDTO>> GetLocationAsync(GetLocationRequest getRequest)
        {
            var locationAux = await SearchLocationAsync(new LocationSearchRequest { Code = getRequest.Code, Page = 1, PageSize = 1 });

            var location = locationAux.Data.Items.FirstOrDefault();

            if (location != null)
            {

                var coordinates = await _locationCoordinatesDataAccess.ListAsync(w => w.LocationCode == location.Code);

                var locationWithCoordinates = new LocationWithCoordinatesDTO
                {
                    LocationId = location.LocationId,
                    Name = location.Name,
                    Code = location.Code,
                    ZipCode = location.ZipCode,
                    CreationDate = location.CreationDate,
                    HasGeofence = location.HasGeofence,
                    CountryISOCode = location.CountryISOCode,
                    CountryName = location.CountryName,
                    StateCode = location.StateCode,
                    StateName = location.StateName,
                    CityCode = location.CityCode,
                    CityName = location.CityName,
                    LocationTypeCode = location.LocationTypeCode,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Address1 = location.Address1,
                    Address2 = location.Address2,
                    ParentLocationCode = location.ParentLocationCode,
                    ParentLocationName = location.ParentLocationName,
                    LocationTypeName = location.LocationTypeName,
                    Coordinates = coordinates.ToList()
                };

                return ResponseBuilder.Correct(locationWithCoordinates);
            }
            else
            {
                LocationWithCoordinatesDTO obj = null;
                return ResponseBuilder.Correct(obj);
            }
        }



        public async Task<ResponseDTO<List<LocationCoordinateDTO>>> ListCoordinatesAsync(LocationCoordinatesListRequest search)
        {
            var locationCoordinates = await _locationCoordinatesDataAccess.ListAsync(x => x.LocationCode == search.LocationCode);
            var result = locationCoordinates.ToList();
            return ResponseBuilder.Correct(result);
        }



        public async Task<ResponseDTO<List<CityDTO>>> GetCitiesAsync()
        {

            var cities = await _cityDataAccess.ListAsync(null);
            var result = cities.ToList();
            return ResponseBuilder.Correct(result);
        }



        public async Task<ResponseDTO<List<ParentLocationDTO>>> GetParentLocationsAsync()
        {
            var parentLocations = await _parentLocationDataAccess.ListAsync(null);
            var result = parentLocations.ToList();
            return ResponseBuilder.Correct(result);
        }

        public async Task<ResponseDTO<List<LocationTypeDTO>>> GetLocationTypesAsync()
        {
            var locationTypes = await _locationTypeDataAccess.ListAsync(null);
            var result = locationTypes.ToList();
            return ResponseBuilder.Correct(result);
        }

        public async Task<ResponseDTO> RegisterLocationAsync(RegisterLocationRequest newLocation)
        {

            //Map to Command
            var command = _mapper.Map<RegisterLocationRequest, RegisterLocationCommand>(newLocation);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return resp.ToResponse();
        }

        public async Task<ResponseDTO> UpdateLocationAsync(UpdateLocationRequest location)
        {
            //Map to Command
            var command = _mapper.Map<UpdateLocationRequest, UpdateLocationCommand>(location);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO> DeleteLocationAsync(DeleteLocationRequest location)
        {
            //Map to Command
            var command = _mapper.Map<DeleteLocationRequest, DeleteLocationCommand>(location);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        

        public async Task<ResponseDTO> RegisterLocationCoordinatesAsync(RegisterLocationCoordinatesRequest newCoordinates)
        {
            //Map to Command
            var command = _mapper.Map<RegisterLocationCoordinatesRequest, RegisterLocationCoordinatesCommand>(newCoordinates);
            
            //TODO FAVIO use mapper:
            command.RegisterLocationCoordinatesList = new List<Commands.Tracking.Location.RegisterLocationCoordinateItem>();
            foreach (var coordinate in newCoordinates.RegisterLocationCoordinateList)
            {
                command.RegisterLocationCoordinatesList.Add(new Commands.Tracking.Location.RegisterLocationCoordinateItem()
                {
                    Latitude = coordinate.Latitude,
                    Longitude = coordinate.Longitude,
                    LocationCode = coordinate.LocationCode
                });
            }


            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }



        public async Task<ResponseDTO> DeleteLocationCoordinatesAsync(DeleteLocationCoordinatesRequest location)
        {
            //Map to Command
            var command = _mapper.Map<DeleteLocationCoordinatesRequest, DeleteLocationCoordinatesCommand>(location);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<LocationTypeAheadDTO> SearchLocationByNameAsync(string name)
        {
            Expression<Func<LocationDTO, bool>> queryFilter = c => true;
            if (!string.IsNullOrEmpty(name))
            {
                queryFilter = queryFilter.And(p => p.Name.Contains(name));
            }
            var typeAheadElement = new LocationTypeAheadDTO();
            var location = (await _locationDataAccess.FirstOrDefaultAsync(queryFilter));
            if (location != null)
            {
                typeAheadElement.LocationId = location.LocationId;
                typeAheadElement.Code = location.Code;
                typeAheadElement.Name = location.Name;
            }

            return typeAheadElement;
        }


        #region Helpers

        private Expression<Func<LocationDTO, bool>> GetQueryFilter(LocationSearchRequest search)
        {
            Expression<Func<LocationDTO, bool>> queryFilter = p => true;

            if (!string.IsNullOrEmpty(search.Name))
            {
                queryFilter = queryFilter.And(p => p.Name.Contains(search.Name));
            }

            if (!string.IsNullOrEmpty(search.Code))
            {
                queryFilter = queryFilter.And(p => p.Code == search.Code);
            }

            if (!string.IsNullOrEmpty(search.ZipCode))
            {
                queryFilter = queryFilter.And(p => p.ZipCode == search.ZipCode);
            }


            if (search.HasGeofence != null)
            {
                queryFilter = queryFilter.And(p => p.HasGeofence == search.HasGeofence);
            }


            if (!string.IsNullOrEmpty(search.CountryISOCode))
            {
                queryFilter = queryFilter.And(p => p.CountryISOCode == search.CountryISOCode);
            }


            if (!string.IsNullOrEmpty(search.StateCode))
            {
                queryFilter = queryFilter.And(p => p.StateCode == search.StateCode);
            }



            if (!string.IsNullOrEmpty(search.CityCode))
            {
                queryFilter = queryFilter.And(p => p.CityCode == search.CityCode);
            }

            if (!string.IsNullOrEmpty(search.LocationTypeCode))
            {
                queryFilter = queryFilter.And(p => p.LocationTypeCode == search.LocationTypeCode);
            }

            if (!string.IsNullOrEmpty(search.ParentLocationCode))
            {
                queryFilter = queryFilter.And(p => p.ParentLocationCode == search.ParentLocationCode);
            }

            return queryFilter;
        }


        #endregion
    }
}
