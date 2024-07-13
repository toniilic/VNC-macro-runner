using System;
using System.IO;
using VncSharp;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Usage: VNCMacroRunner.exe <host> <username> <password> <commandfile>");
            return;
        }

        string host = args[0];
        string username = args[1];
        string password = args[2];
        string commandFile = args[3];

        using (var vnc = new RemoteDesktop())
        {
            try
            {
                vnc.Connect(host, 5900); // Assuming default VNC port
                vnc.Login(password);

                foreach (string line in File.ReadLines(commandFile))
                {
                    string[] parts = line.Split(':');
                    if (parts.Length != 2) continue;

                    string command = parts[0];
                    string[] parameters = parts[1].Split(',');

                    switch (command.ToLower())
                    {
                        case "mousemove":
                            if (parameters.Length == 2 && int.TryParse(parameters[0], out int x) && int.TryParse(parameters[1], out int y))
                            {
                                vnc.Mouse.Move(x, y);
                                Console.WriteLine($"Moved mouse to {x},{y}");
                            }
                            break;
                        case "mouseclick":
                            if (parameters[0].ToLower() == "left")
                            {
                                vnc.Mouse.LeftButtonClick();
                                Console.WriteLine("Left mouse button clicked");
                            }
                            else if (parameters[0].ToLower() == "right")
                            {
                                vnc.Mouse.RightButtonClick();
                                Console.WriteLine("Right mouse button clicked");
                            }
                            break;
                        case "type":
                            vnc.Keyboard.SendKeyEvent(parameters[0]);
                            Console.WriteLine($"Typed: {parameters[0]}");
                            break;
                        default:
                            Console.WriteLine($"Unknown command: {command}");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        Console.WriteLine("Macro execution completed.");
    }
}
