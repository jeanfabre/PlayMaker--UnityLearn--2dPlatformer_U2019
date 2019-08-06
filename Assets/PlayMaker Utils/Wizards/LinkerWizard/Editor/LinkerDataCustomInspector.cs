using System;
using System.Collections;
using System.Collections.Generic;

using System.Xml;

using UnityEngine;
using UnityEditor;

using HutongGames.PlayMakerEditor;

using HutongGames.PlayMaker.Ecosystem.Utils;

[CustomEditor(typeof(LinkerData))]
public class LinkerDataCustomInspector : Editor
{
	static string ActionsPackagePath = "PlayMaker Utils/Wizards/LinkerWizard/LinkerWizardActions.unitypackage";


	public override void OnInspectorGUI()
	{
		LinkerData _target = target as LinkerData;

		FsmEditorStyles.Init();

		//GUILayout.Box("Hello",FsmEditorStyles.LargeTitleWithLogo,GUILayout.Height(42f));
		GUI.Box (new Rect (0f, 0f, Screen.width, 42f), "Linker Wizard", FsmEditorStyles.LargeTitleWithLogo);


		GUILayout.Label("1: Make sure you installed them actions");

		if (GUILayout.Button("Install Actions"))
		{
			Debug.Log("importing package "+Application.dataPath+"/"+ActionsPackagePath);
			
			AssetDatabase.ImportPackage(Application.dataPath+"/"+ActionsPackagePath,true);

		}


		GUILayout.Label("2: Check 'debug' for tracking all reflections");

		EditorGUI.indentLevel++;
		bool _debug = EditorGUILayout.Toggle("Debug",_target.debug);
		EditorGUI.indentLevel--;
		if (_debug!=_target.debug)
		{
			_target.debug = _debug;
			EditorUtility.SetDirty(_target);
		}

		GUILayout.Label("3: Run your scenes from start to finish");
		GUILayout.Label("   Check the preview below for usages as they come ");

		if (_target.linkerEntries.Count>0)
		{
			GUILayout.Label("4: Update Linker xml file");
			if (GUILayout.Button("Update Linker content"))
			{
				UpdateLinkerContent(_target);
				GUIUtility.ExitGUI();
			}
			if (_target.LinkContentUpdateDone)
			{

				if (_target.Asset == null)
				{
					//Debug.Log("loading asset "+_target.AssetPath);
					_target.Asset = AssetDatabase.LoadAssetAtPath(_target.AssetPath,typeof(TextAsset)) as TextAsset;
				}

				if (_target.Asset!=null)
				{
					GUILayout.Label("5: You can now publish and test on device");
					GUILayout.BeginHorizontal();
					if (GUILayout.Button("Ping link.xml"))
					{

						EditorGUIUtility.PingObject(_target.Asset);
					}
					if (GUILayout.Button("Select link.xml"))
					{
		
						Selection.activeObject = _target.Asset;
						EditorGUIUtility.PingObject(_target.Asset);
					}
					GUILayout.EndHorizontal();
				}
			}
		}


		GUILayout.BeginHorizontal(GUILayout.Height(25));
			GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();
					FsmEditorGUILayout.Divider();
				GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.Label("preview",GUILayout.ExpandWidth(false));
			GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();
					FsmEditorGUILayout.Divider();
				GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
		GUILayout.EndHorizontal();


		foreach(KeyValuePair<string, List<string>> entry in _target.linkerEntries)
		{
			GUILayout.Label(entry.Key);
			foreach(string _item in entry.Value)
			{
				GUILayout.Label("    "+_item);
			}
		}
	}
	
	public void UpdateLinkerContent(LinkerData _target)
	{
		_target.LinkContentUpdateDone = false;

		_target.Asset = PlayMakerEditorUtils.GetAssetByName("link.xml") as TextAsset;


		_target.AssetPath = "Assets/link.xml";


		// create xml doc
		XmlDocument _doc = new XmlDocument();

		XmlNode _rootNode;
		if (_target.Asset != null)
		{
			_target.AssetPath = AssetDatabase.GetAssetPath(_target.Asset);
			_doc.LoadXml(_target.Asset.text);
			_rootNode = _doc.SelectSingleNode("linker");
		}else{
			_rootNode = _doc.CreateNode(XmlNodeType.Element,"linker",null);
			_doc.AppendChild(_rootNode);
		}

		if (_rootNode==null){
			Debug.LogError("Link.xml seems to be badly formatted");
			return;
		}

		foreach(KeyValuePair<string, List<string>> entry in _target.linkerEntries)
		{
			string assemblyName = entry.Key;

			XmlNode _assemblyNode = _doc.SelectSingleNode("//assembly[@fullname='"+assemblyName+"']");
			if (_assemblyNode==null)
			{
				_assemblyNode = _doc.CreateNode(XmlNodeType.Element,"assembly",null);
				_rootNode.AppendChild(_assemblyNode);

				XmlAttribute _fullnameAttr = _doc.CreateAttribute("fullname");
				_fullnameAttr.Value = assemblyName;
				_assemblyNode.Attributes.Append(_fullnameAttr);
			}

			foreach(string _type in entry.Value)
			{
				XmlNode _typeNode = _assemblyNode.SelectSingleNode("./type[@fullname='"+_type+"']");
				if (_typeNode==null)
				{
					_typeNode = _doc.CreateNode(XmlNodeType.Element,"type",null);
					_assemblyNode.AppendChild(_typeNode);
					
					XmlAttribute _fullnameAttr = _doc.CreateAttribute("fullname");
					_fullnameAttr.Value = _type;
					_typeNode.Attributes.Append(_fullnameAttr);

					XmlAttribute _preserveAttr = _doc.CreateAttribute("preserve");
					_preserveAttr.Value = "all";
					_typeNode.Attributes.Append(_preserveAttr);
				}
			}
		}


		Debug.Log("Updated and Saving linker xml content to : "+_target.AssetPath);
		_doc.Save(Application.dataPath+ (_target.AssetPath.Substring(6)));

		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow ();

		_target.Asset = AssetDatabase.LoadAssetAtPath(_target.AssetPath,typeof(TextAsset)) as TextAsset;

		_target.LinkContentUpdateDone = true;

		GUIUtility.ExitGUI();
	}

	[MenuItem("PlayMaker/Addons/Tools/Create Linker Wizard",false,0)]
	[MenuItem("Assets/Create/PlayMaker/Linker Wizard",false,0)]
	public static void CreateAsset ()
	{

		if (LinkerData.instance!=null)
		{
			string path = AssetDatabase.GetAssetPath(LinkerData.instance);
			if (string.IsNullOrEmpty(path))
			{
				LinkerData.instance = null;
			}else{
				
				Selection.activeObject = LinkerData.instance;
				EditorGUIUtility.PingObject(Selection.activeObject);
				Debug.Log("Linker Wizard already exists at "+path);
				return;
			}
		}
		
		// search in the assets:
		UnityEngine.Object[] _assets =	PlayMakerEditorUtils.GetAssetsOfType(typeof(LinkerData),".asset");
		
		if (_assets!=null && _assets.Length>0)
		{
			LinkerData.instance = _assets[0] as LinkerData;
			
			Selection.activeObject = LinkerData.instance;
			EditorGUIUtility.PingObject(Selection.activeObject);
			Debug.Log("Linker Wizard already exists at "+AssetDatabase.GetAssetPath(LinkerData.instance));
			return;
		}
		
		PlayMakerEditorUtils.CreateAsset<LinkerData>("Linker Wizard");
	}
}
