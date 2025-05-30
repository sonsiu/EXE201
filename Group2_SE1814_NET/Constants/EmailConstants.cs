using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Group2_SE1814_NET.Constants
{
    public class EmailConstants
    {
        public const string GMAIL_REGISTER_EMAIL_SUBJECT = "Welcome to SHOPQUANAO - Google Sign-In Success!";

        public const string GMAIL_REGISTER_EMAIL_BODY = @"
        Hello {0},

        We noticed that you signed in using Google for the first time. Your account has been successfully created in SHOPQUANAO.

        ✅ Registered Email: {1}  
        ✅ Login Method: Google Sign-In  

        You can now access your account anytime by signing in with Google. If you would like to set a password for direct login, you can do so using the link below:

        🔐 [Set Your Password]({2})

        If this wasn’t you, please contact our support team immediately.

        Best regards,  
        SHOPQUANAO Team  

        support@SHOPQUANAO.com | SHOPQUANAO";
    }
}

