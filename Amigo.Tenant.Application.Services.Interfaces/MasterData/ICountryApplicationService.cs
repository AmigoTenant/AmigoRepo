using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.Interfaces.MasterData
{
    public interface ICountryApplicationService
    {
        Task<ResponseDTO<List<CountryDTO>>> GetCountrieAll();
    }
}
