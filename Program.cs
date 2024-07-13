using System;
using System.IO;
using VncSharp;
using YamlDotNet.Serialization;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Usage: VNCMacroRunner.exe <host> <username> <password> <configfile>");
            return;
        }

        string host = args[0];
        string username = args[1];
        string password = args[2];
        string configFile = args[3];

        var steps = ReadConfigFile(configFile);

        using (var vnc = new RemoteDesktop())
        {
            try
            {
                vnc.Connect(host, 5900); // Assuming default VNC port
                vnc.Login(password);

                ExecuteSteps(vnc, steps);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        Console.WriteLine("Macro execution completed.");
    }

    static List<Step> ReadConfigFile(string filePath)
    {
        var deserializer = new DeserializerBuilder().Build();
        var yaml = File.ReadAllText(filePath);
        return deserializer.Deserialize<List<Step>>(yaml);
    }

    static void ExecuteSteps(RemoteDesktop vnc, List<Step> steps)
    {
        foreach (var step in steps)
        {
            Console.WriteLine($"Executing step: {step.Action}");
            switch (step.Action.ToLower())
            {
                case "mousemove":
                    vnc.Mouse.Move(step.X, step.Y);
                    Console.WriteLine($"Moved mouse to {step.X},{step.Y}");
                    break;
                case "mouseclick":
                    if (step.Button == "left")
                    {
                        vnc.Mouse.LeftButtonClick();
                        Console.WriteLine("Left mouse button clicked");
                    }
                    else if (step.Button == "right")
                    {
                        vnc.Mouse.RightButtonClick();
                        Console.WriteLine("Right mouse button clicked");
                    }
                    break;
                case "type":
                    vnc.Keyboard.SendKeyEvent(step.Text);
                    Console.WriteLine($"Typed: {step.Text}");
                    break;
                case "wait":
                    System.Threading.Thread.Sleep(step.Milliseconds);
                    Console.WriteLine($"Waited for {step.Milliseconds} ms");
                    break;
                default:
                    Console.WriteLine($"Unknown action: {step.Action}");
                    break;
            }
        }
    }
}

class Step
{
    public string Action { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public string Button { get; set; }
    public string Text { get; set; }
    public int Milliseconds { get; set; }
}
