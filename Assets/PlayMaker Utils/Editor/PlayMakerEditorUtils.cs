// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


using HutongGames;
using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;


public class PlayMakerEditorUtils : Editor {
	
	#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
	[MenuItem ("PlayMaker/Addons/Tools/Export Current Scene",false,100)]
	public static void ExportCurrentScene()
	{

		if (!EditorApplication.SaveCurrentSceneIfUserWantsTo())
		{
			return;
		}
		
		EditorUtility.DisplayDialog("PlayMaker dll export","Just a reminder that PlayMaker.dll file is not redistributable,\n\nMake sure you uncheck: \n'Assets/PlayMaker/PlayMaker.dll'\n\nwhen exporting a package to sharing with others.","Ok");
		
		
		var _sel =  Selection.objects;
		
		
		if (EditorUtility.DisplayDialog("Export Globals?","If your scene if using global variables, it will need to be included in the package as well.","Export Globals","Ignore Globals"))
		{
			EditorApplication.ExecuteMenuItem("PlayMaker/Tools/Export Globals");
			var _globalAsset = AssetDatabase.LoadAssetAtPath("Assets/PlaymakerGlobals_EXPORTED.asset",typeof(UnityEngine.Object));
			ArrayUtility.Add<UnityEngine.Object>(ref _sel ,_globalAsset);
			Selection.objects = _sel;
		}
		
	
		SelectSceneCustomAction();
		
		
	
		var _scene = AssetDatabase.LoadAssetAtPath(EditorApplication.currentScene,typeof(UnityEngine.Object));
		_sel =  Selection.objects;
		ArrayUtility.Add<UnityEngine.Object>(ref _sel ,_scene);
		Selection.objects = _sel;
		EditorApplication.ExecuteMenuItem("Assets/Export Package...");
	}
	
	
	[MenuItem ("PlayMaker/Addons/Tools/Export Current Scene", true)]
	public static bool CheckExportCurrentScene() {
	    return !String.IsNullOrEmpty(EditorApplication.currentScene);
	}
	
	
	[MenuItem ("PlayMaker/Addons/Tools/Select Current Scene Used Custom Actions")]
	public static void SelectSceneCustomAction()
	{
		UnityEngine.Object[] _list = GetSceneCustomActionDependencies();
		
		var _sel =  Selection.objects;
		ArrayUtility.AddRange<UnityEngine.Object>(ref _sel ,_list);
		Selection.objects = _sel;	
	}

	#endif

	public static UnityEngine.Object[] GetSceneCustomActionDependencies()
	{
		
		UnityEngine.Object[] list = new UnityEngine.Object[0];
		
		FsmEditor.RebuildFsmList();

		List<PlayMakerFSM> fsms = FsmEditor.FsmComponentList;
		
//		List<System.Type> PlayMakerActions = FsmEditorUtility.Actionslist;

		foreach(PlayMakerFSM fsm in fsms)
		{
			
			//Debug.Log(FsmEditorUtility.GetFullFsmLabel(fsm));
			
			//if (fsm.FsmStates != null) fsm.FsmStates.Initialize();
			
			for (int s = 0; s<fsm.FsmStates.Length; ++s)
			{
				
					fsm.FsmStates[s].LoadActions();
				
					Debug.Log(fsm.FsmStates[s].Name+" is loaded:"+fsm.FsmStates[s].ActionsLoaded);
				
					// Show SendEvent and SendMessage as we find them
					foreach(FsmStateAction action in fsm.FsmStates[s].Actions)
					{
						UnityEngine.Object _asset = FsmEditorUtility.GetActionScriptAsset(action);
						string _name = action.Name;
						if (String.IsNullOrEmpty(_name))
						{
							if (_asset!=null)
							{
								_name = _asset.name;
							}else
							{
								_name = FsmEditorUtility.GetActionLabel(action) + "[WARNING: FILE NOT FOUND]";
							}
							
						}
					
						if (Enum.IsDefined(typeof(WikiPages),_name))
						{
						//	Debug.Log(_name+" : official action");
						}else{
						//	Debug.Log(_name+" : custom action");
						
							if (_asset!=null)
							{
								ArrayUtility.Add<UnityEngine.Object>(ref list ,_asset);
							}
						}
							
					}
			}
			
			
		}
		
		return list;
		
	}// GetSceneCustomActionDependencies
	
	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static void CreateAsset<T> (string name="") where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
		
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") 
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}

		string _name = string.IsNullOrEmpty(name)? "New " + typeof(T).ToString() : name ;

		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + _name + ".asset");
		
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}


	
	/// <summary>
	/// Used to get assets of a certain type and file extension from entire project
	/// </summary>
	/// <param name="type">The type to retrieve. eg typeof(GameObject).</param>
	/// <param name="fileExtension">The file extention the type uses eg ".prefab".</param>
	/// <returns>An Object array of assets.</returns>
	public static UnityEngine.Object[] GetAssetsOfType(System.Type type, string fileExtension)
	{
		List<UnityEngine.Object> tempObjects = new List<UnityEngine.Object>();
		DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
		FileInfo[] goFileInfo = directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories);
		
		int i = 0; int goFileInfoLength = goFileInfo.Length;
		FileInfo tempGoFileInfo; string tempFilePath;
		UnityEngine.Object tempGO;
		for (; i < goFileInfoLength; i++)
		{
			tempGoFileInfo = goFileInfo[i];
			if (tempGoFileInfo == null)
				continue;
			
			tempFilePath = tempGoFileInfo.FullName;
			tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
			
			//Debug.Log(tempFilePath + "\n" + Application.dataPath);
			
			tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(UnityEngine.Object)) as UnityEngine.Object;
			if (tempGO == null)
			{
				//Debug.LogWarning("Skipping Null");
				continue;
			}
			else if (tempGO.GetType() != type)
			{
				//Debug.LogWarning("Skipping " + tempGO.GetType().ToString());
				continue;
			}
			
			tempObjects.Add(tempGO);
		}
		
		
		
		
		return tempObjects.ToArray();
	}
	
	public static UnityEngine.Object GetAssetByName(string fileName)
	{
		DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
		FileInfo[] goFileInfo = directory.GetFiles("*" + fileName, SearchOption.AllDirectories);
		
		int i = 0; int goFileInfoLength = goFileInfo.Length;
		FileInfo tempGoFileInfo; string tempFilePath;
		UnityEngine.Object tempGO;
		for (; i < goFileInfoLength; i++)
		{
			tempGoFileInfo = goFileInfo[i];
			if (tempGoFileInfo == null)
				continue;
			
			tempFilePath = tempGoFileInfo.FullName;
			tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
			
			//	Debug.Log(tempFilePath + "\n" + Application.dataPath);
			
			tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(UnityEngine.Object)) as UnityEngine.Object;
			if (tempGO == null)
			{
				Debug.LogWarning("Skipping Null");
				continue;
			}
			
			return tempGO;
		}
		
		return null;
	}

}
