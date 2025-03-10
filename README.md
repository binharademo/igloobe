# IGloobe - Motion Capture Interaction System

## Project Overview

IGloobe is an interactive application developed in C# that uses Bluetooth motion capture technology to enable presentation control and on-screen drawing. The system was designed to offer an interactive experience similar to the Nintendo Wiimote, but adapted for use in presentations and educational applications.

**Development Period:** July 2010 to April 2011 (based on file modification dates)

## Main Features

1. **Bluetooth Device Connection**
   - Automatic discovery of compatible devices
   - State-based connection management
   - Automatic reconnection in case of signal loss

2. **Presentation Control**
   - Navigation between PowerPoint presentation slides
   - Support for .ppt and .pptx files
   - Automatic conversion to presentation format (.pps)
   - Windows taskbar hiding during presentations

3. **Interactive Drawing**
   - Virtual whiteboard
   - Drawing over the desktop
   - Color control (black, blue, red, yellow, white)
   - Stroke thickness control (3 levels)
   - Transparency control
   - Eraser tool

4. **Cursor Control**
   - Mapping device movement to cursor
   - Right-click support
   - Calibration for sensitivity adjustment

5. **Additional Utilities**
   - Quick access to Windows virtual keyboard
   - Minimalist and non-intrusive interface

## System Architecture

The system is divided into five main projects, each with specific responsibilities:

### 1. Connector (Base Library)
- Defines interfaces and abstract classes for device communication
- Implements the State pattern to manage connection states
- Provides logging and debugging mechanisms
- Contains calibration logic and coordinate transformation (warping)

### 2. IGloobeMote (Motion Capture)
- Implements communication with the physical device via HID (Human Interface Device)
- Processes accelerometer and infrared sensor data
- Manages button events and movements
- Implements device memory reading and writing

### 3. ConnectorWindows (Windows Implementation)
- Implements Windows-specific Bluetooth communication
- Uses the InTheHand.Net.Personal library for Bluetooth
- Manages the device connection lifecycle

### 4. iGloobeAppsMain (Applications)
- Contains graphical interfaces for drawing and presentation
- Implements business logic for applications
- Manages user interaction with the system
- Implements draggable and transparent forms for better user experience

### 5. ConnectorMain (Main Application)
- System entry point
- Initializes necessary components
- Manages application lifecycle
- Prevents multiple instances of the application

## Technologies Used

### Languages and Frameworks
- **C# (.NET Framework 2.0)** - Main development language
- **Windows Forms** - Framework for graphical interface

### External Libraries
- **InTheHand.Net.Personal (32-bit)** - Library for Bluetooth communication
- **Microsoft.VisualBasic.PowerPacks.Vs** - Additional components for Windows Forms

### APIs and Techniques
- **P/Invoke** - Native calls to Windows APIs (user32.dll)
- **HID (Human Interface Device)** - Protocol for device communication
- **Windows Registry** - For storing settings

## Design Patterns Implemented

1. **Singleton**
   - Used in main form classes (FormAppsMain, FormAppsDraw, FormAppsPresentation)
   - Ensures a single instance of critical components

2. **State Pattern**
   - Implemented in the ConnectorImpl class to manage different connection states
   - States include: NotInitialized, SearchingBluetooth, BluetoothClientFound, ConnectingDevice, InitializingMotionCapture, ReadyToUse

3. **Factory Method**
   - Used in the Connector class to create specific implementation instances
   - Allows runtime implementation substitution

4. **Observer Pattern**
   - Implemented through events for state change notification
   - Used for communication between system layers

5. **Template Method**
   - Used in the ConnectionStateImpl class to define operation flow
   - Allows subclasses to implement specific behaviors

## Directory Structure

```
IGloobeConnector/
├── connector/                  # Base library
│   ├── src/                    # Source code
│   │   ├── CalibrationData.cs  # Calibration data
│   │   ├── Commands/           # System commands
│   │   ├── Connector.cs        # Base abstract class
│   │   ├── ConsoleManager.cs   # Console management
│   │   ├── Gui/                # Common graphical interfaces
│   │   ├── Hardware.cs         # Hardware abstraction
│   │   ├── MotionCapture.cs    # Motion capture
│   │   └── Warper.cs           # Coordinate transformation
│   └── Properties/             # Assembly properties
│
├── motionCapture/              # Motion capture implementation
│   ├── src/                    # Source code
│   │   ├── DataTypes.cs        # Data types
│   │   ├── HidBinder.cs        # HID communication
│   │   ├── IGloobeMote.cs      # Main implementation
│   │   └── StateChanged*.cs    # State change events
│   └── Properties/             # Assembly properties
│
├── connectorWindows/           # Windows-specific implementation
│   ├── src/                    # Source code
│   │   ├── ConnectorImpl.cs    # Connector implementation
│   │   └── Connector/          # Helper classes
│   ├── dlls/                   # External libraries
│   └── Properties/             # Assembly properties
│
├── iGloobeAppsMain/            # Main applications
│   ├── src/                    # Source code
│   │   ├── DraggableForm.cs    # Base draggable form
│   │   ├── FormAppsMain.cs     # Main form
│   │   ├── FormAppsDraw.cs     # Drawing application
│   │   ├── FormAppsPresentation.cs # Presentation application
│   │   ├── Window.cs           # Window utilities
│   │   └── Windows.cs          # Window management
│   ├── img/                    # Image resources
│   └── Properties/             # Assembly properties
│
└── connectorMain/              # Main application
    ├── src/                    # Source code
    │   └── Start.cs            # Entry point
    └── Properties/             # Assembly properties
```

## Important Technical Details

### Bluetooth Communication
- Uses the InTheHand.Net.Personal library for Bluetooth communication
- Implements device discovery and automatic connection
- Manages reconnection in case of signal loss

### Motion Capture
- Processes infrared sensor data to determine position
- Uses smoothing algorithm to reduce jitter
- Implements calibration to map sensor coordinates to screen

### Coordinate Transformation (Warping)
- Implements coordinate transformation algorithm to map sensor space to screen space
- Supports four-point calibration for greater accuracy
- Applies smoothing to improve user experience

### Cursor Control
- Uses P/Invoke to call native Windows APIs for cursor control
- Implements mouse click simulation
- Supports different operation modes (absolute and relative)

### Graphical Interface
- Implements transparent and draggable forms
- Uses icons and visual resources for better user experience
- Supports different viewing modes (fullscreen, window)

## Limitations and Considerations

1. **Compatibility**
   - Developed for .NET Framework 2.0, which is an older version
   - May require adjustments to work on modern operating systems
   - Uses 32-bit libraries, which may cause issues on 64-bit systems

2. **Security**
   - Does not implement encryption in Bluetooth communication
   - Uses P/Invoke, which can pose security risks if not used correctly

3. **Maintainability**
   - Some values are hardcoded in the code
   - Limited internal documentation in some parts of the code
   - Uses legacy technologies (Windows Forms)

4. **Testing**
   - No evidence of automated tests
   - Relies on manual testing for validation

## Implemented Best Practices

1. **Separation of Concerns**
   - Each project has a well-defined responsibility
   - Classes with single responsibilities

2. **Use of Interfaces**
   - Well-defined interfaces for communication between components
   - Facilitates implementation substitution

3. **Error Handling**
   - Implementation of exception handling at critical points
   - Logs to facilitate debugging

4. **Modularity**
   - System divided into independent modules
   - Facilitates maintenance and extension

5. **Design Patterns**
   - Appropriate use of design patterns to solve common problems
   - Improves code structure and organization

## Conclusion

IGloobe is a well-structured application that demonstrates good programming practices, despite using older technologies. The system architecture is well thought out, with clear separation of responsibilities and appropriate use of design patterns.

The implementation of Bluetooth communication and motion capture is robust, with proper error and state handling. The graphical interface is minimalist and non-intrusive, providing a good user experience.

However, the use of older technologies (.NET Framework 2.0 and Windows Forms) may limit its compatibility with newer operating systems and make future maintenance difficult. The lack of automated tests is also a point of attention.

In summary, IGloobe represents a significant development effort, with a well-thought-out architecture and solid implementation, despite the technological limitations of the time it was developed.

## System Requirements

- Operating System: Windows XP/Vista/7
- .NET Framework 2.0 or higher
- Compatible Bluetooth adapter
- Compatible motion capture device (similar to Wiimote)
- Minimum screen resolution: 800x600

## License

This project was developed between 2010 and 2011, and its copyright belongs to the original developers.
