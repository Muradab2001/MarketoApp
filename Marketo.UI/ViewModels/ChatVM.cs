using Marketo.Core.Entities;

namespace Marketo.UI.ViewModels
{
    public class ChatVM
    {
        public List<AppUser> AppUsers { get; set; }
        public List<Message> Messages { get; set; }
    }
}
