using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HutongGames.PlayMakerEditor.Ecosystem.Utils
{

	public static class EventProxyFileFinder
	{

		public static Dictionary<string,EventProxyFileDetails> FindFiles()
		{
			
			//Lookup enums in file names
			Dictionary<string,EventProxyFileDetails> detailsList = new Dictionary<string,EventProxyFileDetails>();
			
			classFiles = new List<string>();
			FindAllCSharpScriptFiles(Application.dataPath);
			
			//Lookup class name in the class file text 
			for (int i = 0; i < classFiles.Count; i++)
			{
				string filePath = classFiles[i];
				string codeFile = File.ReadAllText(filePath);

				if (codeFile.Contains("__"+"PLAYMAKER_EVENT_PROXY__")) // compose the tag to avoid this file to be found...
				{
					FileInfo _info = new FileInfo(filePath);

					string fileName = _info.Name;

					string className = "";
					Match _classNameMatch = Regex.Match(codeFile,@"(?:public(?:\s+)class(?:\s+))(\w+)(?:\s+)(?::)");
					if (_classNameMatch.Success)
					{
						className = _classNameMatch.Groups[1].Value;
					}

					string nameSpace = "";
					Match _nameSpaceMatch = Regex.Match(codeFile,@"namespace (\w.+)");
					if (_nameSpaceMatch.Success)
					{
						nameSpace = _nameSpaceMatch.Groups[1].Value;
					}

					string methodName = "";
					Match methodNameMatch = Regex.Match(codeFile,@"public(?:\s+)void(?:\s+)(\w+)");
					if (methodNameMatch.Success)
					{
						methodName = methodNameMatch.Groups[1].Value;
					}


					EventProxyFileDetails _details = new EventProxyFileDetails(
						className, 
						nameSpace,
						methodName,
						fileName,
						filePath, 
						_info.LastWriteTimeUtc
						);

					detailsList.Add (filePath,_details);

				}

			}

			return detailsList;
		}

		static List<string> classFiles;
		static void FindAllCSharpScriptFiles(string startDir)
		{

			try
			{
				foreach (string file in Directory.GetFiles(startDir))
				{
					if (file.Contains(".cs"))
						classFiles.Add(file);
				}
				foreach (string dir in Directory.GetDirectories(startDir))
				{
					FindAllCSharpScriptFiles(dir);
				}
			}
			catch (System.Exception ex)
			{
				Debug.Log(ex.Message);
			}
		}
			
	}

	public class EventProxyFileDetails
	{
		string _className;

		/// <summary>
		/// Gets the name of the class.
		/// </summary>
		/// <value>The name of the class.</value>
		public string className 
		{ 
			get
			{
				return _className;
			}
		}

		string _nameSpace;
		/// <summary>
		/// Gets or sets the namespace.
		/// </summary>
		/// <value>The name space.</value>
		public string nameSpace
		{
			get
			{
			return _nameSpace;
			}
		}

		string _methodName;
		/// <summary>
		/// Gets or sets the method Name.
		/// </summary>
		/// <value>The method Name</value>
		public string methodName
		{
			get
			{
				return _methodName;
			}
		}

		string _fileName;
		
		/// <summary>
		/// Gets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		public string fileName 
		{ 
			get
			{
				return _fileName;
			}
		}

		string _filePath;
		/// <summary>
		/// Gets the filePath.
		/// </summary>
		/// <value>The filepath of the enum.</value>
		public string filePath
		{
			get
			{
				return _filePath;
			}
		}

		string _projectPath;
		/// <summary>
		/// Gets the relative path from the project's assets folder.
		/// </summary>
		/// <value>The relative path of the enum.</value>
		public string projectPath
		{
			get
			{
				return _projectPath;
			}
		}

		System.DateTime _updateTime;
		/// <summary>
		/// Gets the update time.
		/// </summary>
		/// <value>The update time.</value>
		public System.DateTime updateTime
		{
			get
			{
				return _updateTime;
			}
		}

		public override string ToString ()
		{
			return string.Format ("PlayMaker Event proxy File Details:\n" +
			                      "<b>NameSpace:</b> {0}\n" +
			                      "<b>className:</b> {1}\n" +
			                      "<b>method:</b> {2}\n" +
			                      "<b>projectPath:</b> {3}\n" +
			                      "<b>updateTime:</b> {4}", nameSpace,className,methodName,projectPath, updateTime);
		}
		
		internal EventProxyFileDetails() {}
		internal EventProxyFileDetails(string className,string nameSpace,string methodName,string fileName, string filePath, System.DateTime updateTime)
		{
			_className = className;
			_nameSpace = nameSpace;
			_methodName = methodName;
			_fileName = fileName;
			_filePath = filePath;
			_updateTime = updateTime;

			_projectPath =  filePath.Substring(Application.dataPath.Length+1);

		}


	}
}
