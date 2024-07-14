# VNC Macro Runner

VNC Macro Runner is a simple C# application that allows you to automate interactions with a VNC server by executing a series of commands from a text file. This tool can be used for basic remote desktop automation tasks.

## Features

- Connect to a VNC server
- Execute mouse movements and clicks
- Simulate keyboard input
- Implement wait times between actions
- Read and execute commands from a text file

## Prerequisites

- .NET 6.0 SDK or later
- A VNC server to connect to

## Setup

1. Clone this repository:
   ```
   git clone https://github.com/toniilic/VNC-macro-runner.git
   cd VNC-macro-runner
   ```

2. Ensure you have the .NET 6.0 SDK or later installed on your system. You can download it from [here](https://dotnet.microsoft.com/download).

3. Build the project:
   ```
   dotnet build
   ```

## Usage

1. Create a text file (e.g., `commands.txt`) with your desired actions. Example:
   ```
   mousemove:300,50
   mouseclick:left
   wait:1000
   type:Hello, World!
   ```

2. Run the application:
   ```
   dotnet run <host> <port> <password> commands.txt
   ```
   Replace `<host>`, `<port>`, `<password>`, and `commands.txt` with appropriate values for your VNC server and command file.

## Supported Commands

- `mousemove:x,y` - Move the mouse cursor to the specified coordinates
- `mouseclick:left` - Perform a left mouse click
- `type:text` - Type the specified text
- `wait:milliseconds` - Wait for the specified number of milliseconds

## Implementation Details

This application uses basic TCP sockets to communicate with the VNC server using a simplified version of the RFB (Remote Framebuffer) protocol. It includes:

- Basic VNC handshake and authentication
- Sending mouse and keyboard events
- Executing a series of commands from a text file

## Limitations and Considerations

- This is a basic implementation and may not work with all VNC servers or handle all error cases.
- The authentication method is simplified and not secure for production use.
- The application doesn't handle the full VNC protocol, including screen updates or more complex interactions.
- Error handling is basic and may not cover all possible scenarios.

## Security Warning

This application sends the VNC password in plain text and does not implement proper VNC security measures. It is intended for educational and testing purposes only. Do not use it on production systems or over untrusted networks.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is open source and available under the [MIT License](LICENSE).
