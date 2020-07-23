using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Models;
using DataAccessLayer.DTO;

namespace DataAccessLayer
{
    public class FakeMessagesRepository : IMessagesRepository
    {
        private int _lastNumber = 0;
        private readonly List<MessageDb> _messages = new List<MessageDb>();
        public Guid SaveMessage(string messageText)
        {
            var message = new MessageDb
            {
                Date = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Number = ++_lastNumber,
                Text = messageText
            };
            _messages.Add(message);
            return message.Id;
        }

        public Message GetMessage(Guid id)
        {
            var storedMessage = _messages.FirstOrDefault(message => message.Id == id);
            return storedMessage != null ? ConvertToMessage(storedMessage) : null;
        }

        public IEnumerable<Message> GetMessages(DateTime fromDate, DateTime toDate)
        {
            return _messages.Where(storedMessage => storedMessage.Date >= fromDate && storedMessage.Date <= toDate).Select(ConvertToMessage);
        }

        private static Message ConvertToMessage(MessageDb storedMessage)
        {
            return new Message { Date = storedMessage.Date, Number = storedMessage.Number, Text = storedMessage.Text };
        }
    }
}
