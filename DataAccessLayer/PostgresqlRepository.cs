using System;
using System.Collections.Generic;
using Contracts.Models;
using Npgsql;

namespace DataAccessLayer
{
    public class PostgresqlRepository : IMessagesRepository, IDisposable
    {
        private readonly string _connectionString;
        private readonly NpgsqlConnection _connection;
        public PostgresqlRepository(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();
        }

        public Guid SaveMessage(string messageText)
        {
            var sqlInsert = "INSERT INTO Messages (Text, Date) VALUES (@text, @date) RETURNING Id;";

           using (var insertCommand = new NpgsqlCommand(sqlInsert, _connection))
           {
               insertCommand.Parameters.AddWithValue("text", messageText);
               insertCommand.Parameters.AddWithValue("date", DateTime.UtcNow);
               var id = (Guid) insertCommand.ExecuteScalar();
               return id;
           }
        }

        public Message GetMessage(Guid id)
        {
            var selectMessageSql = "SELECT Number, Text, Date FROM public.messages WHERE Id=@id";

            using (var selectMessageCommand = new NpgsqlCommand(selectMessageSql, _connection))
            {
                selectMessageCommand.Parameters.AddWithValue("id", id);

                using (NpgsqlDataReader reader = selectMessageCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return GetMessageFromReader(reader);
                    }
                }
            }

            return null;
        }

        public IEnumerable<Message> GetMessages(DateTime fromDate, DateTime toDate)
        {
            var selectMessageSql =
                "SELECT Number, Text, Date FROM public.messages" +
                " WHERE Date <= @to " +
                "and Date >= @from ORDER BY Number ASC;";

            List<Message> messages = new List<Message>();

            using (var selectMessagesCommand = new NpgsqlCommand(selectMessageSql, _connection))
            {
                selectMessagesCommand.Parameters.AddWithValue("to", toDate);
                selectMessagesCommand.Parameters.AddWithValue("from", fromDate);
                using (NpgsqlDataReader reader = selectMessagesCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var message = GetMessageFromReader(reader);
                        messages.Add(message);
                    }
                }
            }

            return messages;
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }

        private static Message GetMessageFromReader(NpgsqlDataReader reader)
        {
            return new Message { Number = reader.GetInt32(0), Text = reader.GetString(1), Date = reader.GetDateTime(2) };
        }
    }
}
