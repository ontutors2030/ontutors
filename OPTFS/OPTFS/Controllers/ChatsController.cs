using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using OPTFS.Models;
using OPTFS.RealtimeChat;

namespace OPTFS.Controllers
{
    public class ChatsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ChatsController(ILogger<HomeController> logger,
           RoleManager<IdentityRole> roleManager,
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           ApplicationDbContext context)
        {
            this._logger = logger;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.db = context;            
        }

        [Authorize]
        [HttpGet]
        // GET: Chats
        public async Task<IActionResult> OpenChat(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);            
            if (id != null)
            {
                var myChat = db.Chat.Where
                            (
                                c =>
                                (c.User1Id == userId && c.User2Id == id) ||
                                (c.User2Id == userId && c.User1Id == id)
                            )?.OrderBy(c=>c.UpdatedAt)?.LastOrDefault();
                if (myChat != null)
                {
                    return RedirectToAction("Index", new { id = myChat.Id });
                }
                else
                {
                    Chat chat = new Chat
                    {
                        User1Id=userId,
                        User2Id=id,
                    };
                    db.Chat.Add(chat);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", new { id = chat.Id });
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        // GET: Chats
        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.SelectedPage = "inboxNavItem";
            if (id != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var chat= db.Chat
                    .Include(c=>c.Messages)
                    .Include(c => c.User1).Include(c => c.User2)
                    .Where(c=>c.Id==id)?.FirstOrDefault();
                if (chat != null && chat.Messages!=null)
                {
                    var newMessages= chat.Messages.Where(m=>m.StatusId==(int)MessageStatus.New).ToList();
                    if(newMessages!=null && newMessages.Count>0)
                        foreach (var msg in newMessages)
                        {
                            if (userId == msg.RecieverId)
                            {
                                msg.StatusId = (int)MessageStatus.Delivered;
                                db.ChatMessages.Update(msg);
                                await db.SaveChangesAsync();
                            }
                        }
                }

                ViewBag.Chat = chat;
            }            

            return View();
        }

        [Authorize]
        [HttpGet]
        public int GetNewMessagesCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var messages = db?.ChatMessages.Where(n => n.RecieverId == userId)?
                .Where(n => n.StatusId == (int)MessageStatus.New).ToList();
            return messages.Count;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LoadMessage(int id)
        {
            if (id != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var msg = db.ChatMessages
                    .Include(m=>m.Sender)
                    .Where(m => m.Id == id)?.FirstOrDefault();
                if (msg!=null && userId == msg.RecieverId)
                {
                    msg.StatusId = (int)MessageStatus.Delivered;
                    db.ChatMessages.Update(msg);
                    await db.SaveChangesAsync();
                }
                return PartialView(msg);
            }

            return PartialView();
        }

        [Authorize]
        [HttpPost(Name = "SendMessage")]
        public async Task<int> SendMessage([Bind("Text,RecieverId,ChatId")] ChatMessage message)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool recieverReady = false;
            bool chatReady = false;
            if (!string.IsNullOrWhiteSpace(message.RecieverId))
            {
                var reciever = db.Users.Find(message.RecieverId);
                if (reciever != null)
                {
                    recieverReady = true;
                    if (message.ChatId!=null && message.ChatId!=0)
                    {
                        var myChat = db.Chat.Where
                            (
                                c => c.Id == message.ChatId &&
                                ((c.User1Id == userId && c.User2Id == reciever.Id) ||
                                (c.User2Id == userId && c.User1Id == reciever.Id))
                            )?.FirstOrDefault();
                        if (myChat != null)
                            chatReady = true;
                    }
                    else
                    {
                        var myChat = db.Chat.Where
                            (
                                c =>
                                (c.User1Id == userId && c.User2Id == reciever.Id) ||
                                (c.User2Id == userId && c.User1Id == reciever.Id)
                            )?.OrderBy(c=>c.User1Id)?.LastOrDefault();
                        if (myChat != null)
                        {
                            message.ChatId = myChat.Id;
                            chatReady = true;
                        }
                    }
                }
            }

            if (!chatReady)
            {
                var chat = db.Chat.Find(message.ChatId);
                if (chat != null)
                {
                    message.ChatId = chat.Id;
                    chatReady = true;                    
                }
            }
            else
            {
                var chat = db.Chat.Find(message.ChatId);
                if (!recieverReady)
                {
                    if (chat?.User1Id == userId)
                    {
                        message.RecieverId = chat?.User2Id;
                        recieverReady = true;
                    }
                    else if (chat?.User2Id == userId)
                    {
                        message.RecieverId = chat.User1Id;
                        recieverReady = true;
                    }
                }
            }

            if (recieverReady && chatReady)
            {
                message.SenderId = userId;
                if (ModelState.IsValid)
                {
                    db.ChatMessages.Add(message);
                    await db.SaveChangesAsync();
                    SignalRClient.GetInstance.SendToUser(userId, message.RecieverId, message.Id);
                    return message.Id;
                }
            }            
            return 0;
        }
    }
}