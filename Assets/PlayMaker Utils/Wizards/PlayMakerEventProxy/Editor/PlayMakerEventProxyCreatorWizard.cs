// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using UnityEditor;
using UnityEngine;

using HutongGames.PlayMakerEditor.Ecosystem.Utils;

namespace HutongGames.PlayMakerEditor
{
	#pragma warning disable 168
	public class PlayMakerEventProxyCreatorWizard : EditorWindow
	{ 
		static readonly string __ListFoldOutPrefKey__   = "PlayMakerEventProxyCreatorWizard:ListFoldOut";

		PlayMakerEventProxyCreator eventProxyCreator = new PlayMakerEventProxyCreator();

		public PlayMakerEventProxyCreator.PlayMakerEventProxyCreatorDefinition currentDefinition = new PlayMakerEventProxyCreator.PlayMakerEventProxyCreatorDefinition();

		bool ReBuildPreview;

		bool isCompiling;

		#region UI

		GUIStyle labelStyle;

		private void OnFocus()
		{
			Repaint();
		}

		public void OnGUI()
		{ 
			wantsMouseMove = true;
			if (Event.current.type == EventType.MouseMove) Repaint ();


			if (isCompiling!=EditorApplication.isCompiling)
			{
				isCompiling = EditorApplication.isCompiling;
			}

			FsmEditorStyles.Init();

			// set style ot use rich text.
			if (labelStyle==null)
			{
				labelStyle = GUI.skin.GetStyle("Label");
				labelStyle.richText = true;
			}

			FsmEditorGUILayout.ToolWindowLargeTitle(this, "Event Proxy Creator");

			// for the banner odd bug on height
			GUILayout.Space(15);

			GUILayout.Label("This wizard lets you create a Component with a public method.\n" +
			                "When this method is called, it will send a PlayMaker event.\n" +
			                "You can define this PlayMaker event in the component Inspector.\n" +
			                "\nUse this when you expect Unity or third parties to fire messages\n" +
			                "and you want to catch that message as a PlayMaker Event");
			
			FsmEditorGUILayout.Divider();

			OnGUI_DoDefinitionForm();

			FsmEditorGUILayout.Divider();


			OnGUI_DoProxyList();

			GUILayout.Space(5);

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();

			if (isCompiling)
			{
				GUILayout.Label("Unity is compiling, please wait");
			}else{
				GUILayout.Label(" ");
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		bool ListFoldOut;

		[SerializeField]
		Vector2 listScrollPosition;
		
		Dictionary<string,EventProxyFileDetails> _list;
		bool repaintNeeded;

		void OnGUI_DoProxyList()
		{
			if (repaintNeeded)
			{
				repaintNeeded = false;
				Repaint();

			}

			// DON'T ADD ANYTHING HERE, the heightis hardcoded... see tofix note below
			int count = _list==null?0:_list.Count;

			// foldout option
			bool newListFoldOut = EditorGUILayout.Foldout(ListFoldOut,"Editable proxies in this project ("+count+")");
			if ( newListFoldOut!=ListFoldOut)
			{
				ListFoldOut = newListFoldOut;
				EditorPrefs.SetBool(__ListFoldOutPrefKey__,newListFoldOut);
			}

			try{
				if (newListFoldOut)
				{
					listScrollPosition = GUILayout.BeginScrollView(listScrollPosition);
					
					if (_list!=null)
					{
						foreach(KeyValuePair<string,EventProxyFileDetails> i in _list)
						{
							
							OnGUI_DoEditableEnumItem(i.Key,i.Value);
							
						}
					}
					
					GUILayout.EndScrollView();
				}else{
					GUILayout.FlexibleSpace();
				}
			}catch(Exception e)
			{
				repaintNeeded =true;
				EditorGUIUtility.ExitGUI();
			}

		}
		

		void OnGUI_DoEditableEnumItem(string filePath,EventProxyFileDetails details)
		{
			bool _isCurrentlyEdited = (currentDefinition.FolderPath+currentDefinition.Name+".cs").Equals(details.projectPath);
			// check if this is the item we are currently editing
			if (_isCurrentlyEdited)
			{
				_currentFileDetails = details;
				GUI.color = Color.green;
			}
			GUILayout.BeginHorizontal("box",GUILayout.ExpandHeight(false));

			GUI.color = Color.white;

			GUILayout.Label(details.nameSpace+"."+details.methodName);
			
			GUILayout.FlexibleSpace();
			
			if (GUILayout.Button("Select in Project","MiniButton"))
			{
				var _object = AssetDatabase.LoadAssetAtPath("Assets/"+details.projectPath,typeof(UnityEngine.Object));
				
				EditorGUIUtility.PingObject(_object.GetInstanceID());
				Selection.activeInstanceID = _object.GetInstanceID();
			}
			
			if (!_isCurrentlyEdited)
			{
				if (GUILayout.Button("Edit","MiniButton",GUILayout.Width(40)))
				{
					StartEditingExistingProxy(details);
				}
			}else{
				GUI.color = Color.yellow;
				if (GUILayout.Button("Cancel","MiniButton",GUILayout.Width(40)))
				{
					StartEditingExistingProxy(details);
				}
				GUI.color = Color.white;
			}

			
			GUILayout.EndHorizontal();
		}


		void OnGUI_DoDefinitionForm()
		{
			Color _orig = Color.clear;
			ReBuildPreview = false;

			/*
			if (currentFileDetails!=null)
			{
				GUILayout.Label("You are editing an existing enum");
				FsmEditorGUILayout.Divider();
			}
			*/
			// FOLDER
			_orig = GUI.color;
			if (!currentDefinition.FolderPathValidation.success)
			{
				GUI.color = new Color(255,165,0);
			}
			GUILayout.Label("Project Folder: <color=#ffa500><b>"+currentDefinition.FolderPathValidation.message+"</b></color>");
			currentDefinition.FolderPath = GUILayout.TextField(currentDefinition.FolderPath);
			GUI.color = _orig;

			// NAMESPACE
			_orig = GUI.color;
			if (!currentDefinition.NameSpaceValidation.success)
			{
				GUI.color = Color.red;
			}
			GUILayout.Label("NameSpace: <color=#B20000><b>"+currentDefinition.NameSpaceValidation.message+"</b></color>");
			string _nameSpace = GUILayout.TextField(currentDefinition.NameSpace);
			GUI.color = _orig;
			if (!string.Equals(_nameSpace,currentDefinition.NameSpace))
			{
				currentDefinition.NameSpace = _nameSpace;
				ReBuildPreview = true;
			}

			// Method Name
			_orig = GUI.color;
			if (!currentDefinition.PublicMethodValidation.success)
			{
				GUI.color = Color.red;
			}
			GUILayout.Label("Public Method/Message Name: <color=#B20000><b>"+currentDefinition.PublicMethodValidation.message+"</b></color>");
			string _methodName = GUILayout.TextField(currentDefinition.PublicMethodName);
			GUI.color = _orig;
			if (!string.Equals(_methodName,currentDefinition.PublicMethodName))
			{
				currentDefinition.Name = _methodName+"Proxy";
				currentDefinition.PublicMethodName = _methodName;
				ReBuildPreview = true;
			}

			GUILayout.Label("FileName being generated: "+currentDefinition.Name+".cs");

			FsmEditorGUILayout.Divider();

			if (!currentDefinition.DefinitionValidation.success)
			{
				GUILayout.Label("<color=#B20000><b>"+currentDefinition.DefinitionValidation.message+"</b></color>");
			}

			if (currentDefinition.DefinitionValidation.success)
			{
			
				//if (_currentFileDetails!=null) GUILayout.Label(_currentFileDetails.projectPath);
				// check if this is the item we are currently editing
				string _label = "Create";
				bool _isUpdating = _currentFileDetails!=null && (currentDefinition.FolderPath+currentDefinition.Name+".cs").Equals(_currentFileDetails.projectPath);
				if (_isUpdating )
				{
					_label = "Save Modification";
					GUI.color = Color.yellow;
				}

				if (GUILayout.Button(_label)) // Label "Save Changes" when we detected that we are editing an existing enum
				{
					eventProxyCreator.CreateProxy(currentDefinition);
					_list = EventProxyFileFinder.FindFiles();
				}
				GUI.color = Color.white;

				if (_isUpdating)
				{
					GUILayout.Label("If this component is already in use, this may break your logic.");
				}

			}else{
				
				Color _color = GUI.color;
				
				_color.a = 0.5f;
				GUI.color = _color;
				GUILayout.Label("Fix form before saving","Button");
				_color.a = 1f;
				GUI.color =_color;
			}


			if (ReBuildPreview )
			{
				currentDefinition.ValidateDefinition();
				
				//enumCreator.BuildScriptLiteral(currentEnum);
				Repaint();
			}
		}

		 EventProxyFileDetails _currentFileDetails;

		void StartEditingExistingProxy(EventProxyFileDetails details)
		{

			currentDefinition.FolderPath = details.projectPath.Substring(0,details.projectPath.Length -(details.className.Length+3));
			currentDefinition.Name = details.className;
			currentDefinition.PublicMethodName = details.methodName;
			currentDefinition.NameSpace = details.nameSpace;

			_currentFileDetails = details;
		}

		#endregion
		

		#region Window Management
		
		public static PlayMakerEventProxyCreatorWizard Instance;


		[MenuItem ( "PlayMaker/Addons/Tools/Event Proxy Wizard")]
		static public void Init () {
			
			// Get existing open window or if none, make a new one:
			Instance = (PlayMakerEventProxyCreatorWizard)EditorWindow.GetWindow (typeof (PlayMakerEventProxyCreatorWizard));
			
			Instance.Initialize();
		}
		
		public void Initialize()
		{
			//Debug.Log("Init");
			Instance = this;
			
			InitWindowTitle();
			position =  new Rect(120,120,400,370);
			// initial fixed size
			minSize = new Vector2(400, 370);

		}
		
		public void InitWindowTitle()
		{
			#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0
			title = "Proxy Creator";
			#else
			titleContent = new GUIContent("Proxy Creator");
			#endif
		}
		
		
		protected virtual void OnEnable()
		{
			// Debug.Log("OnEnable");
			// scan the project for enumfiles generated by this Wizard
			_list = EventProxyFileFinder.FindFiles();

			
			ListFoldOut = EditorPrefs.GetBool(__ListFoldOutPrefKey__,true);
		}

		#endregion Window Management
	}
}