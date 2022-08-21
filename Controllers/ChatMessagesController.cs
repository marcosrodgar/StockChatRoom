using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockBotChatRoom.Data;
using StockBotChatRoom.Data.Entities;
using StockBotChatRoom.Data.Repositories;
using StockBotChatRoom.Models;

namespace StockBotChatRoom.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly UserManager<ChatUser> _userManager;

        public ChatMessagesController(IChatMessageRepository chatMessageRepository, UserManager<ChatUser> userManager )
        {
            _chatMessageRepository = chatMessageRepository;
            _userManager = userManager;
        }

        // GET: api/ChatMessages
        [HttpGet]
        public ActionResult<IEnumerable<ChatMessageModel>> GetChatMessages()
        {
            return Ok(_chatMessageRepository.GetMostRecentMessages());
        }


        [HttpPost]
        public async Task<ActionResult<ChatMessage>> PostChatMessage(ChatMessage chatMessage)
        {
            if (chatMessage.Content == null || chatMessage.Content == string.Empty)
            {
                return BadRequest("Message content cannot be empty");
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            chatMessage.User = currentUser;

            _chatMessageRepository.AddMessage(chatMessage);
            _chatMessageRepository.SaveChanges();

            return chatMessage;
        }

          

       
    }
}
