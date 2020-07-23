using System;

namespace DataAccessLayer.DTO
{
    internal class MessageDb
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
