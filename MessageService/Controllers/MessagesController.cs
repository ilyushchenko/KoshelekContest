using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Models;
using DataAccessLayer;
using MessageService.Models;
using MessageService.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesRepository _messagesRepository;
        private readonly WebSocketService _webSocketService;

        public MessagesController(IMessagesRepository messagesRepository, WebSocketService webSocketService)
        {
            _messagesRepository = messagesRepository;
            _webSocketService = webSocketService;
        }

        [HttpGet("from-{from}/to-{to}")]
        public IEnumerable<Message> GetInTime(DateTime from, DateTime to)
        {
            return _messagesRepository.GetMessages(from, to);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]MessageContent messageModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var messageId = _messagesRepository.SaveMessage(messageModel.Text);
            var storedMessage = _messagesRepository.GetMessage(messageId);

            var json = JsonConvert.SerializeObject(storedMessage);

            await _webSocketService.SendMessageToAllAsync(json);
            
            return Ok();
        }
    }
}
