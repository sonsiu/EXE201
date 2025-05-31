using Group2_SE1814_NET.ViewModels;

namespace Group2_SE1814_NET.Proxy {
    public interface IVnPayService {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
