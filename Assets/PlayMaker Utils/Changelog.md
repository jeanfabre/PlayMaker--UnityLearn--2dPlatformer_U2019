#PlayMaker Utils Change log

###1.2.2 beta
**Release Date:** 12/01/2016  

**Fix:** Unity 4.7 compatibility 

###1.2.1 beta
**Release Date:** 14/12/2015  

**Fix:** Unity 5.3 new SceneManagement obsolete calls  
**Fix:** PlayMaker 1.8f36 change in EventTarget setup for sending events programmatically

###1.2.0 beta
**Release Date:** 11/12/2015  

**New:** New Component public class with related propertyDrawers for `PlayMakerFsmVariableTarget` and `PlayMakerFsmVariable`  
**New:** New utils to list variables by string based on a FsmVariables reference   
**New:**  New Reflections Utils to get the BaseProperty of a SerializedProperty   

###1.1.6
**Release Date:** 19/09/2015  

**New:** New function to create global events  
**Fix:** Added support for PlayMakerEvent propertyDrawer to work within stateMachine context  
**Improvement:** Better description for auto generated "PlayMaker sent even proxy"  
  
###1.1.5
**Release Date:**  16/09/2015  

**New:** Event Proxy Creator wizard  

###1.1.4
**Release Date:**  10/09/2015  

**New:** Custom Sample for linker wizard  
**Improvement:** Moved LinkerWizard package into U4 github rep to centralize content  
**Improvement:** moved all menus into *Addons* section  


###1.1.3
**Release Date:**  26/08/2015  

**New:** LinkerData introspection system  
**New:** new PlayMakerUtils tools for Assets( creation and search)  
**New:** PlayMakerFsmTarget PropertyDrawer  
**New:** StateSynchronizer Component  


###1.1.2
**New:** Added automatic byte conversion case

**Fix:** removed logs  

###1.1.1
**Fix:** Fixed PlayMakeEvent Broadcasting call  
**Fix:** Fixed PlayMakerEvent PropertyDrawer when EventTarget is undefined  
**Fix:** Fixed Missing StringComparision enum for windows mobile


###1.1.0
**New:** Added versioning and change long following Ecosystem convention for future distribution  
**New:** Merge new public variables dedicated for PlayMaker integration in proxy Components
**New:** Explicit opensource licensing under [LGPL-3.0](http://opensource.org/licenses/LGPL-3.0) (see README.md)

**Fix:** Support for Unity 5 WebGL target
**Fix:** included generic attributes inside HutongGames.PlayMaker.Ecosystem.Utils namespace
  

###1.0.0
**New:** Initial release

