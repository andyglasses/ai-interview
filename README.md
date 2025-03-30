# Advanced Instruments Interview Task

## Requirements

|REQID|Name|Status|Comments|
|-|-|-|-|
|REQ01| Application must be provided as a Windows Desktop application.| Met | WPF application |
|REQ02| Application must provide user interface listing all axis registered in the motion control system| Met | All four shown including dodgy no unit one |
|REQ03| Application must provide information about position and velocity for all axis registered in the motion system.| Met |  |
|REQ04| Application must perform automatic update of the position and velocity information on the screen at least once per second.| Met |  |
|REQ05| Application must automatically connect to the hardware via the API provided in SolentimHardwareAccessLayer.dll| Met | Wrapper created in core layer |
|REQ06| Application must allow the user to select preferred metrics unit for position information (one setting for all axis)| Met |  |
|REQ07| Application must allow the user to select preferred metrics unit for velocity information (one setting for all axis)| Met |  |
|REQ08| Application must handle emergency stop events without crashing.| Met | Exception  handling added to the wrapper |
|REQ09| Application must resume normal operation after an emergency stop event has been detected.| Met | Auto and manual reset added |


## Notes
* I am converting all units to their 'count' equivalent as it is the smallest unit and then converting back to another unit when needed.  (I am uncertain if this is the best method, maybe using a SI unit as the base might be best?  It would not take much work to swap)
* I am using the enums provided in the dll throughout the solution.  If I had more time I would probably not use the Enum in a 3rd party dll and instead convert to one I had control / visibility of and convert inside my service to prevent updates the 3rd party dll from breaking the app.
* Discounted using DDD as there is not much complexity in the domain.  I have used a service/core layer to keep the UI layer cleaner.


## With more time
* Added proper logging instead of catching and swallowing exceptions (ILogger interface, personal preference is the Serilog library)
* Extract background worker and timer stuff into separate class(es)
* Better Polling mechanism, maybe using a timer instead of a while loop
* Move text to resource files for easy translation
* Move options out of main window and into a child element.
* Make it look better
* Handle the NotSet Unit axis better
* Better error messaging
* Create view components for error messages, position and velocity sections to reduce repetition in the xaml (however the code to write these might be greater than the duplciated code in  such a small  project)
* Use style sheets (Resource Dictionary)
* Check keyboard navigation for A11y.
* Option to change hardware polling rate
* Handle emergency stop better by bubbling it up as a specific thing instead of just an 'error'

## Questions I would ask

***Assumption*** means I have made an assumption in my implementation in leau of actually getting an answer from someone.

1. What rounding rules do you use for the conversion of units?\
  ***Assumption***: I have assumed that doing no rounding until the presentation layer, and then using 3dp.
1. How should the data be displayed?\
  ***Assumption***: I have assumed that the data should be displayed in a text format.  If a graph is required, I would look for a suitable control library to use.
1. Does the user want to see all or one of the axis at a time?\
  ***Assumption***: I have assumed that the user wants to see all axis at once.  If they wanted to see one at a time, I would add a dropdown to select the axis.
1. Do I need to validate current axis values against the min/max?\
  ***Assumption***: I have assumed that the values are always valid. They are provided for user information (and potentially graphing bounds if a graphical representation was added).
1. How should I handle a 'not set' unit being returned from the hardware?\
  ***Assumption***: I have assumed that I should display an error in the UI and assume 'counts'.  If I had more time would probably add an option to set the unknown units in the UI.


## Assumptions
* The conversion of inches to mm is done by multiplying the value by 25.4 (1 inch = 25.4 mm)
* The number of axis is static.
* Emergency stop exception is only thrown by the GetCurrentVelocity and GetCurrentPosition methods. (Extra wrapping of the other methods & props could be added so they handle it too)
* Once the GetCurrent... methods have thrown an exception, I do not need to capture any other value or error for the current axis check (Logging could be added to log all errors even if only one is bubbled up to the user).
* The Velocity & Position Min-Max-Units values of the Axis do not change over time.  If they did, I would need to include calling them in the 'get current state' calls.
* Only current position & velocity and not historic values are required.
* For bigger projects I have used libraries for such as Prism for WPF, but for this small project I have not included it.
* I would have a conversation about the layout.  This uses a very simple layout.
* Getting values from an axis one at a time is performant enough. If it was not, I would look at getting all values at once by dispatching multiple threads to get the values.