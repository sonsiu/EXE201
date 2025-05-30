
namespace Group2_SE1814_NET.Proxy {
    public interface IEmailServices {
        Task SendEmail(string receptor, string subject, string body);
    }
}