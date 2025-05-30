using Group2_SE1814_NET.ViewModels;

namespace Group2_SE1814_NET.Proxy
{
    public interface IPayosService
    {
        string CreatePaymentUrl(PayOSInformationModel model, HttpContext context);
        PayOSResponseModel PaymentExecute(IQueryCollection queryCollection);
    }

}
