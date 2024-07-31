using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InstantMessage
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 2 || args[0] != "-port")
            {
                Console.WriteLine("Usage: dotnet run <nombre-del-proyecto> -port <puerto-escucha>");
                return;
            }

            if (!int.TryParse(args[1], out int port))
            {
                Console.WriteLine("Invalid port number");
                return;
            }

            ChatClient client = new ChatClient(port);
            await client.StartAsync();
        }
    }

    public class ChatClient
    {
        private readonly int _port;
        private TcpListener _listener;
        private CancellationTokenSource _cancellationTokenSource;
        private const int BufferSize = 1024;

        public ChatClient(int port)
        {
            _port = port;
            _listener = new TcpListener(IPAddress.Any, _port);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task StartAsync()
        {
            _listener.Start();
            Console.WriteLine($"Listening on port {_port}...");

            var listenTask = ListenForMessagesAsync(_cancellationTokenSource.Token);

            Console.WriteLine("Enter messages in the format <port> <message>");

            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                var splitInput = input.Split(' ', 2);
                if (splitInput.Length < 2)
                {
                    Console.WriteLine("Invalid input format. Use <port> <message>");
                    continue;
                }

                if (!int.TryParse(splitInput[0], out int targetPort))
                {
                    Console.WriteLine("Invalid port number");
                    continue;
                }

                var message = splitInput[1];
                await SendMessageAsync(targetPort, message);
            }
        }

        private async Task ListenForMessagesAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_listener.Pending())
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _ = HandleClientAsync(client, cancellationToken);
                }
                await Task.Delay(100, cancellationToken);
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            using (var networkStream = client.GetStream())
            {
                var buffer = new byte[BufferSize];
                int bytesRead;
                while ((bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) != 0)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {message}");
                }
            }
        }

        private async Task SendMessageAsync(int port, string message)
        {
            try
            {
                using (var client = new TcpClient("localhost", port))
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    var networkStream = client.GetStream();
                    await networkStream.WriteAsync(buffer, 0, buffer.Length);
                    Console.WriteLine($"Message sent to port {port}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send message: {ex.Message}");
            }
        }
    }
}
