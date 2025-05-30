using System.ComponentModel.DataAnnotations;

namespace Group2_SE1814_NET.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Please enter your full name")]
		public string Fullname { get; set; }
		[Required(ErrorMessage = "Please enter your email")]
		[EmailAddress(ErrorMessage = "Please enter a valid email address")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Please enter your password")]
		[DataType(DataType.Password)]
		[MinLength(3, ErrorMessage = "Password must be at least 3 characters long")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}
