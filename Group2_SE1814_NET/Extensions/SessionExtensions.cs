using System.Text.Json;
using Microsoft.AspNetCore.Http;
namespace Group2_SE1814_NET.Extensions
{
    public static class SessionExtensions
    {
        // Lưu đối tượng vào session
        public static void SetObjectAsSession(this ISession session, string key, object value)
        {
            // Chuyển đổi đối tượng thành chuỗi JSON
            var jsonData = JsonSerializer.Serialize(value);

            // Lưu chuỗi JSON vào session
            session.SetString(key, jsonData);
        }

        // Lấy đối tượng từ session
        public static T GetObjectFromSession<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            // Nếu không có giá trị trong session, trả về default
            if (value == null)
            {
                return default(T);
            }

            // Chuyển đổi chuỗi JSON thành đối tượng
            return JsonSerializer.Deserialize<T>(value);
        }
    }
}
