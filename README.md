# Golfery

Over a period of 48 hours, I developed a silly web game about shooting targets at arrows for the [GMTK Game Jam 2023](https://itch.io/jam/gmtk-2023). It's a short but fun prototype that serves as a polished proof of concept. Your score is based on a mix of which ring of the target hits the arrow, and the total number of shots over or under par it takes to complete the level.

These are a selection of some of the scripts I wrote for the Game Jam:
* [Aim, Draw Trajectory, and Fire](https://github.com/JACPro/Golfery/blob/main/FireTarget.cs)
* [Scene Manager](https://github.com/JACPro/Golfery/blob/main/SceneLoader.cs)
* [Score Manager](https://github.com/JACPro/Golfery/blob/main/ScoreManager.cs)
* [Pause Manager](https://github.com/JACPro/Golfery/blob/main/PauseManager.cs)
* [Input Manager using the "New" Input System](https://github.com/JACPro/Golfery/blob/main/InputManager.cs) 
* [Audio Manager](https://github.com/JACPro/Golfery/blob/main/AudioManager.cs)
* [Handling UI events with the Observer pattern](https://github.com/JACPro/Golfery/blob/main/OnSelectEvents.cs)
* [Wobbly TextMeshPro Letters](https://github.com/JACPro/Golfery/blob/main/WobblyText.cs)

And this is the shader I made with Unity ShaderGraph to work with the LineRenderer for showing trajectory:
* [Dashed Line Shader](https://github.com/JACPro/Golfery/blob/main/DashedLine.shadergraph)

___

<img src="https://github.com/JACPro/Golfery/blob/main/golfery.gif" title="Level 1"></img>

___
#### To play Golfery, [click here](https://jamesacpro.itch.io/golfery)
___
