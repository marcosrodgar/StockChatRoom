using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockBotChatRoom.Data;
using StockBotChatRoom.Data.Entities;
using StockBotChatRoom.Data.Repositories;

namespace StockBotChatRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatMessagesController(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        // GET: api/ChatMessages
        [HttpGet]
        public ActionResult<IEnumerable<ChatMessage>> GetChatMessages()
        {
            return Ok(_chatMessageRepository.GetMostRecentMessages());
        }


        [HttpPost]
        public ActionResult<ChatMessage> PostChatMessage(ChatMessage chatMessage)
        {
            if (chatMessage.Content == null || chatMessage.Content == string.Empty)
            {
                return BadRequest("Message content cannot be empty");
            }

            _chatMessageRepository.AddMessage(chatMessage);
            _chatMessageRepository.SaveChanges();

            return Ok(chatMessage);
        }

          

       
    }
}
