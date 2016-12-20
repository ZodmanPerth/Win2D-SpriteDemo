# Win2d Sprite Demo

This demo is a performance test application for Win2d Drawing Modes.
I wrote it so that I could test the performance of the game loop on various UWP devices with a single program.

By twiddling variables you control how much work the game loop does.
Performance is observed via the colour of objects on screen and a frame rate counter.

The Update event calculates the position of objects.
The Draw event draws objects and debug information.

Objects are drawn in opaque blue, but turn opaque red when Win2d reports that it is running slowly.

![Screenshot](/assets/screen.png "screenshot of the application running")

A menu is displayed by selecting the hamburger menu in the top left.

![Screenshot](/assets/menu.png "screenshot of the menu when open")

Items on the menu allow the control over various parameters:

* The number of objects to draw
* The change in theta (radians) per Update event
* Various parts of the equations controlling the X and Y coordinates
* The drawing mode
  * DrawEllipse - draws using DrawingSession.FillCircle()
  * SpriteBatch - draws using a sprite batch

The sprites are square so you can tell the difference when the menu is closed.

Display of debug information is toggled using the checkbox in the top left.
The debug information includes the frames per second of the application.