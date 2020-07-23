using System;
using System.Collections.Generic;
using Contracts.Models;

namespace DataAccessLayer
{
    public interface IMessagesRepository
    {
        Guid SaveMessage(string messageText);
        Message GetMessage(Guid id);
        IEnumerable<Message> GetMessages(DateTime fromDate, DateTime toDate);
    }
}
