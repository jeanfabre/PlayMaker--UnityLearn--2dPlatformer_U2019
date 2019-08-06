using System;
using System.Collections;

using UnityEditor;
using UnityEngine;

namespace  HutongGames.PlayMaker.Ecosystem.utils
{
	[CustomEditor(typeof(Comment))]
	public class CommentInspector : UnityEditor.Editor {

		public override void OnInspectorGUI()
		{

			Comment _target = (Comment)this.target;

		//	bool _wordWrap = GUI.skin.textField.wordWrap;
		//	GUI.skin.textField.wordWrap = true;

			GUIStyle style = new GUIStyle(EditorStyles.textField);
			style.wordWrap = true;
			
			float height = style.CalcHeight(new GUIContent(_target.Text), Screen.width);

			Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(height));
			
			GUI.changed = false;
			string text = EditorGUI.TextArea(rect, _target.Text, style);
			if (GUI.changed)
			{
				_target.Text = text;
			}

		//	GUI.skin.textField.wordWrap = _wordWrap;
		
		}
	}
}