I have a problem with the game not running on 60FPS, help me reach
at least 60 FPS. Here is my MainForm code:  
[MainForm.cs] \
And here is the designer side: \
[MainForm.Designer.cs] \
Help me improve the target FPS, it should already reach thanks to the timer to the timer that is set on 16ms, but it
caps out at 40 FPS.
Don't change the code that could influence the game logic. Make sure to take into accountability that if you speed up
the loop
itself it speeds up the whole game.

[UiLayer.cs] \
// PROMPT: I need you to implement these features: Every time I gain some xp I need a Statístic Bar (something like in Minecraft there is a "boss-bar" which
// displays the health of the current boss you are fighting, but in this case it will be an experience bar) to display at the top of the screen with the name
// of the skill and the amount of xp gained, and it should disappear after 4 seconds.
// Also, when I level up I want a message to appear in the middle of the screen with the text "LEVEL UP!" and
// "You are now Level X" where X is the new level number, and this message should have a nice animation and disappear after 2 seconds.
// Please implement these features in the UiLayer class.
// Keep the structure and architecture as CLOSE to other RenderLayers and my other code! 
