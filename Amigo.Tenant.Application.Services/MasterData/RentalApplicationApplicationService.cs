using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Commands.MasterData.RentalApplication;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Amigo.Tenant.Application.Services.MasterData
{ 
    public class RentalApplicationApplicationService: IRentalApplicationApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<RentalApplicationSearchDTO> _rentalApplicationSearchDataAccess;
        private readonly IQueryDataAccess<RentalApplicationRegisterRequest> _rentalApplicationDataAccess;
        private readonly IEntityStatusApplicationService _entityStatusApplicationService;
        private readonly IPeriodApplicationService _periodApplicationService;

        public RentalApplicationApplicationService(IBus bus,
            IQueryDataAccess<RentalApplicationSearchDTO> rentalApplicationSearchDataAccess,
            IQueryDataAccess<RentalApplicationRegisterRequest> rentalApplicationDataAccess,
            IEntityStatusApplicationService entityStatusApplicationService,
            IPeriodApplicationService periodApplicationService,
            IMapper mapper
            )
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _rentalApplicationSearchDataAccess = rentalApplicationSearchDataAccess;
            _rentalApplicationDataAccess = rentalApplicationDataAccess;
            _mapper = mapper;
            _entityStatusApplicationService = entityStatusApplicationService;
            _periodApplicationService = periodApplicationService;
        }

        public async Task<ResponseDTO> RegisterRentalApplicationAsync(RentalApplicationRegisterRequest request)
        {

            //request.RentalApplicationCode = await GetNextCode(request);
            //request.PeriodId = await GetPeriodByCode(request.RentalApplicationCode.Substring(2, 6));
            //request.RentalApplicationDate = DateTime.Now;
            //var periodCode = string.Format("YYYY", request.ApplicationDate.Value.Year) + string.Format("MM", request.ApplicationDate.Value.Month);
            //request.PeriodId = await GetPeriodByCode(periodCode);

            //TODO: Matodo para agregar informacion a la tabla Obligation, conceptos del tipo obligations
            //var response = await ValidateEntityRegister(request);

            if (true) //response.IsValid)
            {

                var command = _mapper.Map<RentalApplicationRegisterRequest, RentalApplicationRegisterCommand>(request);

                //command.RentalApplicationStatusId = await GetStatusbyCodeAsync(Constants.EntityCode.RentalApplication, Constants.EntityStatus.RentalApplication.Draft);

                var resp = await _bus.SendAsync(command);
                
                return ResponseBuilder.Correct(resp, command.RentalApplicationId, "");
            }

            return null; // response;
        }

        //private void SetPaymentsPeriod(List<PaymentPeriodRegisterRequest> paymentsPeriod, RentalApplicationRegisterRequest request, PeriodDTO period, int id, bool isLastPeriod)
        //{
        //    var paymentPeriod = new PaymentPeriodRegisterRequest();
        //    paymentPeriod.PaymentPeriodId = id;
        //    paymentPeriod.ConceptId = 15; //"CODE FOR CONCEPT"; //TODO:
        //    paymentPeriod.RentalApplicationId = request.RentalApplicationId;
        //    paymentPeriod.TenantId = request.TenantId;
        //    paymentPeriod.PeriodId = period.PeriodId;
        //    paymentPeriod.PaymentPeriodStatusId = 8; //TODO: PONER EL CODIGO CORRECTO PARA EL CONTRACTDETAILSTATUS
        //    paymentPeriod.RowStatus = true;
        //    paymentPeriod.CreatedBy = request.UserId;
        //    paymentPeriod.CreationDate = DateTime.Now;
        //    paymentPeriod.UpdatedBy = request.UserId;
        //    paymentPeriod.UpdatedDate = DateTime.Now;

        //    if (!isLastPeriod)
        //    {
        //        paymentPeriod.PaymentAmount = Math.Abs(id)==1? CalculateFirstRent(request.BeginDate, request.RentPrice) : request.RentPrice;
        //        paymentPeriod.DueDate = period.DueDate;
        //    }
        //    else
        //    {
        //        paymentPeriod.PaymentAmount = CalculateLastRent(request.EndDate, request.RentPrice);
        //        paymentPeriod.DueDate = request.EndDate;
        //    }

        //    paymentsPeriod.Add(paymentPeriod);
        //}

        private async Task<int?> GetStatusbyCodeAsync(string entityCode, string statusCode)
        {
            var entityStatus = await _entityStatusApplicationService.GetEntityStatusByEntityAndCodeAsync(entityCode, statusCode) ;
            if (entityStatus != null)
            return entityStatus.EntityStatusId.Value;

            return null;
        }

        public async Task<ResponseDTO> UpdateRentalApplicationAsync(RentalApplicationUpdateRequest rentalApplication)
        {
            //Map to Command
            
            var command = _mapper.Map<RentalApplicationUpdateRequest, RentalApplicationUpdateCommand>(rentalApplication);

            //var response = await ValidateEntityUpdate(rentalApplication);

            if (true) //response.IsValid)
            {
                //Execute Command
                var resp = await _bus.SendAsync(command);
                return ResponseBuilder.Correct(resp);
            }

            return null; // response;
        }

        public async Task<ResponseDTO> DeleteRentalApplicationAsync(RentalApplicationDeleteRequest rentalApplication)
        {
            //Map to Command
            var command = _mapper.Map<RentalApplicationDeleteRequest, RentalApplicationDeleteCommand>(rentalApplication);
            
            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO<PagedList<RentalApplicationSearchDTO>>> SearchRentalApplicationAsync(RentalApplicationSearchRequest search)
        {
            List<OrderExpression<RentalApplicationSearchDTO>> orderExpressionList = new List<OrderExpression<RentalApplicationSearchDTO>>();
            orderExpressionList.Add(new OrderExpression<RentalApplicationSearchDTO>(OrderType.Asc, p => p.ApplicationDate));
            Expression<Func<RentalApplicationSearchDTO, bool>> queryFilter = c => true;

            //APPLICATIONDATE
            if (search.ApplicationDateFrom.HasValue && search.ApplicationDateTo.HasValue)
            {
                var toPlusADay = search.ApplicationDateTo.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.ApplicationDate.Value >= search.ApplicationDateFrom);
                queryFilter = queryFilter.And(p => p.ApplicationDate.Value < toPlusADay);
            }
            else if (search.ApplicationDateFrom.HasValue && !search.ApplicationDateTo.HasValue)
            {
                queryFilter = queryFilter.And(p => p.ApplicationDate.Value >= search.ApplicationDateFrom);
            }
            else if (!search.ApplicationDateFrom.HasValue && search.ApplicationDateTo.HasValue)
            {
                var toPlusADay = search.ApplicationDateTo.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.ApplicationDate.Value < toPlusADay);
            }
            //CHECKIN
            if (search.CheckInFrom.HasValue && search.CheckInTo.HasValue)
            {
                var toPlusADay = search.CheckInTo.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.CheckIn.Value >= search.CheckInFrom);
                queryFilter = queryFilter.And(p => p.CheckIn.Value < toPlusADay);
            }
            else if (search.CheckInFrom.HasValue && !search.CheckInTo.HasValue)
            {
                queryFilter = queryFilter.And(p => p.CheckIn.Value >= search.CheckInFrom);
            }
            else if (!search.CheckInFrom.HasValue && search.CheckInTo.HasValue)
            {
                var toPlusADay = search.CheckInTo.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.CheckIn.Value < toPlusADay);
            }
            //CHECKOUT
            if (search.CheckOutFrom.HasValue && search.CheckOutTo.HasValue)
            {
                var toPlusADay = search.CheckOutTo.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.CheckOut.Value >= search.CheckOutFrom);
                queryFilter = queryFilter.And(p => p.CheckOut.Value < toPlusADay);
            }
            else if (search.CheckOutFrom.HasValue && !search.CheckOutTo.HasValue)
            {
                queryFilter = queryFilter.And(p => p.CheckOut.Value >= search.CheckOutFrom);
            }
            else if (!search.CheckOutFrom.HasValue && search.CheckOutTo.HasValue)
            {
                var toPlusADay = search.CheckOutTo.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.CheckOut.Value < toPlusADay);
            }

            if (search.PropertyTypeId.HasValue)
                queryFilter = queryFilter.And(p => p.PropertyTypeId == search.PropertyTypeId);

            if (search.PeriodId.HasValue)
                queryFilter = queryFilter.And(p => p.PeriodId == search.PeriodId);

            if (!string.IsNullOrEmpty(search.Email))
                queryFilter = queryFilter.And(p => p.Email.Contains(search.Email));

            if (!string.IsNullOrEmpty(search.FullName))
                queryFilter = queryFilter.And(p => p.FullName.Contains(search.FullName));


            if (search.ResidenseCountryId.HasValue)
                queryFilter = queryFilter.And(p => p.ResidenseCountryId == search.ResidenseCountryId );

            if (search.BudgetId.HasValue)
                queryFilter = queryFilter.And(p => p.BudgetId == search.BudgetId);

            if (search.ReferredById.HasValue)
                queryFilter = queryFilter.And(p => p.ReferredById == search.ReferredById);

            

            //TODO: City of Interest
            //TODO: Feature

            var rentalApplication = await _rentalApplicationSearchDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());

            var pagedResult = new PagedList<RentalApplicationSearchDTO>()
            {
                Items = rentalApplication.Items,
                PageSize = rentalApplication.PageSize,
                Page = rentalApplication.Page,
                Total = rentalApplication.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<RentalApplicationRegisterRequest>> GetByIdAsync(int? id)
        {
            Expression<Func<RentalApplicationRegisterRequest, bool>> queryFilter = c => true;

            if (id.HasValue)
                queryFilter = queryFilter.And(p => p.RentalApplicationId == id);

            var rentalApplication = await _rentalApplicationDataAccess.FirstOrDefaultAsync(queryFilter);

            return ResponseBuilder.Correct(rentalApplication);
        }

        //public async Task<ResponseDTO> ValidateEntityRegister(RentalApplicationRegisterRequest request)
        //{
        //    bool isValid = true;
        //    Expression<Func<RentalApplicationRegisterRequest, bool>> queryFilter = p => true;
        //    var errorMessage = "";
        //    queryFilter = queryFilter.And(p => p.RentalApplicationCode == request.RentalApplicationCode);
        //    var rentalApplication = await _rentalApplicationDataAccess.FirstOrDefaultAsync(queryFilter);

        //    if (rentalApplication != null)
        //    {
        //        isValid = false;
        //        errorMessage = "RentalApplication already Exists";
        //    }

        //    //Existe un Lease para el mismo tenant Activo o Futuro
        //    if (isValid)
        //    {
        //        queryFilter = p => p.TenantId == request.TenantId;
        //        queryFilter = queryFilter.And(p => p.RentalApplicationStatusCode == Constants.EntityStatus.RentalApplication.Active || p.RentalApplicationStatusCode == Constants.EntityStatus.RentalApplication.Future);
        //        rentalApplication = await _rentalApplicationDataAccess.FirstOrDefaultAsync(queryFilter);

        //        if (rentalApplication != null)
        //        {
        //            isValid = false;
        //            errorMessage = "Already Exists a tenant associated to other Lease Active or Future";
        //        }
        //    }

        //    //Validate Period
        //    if (isValid)
        //    {
        //        Expression<Func<PeriodDTO, bool>> queryFilterPeriod = p => true;
        //        queryFilterPeriod = queryFilterPeriod.And(p => p.EndDate < request.EndDate);
        //        var period = await _periodApplicationService.GetLastPeriodAsync();

        //        if (period != null && period.Data.EndDate < request.EndDate)
        //        {
        //            isValid = false;
        //            errorMessage = "There is no period configurated to support the end date of this Lease, please create period";
        //        }
        //    }


        //    var response = new ResponseDTO()
        //    {
        //        IsValid = string.IsNullOrEmpty(errorMessage),
        //        Messages = new List<ApplicationMessage>()
        //    };

        //    response.Messages.Add(new ApplicationMessage()
        //    {
        //        Key = string.IsNullOrEmpty(errorMessage) ? "Ok" : "Error",
        //        Message = errorMessage
        //    });

        //    return response;
        //}

        //public async Task<ResponseDTO> ValidateEntityUpdate(RentalApplicationUpdateRequest request)
        //{
        //    var errorMessage = "";
        //    Expression<Func<RentalApplicationRegisterRequest, bool>> queryFilter = p => p.RowStatus;
        //    queryFilter = queryFilter.And(p => p.RentalApplicationId != request.RentalApplicationId);
        //    queryFilter = queryFilter.And(p => p.TenantId == request.TenantId);
        //    queryFilter = queryFilter.And(p => p.RentalApplicationStatusCode == Constants.EntityStatus.RentalApplication.Active || p.RentalApplicationStatusCode == Constants.EntityStatus.RentalApplication.Future);

        //    bool isValid = true;
        //    var rentalApplication = await _rentalApplicationDataAccess.FirstOrDefaultAsync(queryFilter);

        //    if (rentalApplication != null)
        //    {
        //        isValid = false;
        //        errorMessage = "Already Exists a tenant associated to other Lease Active or Future";
        //    }

        //    var response = new ResponseDTO()
        //    {
        //        IsValid = string.IsNullOrEmpty(errorMessage),
        //        Messages = new List<ApplicationMessage>()
        //    };

        //    response.Messages.Add(new ApplicationMessage()
        //    {
        //        Key = string.IsNullOrEmpty(errorMessage) ? "Ok" : "Error",
        //        Message = errorMessage
        //    });

        //    return response;
        //}

        /*EXCEL REPORT*/

        public async Task GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent,
            TransportContext transportContext, RentalApplicationSearchRequest search)
        {
            var list = await SearchRentalApplicationAsync(search);
            try
            {
                if (list.Data.Items.Count > 0)
                    using (var writer = new StreamWriter(outputStream))
                    {
                        var headers = new List<string>
                        {
                            "RentalApplication Code",
                            "Period",
                            "Tenant",
                            "Property",
                            "Start",
                            "Finish",
                            "Deposit",
                            "Rent",
                            "Unpaid periods",
                            "Due Date",
                            "Days to collect"
                        };
                        await writer.WriteLineAsync(ExcelHelper.GetHeaderDetail(headers));
                        foreach (var item in list.Data.Items)
                            await writer.WriteLineAsync(ProcessCellDataToReport(item));
                    }
            }
            catch (HttpException ex)
            {
            }
            finally
            {
                outputStream.Close();
            }
        }

        private string ProcessCellDataToReport(RentalApplicationSearchDTO item)
        {
            //var startDate = string.Format("{0:MM/dd/yyyy HH:mm}", item.BeginDate) ?? "";
            //var finishDate = string.Format("{0:MM/dd/yyyy HH:mm}", item.EndDate) ?? "";
            //var product = !string.IsNullOrEmpty(item.ProductName) ? item.ProductName.Replace(@",", ".") : "";
            //var total = string.Format("{0:0.00}", item.DriverPay);
            //var rentPrice = string.Format("{0:0.00}", item.RentPrice);
            //var rentDeposit = string.Format("{0:0.00}", item.RentDeposit);
            //var unpaidPeriods = string.Format("{0:0}", item.UnpaidPeriods);
            //var nextDueDate = string.Format("{0:MM/dd/yyyy HH:mm}", item.NextDueDate) ?? "";
            //var nextDaystoCollect = string.Format("{0:0}", item.NextDaysToCollect);

            var textProperties = ""; //ExcelHelper.StringToCSVCell(item.RentalApplicationCode) + "," +
                                 //ExcelHelper.StringToCSVCell(item.PeriodCode) + "," +
                                 ////ExcelHelper.StringToCSVCell(item.TenantFullName) + "," +
                                 ////ExcelHelper.StringToCSVCell(item.HouseName) + "," +
                                 //ExcelHelper.StringToCSVCell(startDate) + "," +
                                 //ExcelHelper.StringToCSVCell(finishDate) + "," +
                                 //ExcelHelper.StringToCSVCell(rentDeposit) + "," +
                                 //ExcelHelper.StringToCSVCell(rentPrice) + "," +
                                 //ExcelHelper.StringToCSVCell(unpaidPeriods) + "," +
                                 //ExcelHelper.StringToCSVCell(nextDueDate) + "," +
                                 //ExcelHelper.StringToCSVCell(nextDaystoCollect);

            return textProperties;
        }

        private async Task<int?> GetPeriodByCode(string periodCode)
        {
            var period = await _periodApplicationService.GetPeriodByCodeAsync(periodCode);
            if (period.Data != null)
                return period.Data.PeriodId;

            return null;
        }

    }
}
