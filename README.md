# VNC Macro Runner

This is a simple VNC macro runner that can execute a series of commands on a remote VNC server.

## Usage

```
VNCMacroRunner.exe <host> <username> <password> <commandfile>
```

## Command File Format

Each line in the command file should be in the following format:

```
command:parameter1,parameter2
```

Supported commands:
- mouseMove:x,y
- mouseClick:left|right
- type:text

## Example

```
mouseMove:300,50
mouseClick:left
type:home
```

This will move the mouse to coordinates (300,50), perform a left click, and then type 'home'.

