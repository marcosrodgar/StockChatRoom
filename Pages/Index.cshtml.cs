using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockBotChatRoom.Data.Entities;
using StockBotChatRoom.Data.Repositories;
using StockBotChatRoom.Hubs;

namespace StockBotChatRoom.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IChatMessageRepository _chatMessageRepository;

        public IndexModel(ILogger<IndexModel> logger, IChatMessageRepository chatMessageRepository)
        {
            _logger = logger;
           _chatMessageRepository = chatMessageRepository;
        }

        
     

       
    }
}