// source: http://www.reddit.com/r/Unity3D/comments/2qsor5/heres_a_way_to_call_methods_on_components_without/

using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{

	[CustomPropertyDrawer(typeof(ButtonAttribute))]
	public class ButtonDrawer : PropertyDrawer {
		ButtonAttribute battribute;
		Object obj;
		Rect buttonRect;
		Rect valueRect;
		
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			battribute = attribute as ButtonAttribute;
			obj = property.serializedObject.targetObject;
			MethodInfo method = obj.GetType().GetMethod(battribute.methodName, battribute.flags);
			
			if (method == null) {
				EditorGUI.HelpBox(position, "Method Not Found", MessageType.Error);
				
			} else {
				if (battribute.useValue) {
					valueRect = new Rect(position.x, position.y, position.width/2f, position.height);
					buttonRect = new Rect(position.x + position.width/2f, position.y, position.width/2f, position.height);
					
					EditorGUI.PropertyField(valueRect, property, GUIContent.none);
					if (GUI.Button(buttonRect, battribute.buttonName)) {
						method.Invoke(obj, new object[]{fieldInfo.GetValue(obj)});
					}
					
				} else {
					if (GUI.Button(position, battribute.buttonName)) {
						method.Invoke(obj, null);
					}
				}
			}
		}
	}

}