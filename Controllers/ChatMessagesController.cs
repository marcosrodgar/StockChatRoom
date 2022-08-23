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
using StockBotChatRoom.QueueMessages;
using StockBotChatRoom.Services.Interfaces;

namespace StockBotChatRoom.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly UserManager<ChatUser> _userManager;
        private readonly IQueueService _queueService;

        public ChatMessagesController(IChatMessageRepository chatMessageRepository, UserManager<ChatUser> userManager, IQueueService queueService )
        {
            _chatMessageRepository = chatMessageRepository;
            _userManager = userManager;
            _queueService = queueService;
        }

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

            if (chatMessage.Content.Substring(0, 1) == "/")
            {
                chatMessage.SentOn = DateTime.Now;
                _queueService.QueueMessage(new StockCommandMessage
                {
                    ChatMessage = new ChatMessageModel
                    {
                        Content = chatMessage.Content,
                        SentOn = chatMessage.SentOn,
                        UserName = currentUser.UserName
                    }
                });

                return Ok(chatMessage);
            }

            _chatMessageRepository.AddMessage(chatMessage);
            _chatMessageRepository.SaveChanges();

            return chatMessage;
        }

          

       
    }
}
