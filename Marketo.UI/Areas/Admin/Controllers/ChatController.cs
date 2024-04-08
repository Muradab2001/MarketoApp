using Marketo.Core.Entities;
using Marketo.DataAccess.Contexts;
using Marketo.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketo.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Moderator")]


    public class ChatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ChatController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(string id)
        {
            if (id == null) return NotFound();
            AppUser appUser = _context.Users.FirstOrDefault(x => x.UserName == id);
             if(appUser == null) return NotFound();
            List<AppUser> users = _context.Users.Where(u => u.Id != appUser.Id && u.Admin == true || u.Id != appUser.Id&& u.Admin == false).ToList();
            List<Message> messages = _context.Messages.ToList();
            ChatVM chatVM = new ChatVM
            {
                AppUsers = users,
                Messages = messages
            };
            return View(chatVM);
        }
        public IActionResult ChatUser(string Userid)
        {
            if (!User.Identity.IsAuthenticated) return NotFound();
            string Loginuserstring = User.Identity.Name;
            AppUser Loginuser= _context.Users.FirstOrDefault(u => u.UserName == Loginuserstring);
            if (Userid == null) return NotFound();
            AppUser appUser = _context.Users.FirstOrDefault(x => x.Id == Userid);
            if (appUser == null) return NotFound();
            ViewBag.User = appUser;
            List<AppUser> users = _context.Users.Where(u => u.UserName != User.Identity.Name&&u.Admin==true|| u.UserName != User.Identity.Name && u.Admin==false).ToList();
            List<Message> messages = _context.Messages.Include(m => m.AppUser)
           .Where(m => (m.AppUserId == Loginuser.Id && m.AcceptUserId == Userid) || (m.AppUserId == Userid && m.AcceptUserId == Loginuser.Id))
           .ToList();
            ChatVM chatVM = new ChatVM
            {
                AppUsers = users,
                Messages = messages
            };
            return View(chatVM);
        }
    }
}