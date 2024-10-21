using System.ComponentModel.DataAnnotations;

namespace PragmaOnce.Core.src.Common
{
    public class UserCredential
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public UserCredential(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}