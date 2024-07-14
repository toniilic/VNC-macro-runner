using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Usage: dotnet run <host> <port> <password> <commandfile>");
            return;
        }

        string host = args[0];
        int port = int.Parse(args[1]);
        string password = args[2];
        string commandFile = args[3];

        var steps = ReadCommandFile(commandFile);

        using (var client = new TcpClient())
        {
            try
            {
                await client.ConnectAsync(host, port);
                Console.WriteLine("Connected to VNC server successfully.");

                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream))
                using (var writer = new StreamWriter(stream) { AutoFlush = true })
                {
                    // Perform VNC handshake and authentication
                    await PerformHandshakeAndAuthentication(reader, writer, password);

                    await ExecuteStepsAsync(stream, steps);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        Console.WriteLine("Macro execution completed.");
    }

    static List<Step> ReadCommandFile(string filePath)
    {
        var steps = new List<Step>();
        foreach (string line in File.ReadLines(filePath))
        {
            string[] parts = line.Split(':');
            if (parts.Length != 2) continue;

            string command = parts[0];
            string[] parameters = parts[1].Split(',');

            steps.Add(new Step
            {
                Action = command,
                Parameters = parameters
            });
        }
        return steps;
    }

    static async Task PerformHandshakeAndAuthentication(StreamReader reader, StreamWriter writer, string password)
    {
        // Read server version
        string? serverVersion = await reader.ReadLineAsync();
        Console.WriteLine($"Server Version: {serverVersion}");

        // Send client version
        await writer.WriteLineAsync("RFB 003.008\n");

        // TODO: Implement proper authentication based on the server's supported security types
        // For now, we'll assume the server accepts password authentication

        // Send password (this is a simplified version and not secure)
        await writer.WriteAsync(password);
        await writer.FlushAsync();

        // Read authentication result
        byte[] authResultBuffer = new byte[4];
        int bytesRead = await reader.BaseStream.ReadAsync(authResultBuffer, 0, 4);
        if (bytesRead != 4)
        {
            throw new Exception("Failed to read authentication result");
        }
        int authResult = BitConverter.ToInt32(authResultBuffer, 0);
        if (authResult != 0)
        {
            throw new Exception("Authentication failed");
        }

        Console.WriteLine("Authentication successful");
    }

    static async Task ExecuteStepsAsync(NetworkStream stream, List<Step> steps)
    {
        foreach (var step in steps)
        {
            Console.WriteLine($"Executing step: {step.Action}");
            try
            {
                switch (step.Action.ToLower())
                {
                    case "mousemove":
                        if (step.Parameters.Length == 2 &&
                            int.TryParse(step.Parameters[0], out int x) &&
                            int.TryParse(step.Parameters[1], out int y))
                        {
                            await SendPointerEvent(stream, x, y, false);
                            Console.WriteLine($"Moved mouse to {x},{y}");
                        }
                        break;
                    case "mouseclick":
                        if (step.Parameters[0].ToLower() == "left")
                        {
                            await SendPointerEvent(stream, 0, 0, true);
                            await Task.Delay(100);
                            await SendPointerEvent(stream, 0, 0, false);
                            Console.WriteLine("Clicked left mouse button");
                        }
                        break;
                    case "type":
                        string text = string.Join(",", step.Parameters);
                        await SendKeyEvents(stream, text);
                        Console.WriteLine($"Typed: {text}");
                        break;
                    case "wait":
                        if (int.TryParse(step.Parameters[0], out int milliseconds))
                        {
                            await Task.Delay(milliseconds);
                            Console.WriteLine($"Waited for {milliseconds} ms");
                        }
                        break;
                    default:
                        Console.WriteLine($"Unknown action: {step.Action}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing step: {ex.Message}");
            }
        }
    }

    static async Task SendPointerEvent(NetworkStream stream, int x, int y, bool buttonPressed)
    {
        byte[] message = new byte[6];
        message[0] = 5; // PointerEvent message type
        message[1] = buttonPressed ? (byte)1 : (byte)0; // Button mask (1 for left button pressed, 0 for released)
        BitConverter.GetBytes((ushort)x).CopyTo(message, 2);
        BitConverter.GetBytes((ushort)y).CopyTo(message, 4);
        await stream.WriteAsync(message);
    }

    static async Task SendKeyEvents(NetworkStream stream, string text)
    {
        foreach (char c in text)
        {
            byte[] message = new byte[8];
            message[0] = 4; // KeyEvent message type
            message[1] = 1; // Down flag
            BitConverter.GetBytes((uint)c).CopyTo(message, 4);
            await stream.WriteAsync(message);

            message[1] = 0; // Up flag
            await stream.WriteAsync(message);
        }
    }
}

class Step
{
    public string Action { get; set; } = "";
    public string[] Parameters { get; set; } = Array.Empty<string>();
}
