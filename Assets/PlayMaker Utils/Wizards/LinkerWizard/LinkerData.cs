using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	
	public class LinkerData : ScriptableObject
	{
		public bool debug = true;
		public bool LinkContentUpdateDone = false;
		public TextAsset Asset;
		public string AssetPath;

		public LinkerData self;

		// TODO: make it serializable...
		public Dictionary<string,List<string>> linkerEntries = new Dictionary<string, List<string>>();


		static public bool DebugAll
		{
			get{
				if (instance==null)
				{
					return true;
				}
				return instance.debug;
			}
		}

		static private LinkerData _instance_;


		static public LinkerData instance
		{
			get{
				if (_instance_==null)
				{
					return null;
				}
				return _instance_.self;
			}

			set{
				_instance_ = value;
			}

		}

		public static void RegisterClassDependancy(string assemblyName,string typeName)
		{
			if (instance ==null)
			{
				Debug.LogWarning("LinkerData is missing an instance, please create one first in your assets from the create menu: Assets/Create/PlayMaker/Create Linker Wizard");
				return;
			}

			instance.RegisterLinkerEntry(assemblyName,typeName);
		}

		public void RegisterLinkerEntry(string assemblyName,string typeName)
		{
			if (instance ==null)
			{
				Debug.LogWarning("LinkerData is missing an instance, please create one first in your assets from the create menu: Assets/Create/PlayMaker/Create Linker Wizard");
				return;
			}

			if (string.IsNullOrEmpty(assemblyName))
			{
				Debug.LogError("LinkerEntry missing <color=blue>assemblyName</color>");
				return;
			}
			if (string.IsNullOrEmpty(typeName))
			{
				Debug.LogError("LinkerEntry missing <color=blue>typeName</color>");
				return;
			}

			// clean up assembly
			if (assemblyName.Contains(","))
			{
				assemblyName = assemblyName.Split(","[0])[0];
			}

			if (!linkerEntries.ContainsKey(assemblyName))
			{
				linkerEntries.Add(assemblyName,new List<string>());
			}

			linkerEntries[assemblyName].Add(typeName);
		}


		void OnEnable() {
			LinkerData._instance_ = this;
			self = this;
		}


//	#if UNITY_EDITOR
//
//		[MenuItem("PlayMaker/Addons/Tools/Create Linker Wizard")]
//		[MenuItem("Assets/Create/PlayMaker/Linker Wizard")]
//		public static void CreateAsset ()
//		{
//
//
//			if (LinkerData.instance!=null)
//			{
//				string path = AssetDatabase.GetAssetPath(LinkerData.instance);
//				if (string.IsNullOrEmpty(path))
//				{
//					LinkerData.instance = null;
//				}else{
//
//					Selection.activeObject = LinkerData.instance;
//					EditorGUIUtility.PingObject(Selection.activeObject);
//					Debug.Log("Linker Wizard already exists at "+path);
//					return;
//				}
//			}
//
//			// search in the assets:
//		 	UnityEngine.Object[] _assets =	PlayMakerUtils.GetAssetsOfType(typeof(LinkerData),".asset");
//
//			if (_assets!=null && _assets.Length>0)
//			{
//				LinkerData.instance = _assets[0] as LinkerData;
//
//				Selection.activeObject = LinkerData.instance;
//				EditorGUIUtility.PingObject(Selection.activeObject);
//				Debug.Log("Linker Wizard already exists at "+AssetDatabase.GetAssetPath(LinkerData.instance));
//				return;
//			}
//
//			PlayMakerUtils.CreateAsset<LinkerData>("Linker Wizard");
//		}
//
//	#endif

	}
}
