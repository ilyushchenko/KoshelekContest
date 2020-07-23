using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageService.Services
{
    public class WebSocketService
    {
        private readonly WebSocketConnectionManager _webSocketWebSocketConnectionManager;

        public WebSocketService(WebSocketConnectionManager webSocketWebSocketConnectionManager)
        {
            _webSocketWebSocketConnectionManager = webSocketWebSocketConnectionManager;
        }

        public async Task OnConnected(WebSocket socket)
        {
            _webSocketWebSocketConnectionManager.AddSocket(socket);
        }

        public async Task OnDisconnected(WebSocket socket)
        {
            await _webSocketWebSocketConnectionManager.RemoveSocket(_webSocketWebSocketConnectionManager.GetId(socket));
        }

        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(messageBytes, 0, messageBytes.Length);

            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in _webSocketWebSocketConnectionManager.GetAll())
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendMessageAsync(pair.Value, message);
            }
        }
    }
}
