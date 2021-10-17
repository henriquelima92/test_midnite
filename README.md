What was done:
 
Used scriptable objects to store and facilitate the modification of game properties such as:
 
- Board
- Grid dimensions
- Quantity of ingredients
- Quantity of breads
- Background prefab
- Bread Prefab
- Prefabs of fillings
 
- Tiles
- Size of each tile
- Spacing between each tile
- Fill layer offset
 
UI buttons were added to the game to start a random level and to reset the current level. It’s also possible to choose a level to be loaded in a dropdown.
 
Another decision was to generate a one-dimensional ingredient list, containing fillings, breads and also null slots.

The list is randomized and applied to a grid based on scriptable object properties.
 
I decided to create it this way because it makes the movements between slots, the end game and the win game verification easier.
 
For the serialization part, I created a class that stores the randomly generated list of ingredients and save/load it in a json in the app’s appdata folder;

I also implemented the event system, using a graphic raycaster in 3D objects and events like OnPointerDown/OnPointerUp.
 
Added win/loss text feedbacks
 

What can be improved:
 
Creation of animation to rotate the ingredient layers. Currently the layers are only repositioned,
giving the impression that the sandwich is being rotated.
 Improvements in level randomization.
 Use audio feedback.
Improve the camera to follow the game properties changes.
