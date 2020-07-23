using System;
using System.Collections.Generic;
using Contracts.Models;
using DataAccessLayer;
using MessageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesRepository _messagesRepository;

        public MessagesController(IMessagesRepository messagesRepository)
        {
            _messagesRepository = messagesRepository;
        }

        [HttpGet("from-{from}/to-{to}")]
        public IEnumerable<Message> GetInTime(DateTime from, DateTime to)
        {
            return _messagesRepository.GetMessages(from, to);
        }

        [HttpPost]
        public ActionResult Post([FromBody]MessageContent messageModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var messageId = _messagesRepository.SaveMessage(messageModel.Text);
            var storedMessage = _messagesRepository.GetMessage(messageId);
            return Ok();
        }
    }
}
