using System.ComponentModel.DataAnnotations;

namespace WebApiShop
{
    public class UserClass
    {
        [EmailAddress]
        public string UserName { get; set; }

        [StringLength(8)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserID { get; set; }
    }
}
