PlayMaker--Utils
================

## Licensing
This package is released under LGPL license: [http://opensource.org/licenses/LGPL-3.0](http://opensource.org/licenses/LGPL-3.0)


## Description
This is the authoritative source code for "PlayMaker Utils", used in many different projects, like ArrayMaker, XmlMaker, Mecanim, Pathfinding, UI and other Unity modules.

## Implementation
More infos on how to use this repository in your own project using Git and SubModules. 
Refer to the 2dPlatformer UnityLearn PlayMaker project to see how it's integrated. 
Instead of making hard copy, I use this repository as a sub module, allowing me to work on this utils, add functions and improve it without having to switch projects or manually integrate changes from one project to another.

## Including PlayMaker Utils in your Github Rep as a Sub Module using SourceTree
1: Delete the existing "PlayMaker Utils" folder  
2: commit this deletion  
3: in the side bar of SourceTree, right click "Add SubModule"  
4: SourcePath/Url should be https://github.com/jeanfabre/PlayMaker--Utils.git  
5: set the Locale Relative Path to "Assets/PlayMaker Utils"  
6: Click Ok, let sourcetree import this submodule  

Now, make sure you "show" the submodules when you work on that project, and make visible this submodule entry in the side bar, it will prompt you with a pull icon when needed.

## Pull a SubModule
Simple double click on the submodule entry in the side bar, it will open a SourceTree window of that rep, and pull as usual. Close this window when you are done pulling.

## pushing to this Submodule
Simple Double click on the submodule entry in the side bar, it will open a SourceTree window of that rep, and commit/push as usual. Contact me to get writing access and be allow to push.

