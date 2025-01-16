using System.Dynamic;
using System.Net.Sockets;
using System.Text;
internal class Program
{
    private static void Main(string[] args)
    {
        // if (args.Length == 0)
        // {
        //     Console.WriteLine("usage dotnet run -- server_addr");
        // }
        using TcpClient client = new TcpClient("grgta.xyz", 5000);

        using (NetworkStream stream = client.GetStream())
        {
            _ = Task.Run(() => ListenForMessages(stream));

            byte[] buffer = new byte[1024];

            Console.WriteLine("Enter name: ");
            string name = Console.ReadLine()!;
            while (true)
            {
                var message = Console.ReadLine()!;
                stream.Write(Encoding.UTF8.GetBytes(name + ": " + message));
                Console.CursorTop = Console.CursorTop - 1;
                Console.WriteLine("You: " + message);
            }
        }
    }
    private static async Task ListenForMessages(NetworkStream stream)
    {
        try
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"{message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Listening error: {ex.Message}");
        }
    }

}
