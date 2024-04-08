using Marketo.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Marketo.UI.ViewModels
{
    public class AccountVM
    {
        public AppUser AppUser { get; set; }
        public string Token { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
