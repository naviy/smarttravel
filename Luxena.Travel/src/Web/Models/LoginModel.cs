using System.ComponentModel.DataAnnotations;


namespace Luxena.Travel.Web.Models
{
	public class LoginModel
	{
		[Required]
		public string UserName { get; set; }

		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}
}