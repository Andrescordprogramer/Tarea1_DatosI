using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2 || args[0] != "-port")
        {
            Console.WriteLine("Uso: dotnet run <nombre-del-proyecto> -port <puerto-escucha>");
            return;
        }

        int port = int.Parse(args[1]);
        Thread serverThread = new Thread(() => StartServer(port));
        serverThread.Start();

        StartClient(port);
    }

    // Servidor que escucha en el puerto proporcionado
    static void StartServer(int port)
    {
        TcpListener server = new TcpListener(IPAddress.Loopback, port);
        server.Start();
        Console.WriteLine($"Servidor escuchando en el puerto {port}...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Mensaje recibido: {message}");
            client.Close();
        }
    }

    // Cliente que envía mensajes
    static void StartClient(int port)
    {
        while (true)
        {
            Console.Write("Ingrese el puerto destino: ");
            int destinationPort = int.Parse(Console.ReadLine());
            Console.Write("Ingrese el mensaje: ");
            string message = Console.ReadLine();

            TcpClient client = new TcpClient("127.0.0.1", destinationPort);
            NetworkStream stream = client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
            client.Close();
            Console.WriteLine($"Mensaje enviado a {destinationPort}: {message}");
        }
    }
}
