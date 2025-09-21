# STAR-PONG

<img align="left" style="width:350px" src="https://github.com/arceryz/star-pong/blob/master/StarPong.gif" width="380px">

STAR-PONG is a 2-player variant of pong where both player must protect their mothership
from the bomb, while also fighting each other with bullets in space. The game tries to
be inspired by the arcade era, with sounds, shaders and pixel art to match.

---
<br>
<br>

Created by:
- Timothy van der Valk (Programmer/Creator) [Website](https://timothyvv.nl/) [Github](https://github.com/)
- Hunter Short (Music) [LinkedIn](https://www.linkedin.com/in/hunter-short-68708ba4/)
- Theo (Sound Effects) [link??]
- Ro (Art) [LinkedIn](https://www.linkedin.com/in/ro-van-der-neut-679a6a380/)

## Features

- 2-player and player-vs-AI mode.
- Pong gameplay where the bomb direction can be controlled.
- Dogfighting with bullets, adding another strategic layer.
- Energy system for bullets and shield.
- Shield to deflect bullets and bomb, acts as the pong paddle.
- Animated sprites for player ship, bomb, bullets, and vfx.
- Parallax scrolling starfield background.
- Soundtracks for title, gameplay, sudden death and game end.
- Sound effects for most game elements.
- CRT shader effect for retro feel.
- Turbo-mode in settings menu, makes everything ultra-rapid-fire and faster (including AI).
- Health bars and score display in-game.
- Tons of explosions and and fire.
- Screen shake integrated in the CRT-shader post process.
- Debug visualisations for collision boxes (Press `(Z)`) and scene tree (Press `(T)`).
- Easter egg^^.

This game and all its code, except that in [/External](/External), was written by me from the Monday 8th of September 2025, to Sunday 21st. 
It is written for the course *Game Programming 1* at the Utrecht University of Applied Sciences. The assignment was to create *Pong*.
I decided to collaborate with friends to extend the basic implementation of pong to this.

All of the assets, which includes Music, Sound Effects and Art, we're created specifically for this project in the same time frame or shorter. 
The only external code I used is the [FastNoise Library](https://github.com/Auburn/FastNoise_CSharp) for the random movement of the mothership,
and the [Primitives2D Library](https://github.com/DoogeJ/MonoGame.Primitives2D) for drawing debug shapes. None of these libraries are critical to the game.

The game uses CRT-effect shader that I wrote myself. It features a barrel distortion, scanline effect, blurring and bar lines effect.

## Controls

The controls are explained in the game as well. The game is designed for two players to play on the same keyboard.
The AI opponent takes control of the red team if enabled.

|   | Blue | Red  |
|------|------|------|
| Ship Movement | W/S | Up/Down arrows |
| Shoot | C | O |
| Toggle Shield | V | P |

In addition to this, there are some shortcut controls explained on the title screen as well:
- `(Z)` Toggle collisionbox visualisations.
- `(T)` Toggle Scene Tree visualiser. Useful for debugging and learning.
- `(Esc)` Quit the game.
- `(F)` Toggle fullscreen.
- `(Shift-R)` Return to title screen from anywhere.

The game window can be resized, and the game will scale to fit the window while maintaining aspect ratio.
This code is inside the *Engine* class. It uses a rendertarget to render the game to, and then draws that to the real screen.

## Technical Details

Written in C# using Monogame, I initially created a naive implementation of pong with no notion of a game objects or reusable components.
When I started extending the game to add bullets, background layers and other objects, it became quickly apparent that a more structured approach was needed.
This is when I started to implement a GameObject system, inspired by Godot's Scene Tree hierarchy. Components are game objects, and they can be organized
in a tree structure to inherit position, draw layers and update order. This system made writing the game much more pleasant. Logic for anything can be
encapsulated in a game object and then added to the scene.

Speaking of *scenes*, these are also just nodes in the tree. The Scene Tree simply switches the root node when changing scenes. Garbage collection
is handled by clearing up references to the old scene. The collision detection system uses the "Groups" feature of the Scene Tree, it queries
all objects in the "physics" group and then performs naive O(n^2) collision detection. This is not optimal, but it works fine for the small amount of objects in the game.
I extended the input system from Monogame to track key *presses* as well.

Building on top of the scene tree, I used a draw sorting system that sorts objects based on their draw layer and their child index.
This ensures that children always draw on top of parents, but that draw layers are still respected. This is similar to Godot's architecture.

The core of the engine consists of
- [Engine](/Source/Engine.cs)
- [Scene Tree](/Source/Framework/SceneTree.cs)
- [Draw Sorter](/Source/Framework/DrawSorter.cs)
- [Physics](/Source/Framework/Physics.cs)
- [Input](/Source/Framework/Input.cs)

Everything else is part of the framework for building actual games (sprites, buttons, labels, fonts etc). No AI was used in the writing of this code.
All design decisions are made by me, and all of this code is written by me, by hand. I learned a lot about comfortable ways to structure game engine code,
dealing with assets and so on. The result is a sturdy framework that can be used for future assignments.