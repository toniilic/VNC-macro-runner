# VNC Macro Runner

VNC Macro Runner is a C# application that interacts with a remote desktop using the VNC protocol. It reads a series of commands from a YAML file and executes them on the remote machine, allowing for automated interactions with a VNC server.

## Features

- Connect to a VNC server
- Execute mouse movements and clicks on the remote desktop
- Type text on the remote desktop
- Implement wait times between actions
- Read and execute commands from a YAML configuration file

## Prerequisites

- .NET 6.0 SDK or later
- A VNC server to connect to
- VncSharp library (automatically installed via NuGet)

## Installation

### Ubuntu

1. Install the .NET SDK:
   ```
   wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   rm packages-microsoft-prod.deb
   sudo apt-get update
   sudo apt-get install -y dotnet-sdk-6.0
   ```

2. Clone this repository:
   ```
   git clone https://github.com/toniilic/VNC-macro-runner.git
   cd VNC-macro-runner
   ```

3. Install required packages:
   ```
   dotnet add package YamlDotNet
   dotnet add package VncSharp
   ```

### Windows 10

1. Install the .NET SDK:
   - Download the installer from https://dotnet.microsoft.com/download
   - Run the installer and follow the prompts

2. Clone this repository or download it as a ZIP and extract it

3. Open Command Prompt or PowerShell and navigate to the project directory:
   ```
   cd path\to\VNC-macro-runner
   ```

4. Install required packages:
   ```
   dotnet add package YamlDotNet
   dotnet add package VncSharp
   ```

## Usage

1. Create a YAML file (e.g., `steps.yaml`) with your desired actions. Example:
   ```yaml
   - action: mousemove
     x: 300
     y: 50
   - action: mouseclick
     button: left
   - action: wait
     milliseconds: 1000
   - action: type
     text: Hello, World!
   - action: wait
     milliseconds: 500
   - action: mousemove
     x: 500
     y: 100
   - action: mouseclick
     button: right
   ```

2. Run the application:
   ```
   dotnet run <host> <port> <password> steps.yaml
   ```
   Replace `<host>`, `<port>`, `<password>`, and `steps.yaml` with appropriate values for your VNC server and configuration file.

## Supported Actions

- `mousemove`: Move the mouse cursor (requires `x` and `y` coordinates)
- `mouseclick`: Perform a mouse click (requires `button`: "left" or "right")
- `type`: Type text (requires `text` to type)
- `wait`: Wait for a specified duration (requires `milliseconds`)

## Security Considerations

- This application requires a VNC password. Ensure you're using a secure, unique password for your VNC server.
- Be cautious when running scripts that interact with a remote desktop, especially in production environments.
- Consider the security implications of storing VNC passwords in plain text or passing them as command-line arguments.

## Testing

It's recommended to test this application in a controlled environment before using it with any critical systems. You can set up a local VNC server for testing purposes.

## Troubleshooting

- Ensure your VNC server is running and accessible from the machine running this application.
- Check that the port number is correct (default is usually 5900 for the first display).
- If you encounter connection issues, verify that your firewall is not blocking the connection.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is open source and available under the [MIT License](LICENSE).
