// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

// source: whydoidoit : http://answers.unity3d.com/questions/425012/get-the-instance-the-serializedproperty-belongs-to.html

using System.Linq;
using System;
using System.Collections;
using System.Reflection;

using UnityEditor;
using UnityEngine;

#pragma warning disable 168
namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	public partial class PlayMakerInspectorUtils {

		/// <summary>
		/// Gets the base property. Very Convenient when you want to get the target of a propertyDrawer:
		/// 
		/// ColorPaletteReference _target = SerializedPropertyUtils.GetBaseProperty<ColorPaletteReference>(prop);
		/// 
		/// </summary>
		/// <returns>The base property.</returns>
		/// <param name="prop">Property passed by the propertydrawer or else</param>
		/// <typeparam name="T">The expected type of the base property</typeparam>
		public static T GetBaseProperty<T>(SerializedProperty prop)
		{
			// Separate the steps it takes to get to this property
			string[] separatedPaths = prop.propertyPath.Split('.');
			
			// Go down to the root of this serialized property
			System.Object reflectionTarget = prop.serializedObject.targetObject as object;
			// Walk down the path to get the target object
			foreach (var path in separatedPaths)
			{
				FieldInfo fieldInfo = reflectionTarget.GetType().GetField(path);
				reflectionTarget = fieldInfo.GetValue(reflectionTarget);
			}
			return (T) reflectionTarget;
		}

		public static GameObject GetGameObject(SerializedObject serializedObject)
		{
			Component _comp = GetComponent<Component>(serializedObject);
			if (_comp!=null)
			{
				return _comp.gameObject;
			}
			return null;
		}

		public static T GetComponent<T>(SerializedObject serializedObject) 
		{
			if (serializedObject==null)
			{
				return default(T);
			}

			var iterator = serializedObject.GetIterator();
			while(iterator.NextVisible(true))
			{
				try{

					T parent = (T)GetParent(iterator);
					if(parent!=null)
					{
						return parent;
					}
				}catch(Exception e)
				{

				}

			}

			return default(T);
		}



		static object GetParent(SerializedProperty prop)
		{
			var path = prop.propertyPath.Replace(".Array.data[", "[");
			object obj = prop.serializedObject.targetObject;
			var elements = path.Split('.');
			foreach(var element in path.Split('.').Take(elements.Length-1))
			{
				if(element.Contains("["))
				{
					var elementName = element.Substring(0, element.IndexOf("["));
					var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[","").Replace("]",""));
					obj = GetValue(obj, elementName, index);
				}
				else
				{
					obj = GetValue(obj, element);
				}
			}
			return obj;
		}
		
		static object GetValue(object source, string name)
		{
			if(source == null)
				return null;
			var type = source.GetType();
			var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			if(f == null)
			{
				var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if(p == null)
					return null;
				return p.GetValue(source, null);
			}
			return f.GetValue(source);
		}
		
		static object GetValue(object source, string name, int index)
		{
			var enumerable = GetValue(source, name) as IEnumerable;
			var enm = enumerable.GetEnumerator();
			while(index-- >= 0)
				enm.MoveNext();
			return enm.Current;
		}

	}
}