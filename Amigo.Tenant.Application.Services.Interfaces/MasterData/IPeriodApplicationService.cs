﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Application.Services.Interfaces.MasterData
{
    public interface IPeriodApplicationService
    {
        Task<ResponseDTO<List<PeriodDTO>>> GetPeriodAllAsync();
        Task<ResponseDTO<PeriodDTO>> GetPeriodByCodeAsync(string code);
        Task<ResponseDTO<PeriodDTO>> GetPeriodByIdAsync(int? id);
        Task<ResponseDTO<PeriodDTO>> GetPeriodBySequenceAsync(int? sequence);
        Task<ResponseDTO<PeriodDTO>> GetLastPeriodAsync();
        Task<ResponseDTO<List<PeriodDTO>>> SearchForTypeAhead(string search);
        Task<ResponseDTO<PeriodDTO>> GetCurrentPeriodAsync();
        Task<ResponseDTO<List<PeriodDTO>>> GetPeriodLastPeriodsAsync(int periodNumber);
        Task<ResponseDTO<List<PeriodDTO>>> GetPeriodsByYearAsync(int? year);
        Task<ResponseDTO<List<YearDTO>>> GetYearsFromPeriodsAsync();
        Task<PeriodDTO> GetInProcessPeriodAsync();
    }
}
