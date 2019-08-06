# UnityLearn 2dPlatformer PlayMaker


This is a 100% PlayMaker port of Unity 2d Platformer learning project 

This project is a work in progress:

## Description

you need the following setup:

 - Unity 2019.2.0 at least
 - PlayMaker 1.9.0
 
 Improvements over the original version:
 
 - Leak with Enemies
 The original script when enemies fall into the water has a memory leak: the enemy GameObject is not destroyed, leading a definite Memory Warning on Mobile devices and a crash.
 
 - No Pooling system
 The original system creates and destroy rockets, enemies and background props, while it runs fine on most modern platforms, it is known that creating and destroying GameObject has a performance impact. This PlayMaker version offers a simple custom Pooling solution (WIP)
 
 - Bad Scene setup
  The original version is not very clean in the way the hierarchy is organised, typically, objects are being created without proper parenting resulting in a very clunky root with constant creation and deletion of object and so it's near impossible to select a GameObject while the scene is running. The PlayMaker version parent all created object so that the initial hierarchy remains as is.
  
 - Old UI
  This PlayMaker version uses the new Unity UI system, so this is an added value to this project as you can see how to integrate the new UI system in a proper project.
    -- Health Bar is following the Player, so this demonstrate important interaction between the 3d world and the UI Canvas.

## BenchMark
coming soon, see Unity 5 version for last benchmarks.

https://github.com/jeanfabre/PlayMaker--UnityLearn--2dPlatformer




**Notes:**

Overall, this is very interesting to see that the pure PlayMaker solution doesn't expose any issues or noticeable downside on playability and performances, a 1 or 2 frame difference for the FPS isn't really an issue given that this will vary very much based on the client computer performances. If you have other states numbers, please share so we can get a sense of the variation across different computers. Noticeable, Memory allocation from Xcode debugging shows 10MB more for PlayMaker. It should be expected that more memory is allocated when using big frameWork like PlayMaker.

#### TODOS:
-- The background parallax system needs porting to PlayMaker
-- Add Touch Support
-- Make a Networked version using Photon
