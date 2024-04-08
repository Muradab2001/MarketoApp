using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Marketo.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Marketo.UI.Areas.Admin.Controllers
{
    public class ChatHub:Hub
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ChatHub(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public async Task OrderSalary(decimal salary,DateTime dateTime)
        {
            await Clients.All.SendAsync("SendSalary", salary, dateTime);

        }


        public override async Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;
            AppUser user = await _userManager.FindByNameAsync(userName);
            user.Active = true;
            _context.SaveChanges();
            await Clients.All.SendAsync("ConnectUser", user.Id);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userName = Context.User.Identity.Name;
            AppUser user = await _userManager.FindByNameAsync(userName);


            user.Active = false;
            await Clients.All.SendAsync("DisconnectUser", user.Id);
            _context.SaveChanges();
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(string message,string ID)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var userName = Context.User.Identity.Name;
                AppUser user = await _userManager.FindByNameAsync(userName);
                AppUser SendUser=await _userManager.FindByIdAsync(ID);
                Message existed = new Message
                {
                    Text = message,
                    AppUser = user,
                    AppUserId = user.Id,
                    CreateTime = DateTime.UtcNow,
                    AcceptUserId=ID
                    
                };
                _context.Messages.Add(existed);
                _context.SaveChanges();

                await Clients.All.SendAsync("ReceiveMessage", message, user.UserName, SendUser.UserName);
            }
        }
    }
}
