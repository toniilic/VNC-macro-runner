# VNC Macro Runner

VNC Macro Runner is a C# application that automates interactions with a remote desktop using the VNC protocol. It reads a series of commands from a YAML file and executes them on the remote machine.

## Features

- Connect to a VNC server
- Execute mouse movements and clicks
- Type text
- Wait for specified durations between actions
- Read commands from a YAML configuration file

## Prerequisites

- .NET 6.0 SDK or later
- A VNC server to connect to

## Installation

1. Clone this repository:
   ```
   git clone https://github.com/toniilic/VNC-macro-runner.git
   cd VNC-macro-runner
   ```

2. Install the required packages:
   ```
   dotnet add package VncSharp
   dotnet add package YamlDotNet
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
   ```

2. Run the application:
   ```
   dotnet run <host> <username> <password> <path_to_yaml_file>
   ```
   Replace `<host>`, `<username>`, `<password>`, and `<path_to_yaml_file>` with appropriate values.

## Supported Actions

- `mousemove`: Move the mouse cursor (requires `x` and `y` coordinates)
- `mouseclick`: Perform a mouse click (requires `button`: "left" or "right")
- `type`: Type text (requires `text` to type)
- `wait`: Wait for a specified duration (requires `milliseconds`)

## Running on Windows

1. Install .NET SDK from https://dotnet.microsoft.com/download

2. Open Command Prompt or PowerShell as administrator

3. Navigate to the project directory:
   ```
   cd path\to\VNC-macro-runner
   ```

4. Build the project:
   ```
   dotnet build
   ```

5. Run the project:
   ```
   dotnet run localhost username password steps.yaml
   ```
   (Replace with appropriate values)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is open source and available under the [MIT License](LICENSE).
