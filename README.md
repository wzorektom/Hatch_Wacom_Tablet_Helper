# Hatch Wacom Tablet Helper

## Overview

Hatch Wacom (or any other) Tablet Helper is a utility designed to enhance the experience of using tablets with Wilcom Hatch Embroidery software, specifically versions 3 and earlier. This application allows users to simulate mouse clicks and key presses using keyboard, streamlining the digitizing process in Hatch by making it more intuitive and efficient to place corner and curve points using the tablet pen and the allocated keyboard keys.

**Disclaimer**: This tool is not affiliated with, endorsed by, or in any way officially connected with Hatch or Wilcom. It is an independent application that does not rely on Hatch to function and can be utilized with any software that might benefit from simulated mouse clicks and keyboard inputs.

## Features

- **Mouse Click Simulation**: Simulate left and right mouse clicks with keyboard keys to easily place corner and curve points using a tablet pen and pressing the corresponding key on the keyboard.
- **Key Press Simulation**: Seamlessly navigate and edit in the Hatch software by emulating the pressing of Enter and Backspace keys - use Enter to apply stitches and Backspace to remove the stitch point.
- **Customizable Key Bindings**: Users can assign specific keys for each action, allowing for a personalized setup that fits their workflow.

## Getting Started

### Prerequisites

- A computer running Windows OS.
- .NET Framework installed (the version compatible with your system).
- Wilcom Hatch Embroidery software (version 3 or earlier).
- A Wacom standard or Intuos tablet or any other tablet with a pen.

### Installation

1. Clone or download the repository to your local machine.
2. Compile the application using a C# compiler or open it in a compatible IDE like Visual Studio and build the project.
3. Run the executable file generated after the build process.
4. If you don't know how to compile the program, you will find the ready build program in the Hatch_Tablet_Helper.rar archive.

### Configuration

Upon first launch, the application will prompt you to configure your key bindings:
- **Toggle Left Mouse Button**: Press the key you wish to use for simulating a left mouse click. Press Enter to skip.
- **Simulate Right Click**: Press the key you want to assign for simulating a right mouse click. Press Enter to skip.
- **Simulate Enter Key Press**: Choose a key for simulating the Enter key press. Press Enter to skip.
- **Simulate Backspace Key Press**: Select a key for simulating a Backspace key press. Press Enter to skip.

Each key selected will be confirmed on the console. If you wish to change a key binding later, you will need to restart the application and reconfigure your preferences.

### Usage

After configuration, the application runs in the background. Perform the designated key presses to simulate mouse clicks or key presses while using your Wacom tablet with Hatch Embroidery software or any other software of your choice. This setup allows for more natural drawing and editing actions, closely mimicking the use of pen and paper.

### Closing the Application

To close the application, simply close the console window with your mouse. The key hooks will be automatically unregistered, and the application will terminate.

## Support

For any issues or suggestions, feel free to reach out to me.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
