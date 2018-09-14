using Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.Interfaces.PaymentPeriod
{
    public interface IPaymentPeriodApplicationService
    {
        //Task<ResponseDTO> RegisterPaymentPeriodAsync(PaymentPeriodRegisterRequest paymentPeriod);
        Task<ResponseDTO> UpdatePaymentPeriodAsync(PPHeaderSearchByContractPeriodDTO paymentPeriod);
        //Task<ResponseDTO> DeletePaymentPeriodAsync(PaymentPeriodDeleteRequest paymentPeriod);
        Task<ResponseDTO<PagedList<PPSearchDTO>>> SearchPaymentPeriodAsync(PaymentPeriodSearchRequest search);
        Task<ResponseDTO<PPHeaderSearchByContractPeriodDTO>> SearchPaymentPeriodByContractAsync(PaymentPeriodSearchByContractPeriodRequest search);
        Task<ResponseDTO<PPDetailSearchByContractPeriodDTO>> CalculateLateFeeByContractAndPeriodAsync(PaymentPeriodSearchByContractPeriodRequest search);
        Task<ResponseDTO<PPDetailSearchByContractPeriodDTO>> CalculateOnAccountByContractAndPeriodAsync(PaymentPeriodSearchByContractPeriodRequest search);
        Task<ResponseDTO<List<PPHeaderSearchByInvoiceDTO>>> SearchInvoiceByIdAsync(string invoiceNo);
    }
}
