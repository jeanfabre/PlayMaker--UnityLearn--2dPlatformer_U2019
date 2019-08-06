// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;


namespace HutongGames.PlayMaker.Ecosystem.Utils
{

	[CustomPropertyDrawer (typeof(PlayMakerFsmTarget))]
	public class PlayMakerFsmTargetDrawer : PlayMakerPropertyDrawerBaseClass 
	{

		/// <summary>
		/// The row count. Computed and set by inheriting class
		/// </summary>
		int rowCount;

		string fsmNameFromMen;

		SerializedProperty fsmTarget;
		SerializedProperty gameObject;
		SerializedProperty fsmName;
		SerializedProperty fsmComponent;

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			fsmTarget = property.FindPropertyRelative("target");

			rowCount = 2;
			if (fsmTarget.enumValueIndex==1) // targeting GameObject:
			{
				rowCount = 3;
			}

			return base.GetPropertyHeight(property,label) * rowCount;
		}



		public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label)
		{
			fsmTarget = prop.FindPropertyRelative("target");
			gameObject = prop.FindPropertyRelative("gameObject");
			fsmName = prop.FindPropertyRelative("fsmName");
			fsmComponent = prop.FindPropertyRelative("_fsmComponent");

			CacheOwnerGameObject(prop.serializedObject);

			int row = 0;

			// draw the enum popup Field
			int oldEnumIndex = fsmTarget.enumValueIndex;

			// force the GameObject value to be the owner when switching to it
			// this is just to fall back nicely on a preset that is the expected one, as opposed to target nothing
			if (oldEnumIndex==0 && gameObject.objectReferenceValue!=ownerGameObject)
			{
				gameObject.objectReferenceValue = ownerGameObject;
			}

			EditorGUI.PropertyField(
				GetRectforRow(pos,++row -1),
				fsmTarget,label,true);

			// force the GameObject value to be the owner when switching to it
			// this is just to fall back nicely on a preset that is the expected one, as opposed to target nothing
			if (oldEnumIndex==0 && gameObject.objectReferenceValue!=ownerGameObject)
			{
				gameObject.objectReferenceValue = ownerGameObject;
			}

			// Additional fields
			if (fsmTarget.enumValueIndex==0 || fsmTarget.enumValueIndex==1) // targeting Owner or GameObject:
			{
				if (string.IsNullOrEmpty(fsmName.stringValue))
				{
					GameObject _go = (GameObject)gameObject.objectReferenceValue;
					if (_go!=null)
					{
						PlayMakerFSM _fsm = _go.GetComponent<PlayMakerFSM>();
						if (_fsm!=null)
						{
							fsmName.stringValue = _fsm.FsmName;

						}
					}
				}

				EditorGUI.indentLevel++;

				if (fsmTarget.enumValueIndex==1)
				{
					EditorGUI.PropertyField(
						GetRectforRow(pos,++row -1),
						gameObject,new GUIContent("Game Object"),true);
				}


				Rect _rect= GetRectforRow(pos,++row -1);
				Rect _fieldRect = new Rect(_rect);
				_fieldRect.width -= 18;
				EditorGUI.PropertyField(
					_fieldRect,
					fsmName,new GUIContent("Fsm Name"),true);

				// Event Popup 
				_rect.xMin = _rect.xMax -16;
			
				if (GUI.Button(
					_rect,
					"", 
					EditorStyles.popup)
				    )
				{	

					GameObject _target = (GameObject)gameObject.objectReferenceValue;
					
					PlayMakerFSM[] _fsms = _target.GetComponents<PlayMakerFSM>();
					
					GenericMenu menu = GenerateFsmMenu(_fsms,fsmName.stringValue);
					menu.DropDown(_fieldRect);
				
					// move the focus out to reflect the change of the menu selection
					GUI.SetNextControlName("noFocus");
					GUI.FocusControl("noFocus");
				}

				EditorGUI.indentLevel--;
			}else{ // targeting  FsmComponent;

				EditorGUI.indentLevel++;

				EditorGUI.PropertyField(
					GetRectforRow(pos,++row -1),
					fsmComponent,new GUIContent("Fsm Component"),true);

				EditorGUI.indentLevel--;
			}

			// attempt to refresh UI and avoid glitch
			if (row!=rowCount)
			{
				prop.serializedObject.ApplyModifiedProperties();
				prop.serializedObject.Update();
			}

		

			// update the rowCount to compute the right interface height
			rowCount = row;
		}

		void FsmMenuSelectionCallBack(object userdata)
		{
			if (userdata==null) // none
			{
				fsmName.stringValue = "";
			}else{
				fsmName.stringValue = (string)userdata;
			}
			fsmName.serializedObject.ApplyModifiedProperties();
			fsmName.serializedObject.Update();
			//GUIUtility.hotControl = 0;
			GUI.FocusControl("noFocus");
			//GUIUtility.keyboardControl = 0 ;
		}

		GenericMenu GenerateFsmMenu(PlayMakerFSM[] _eventList,string currentSelection)
		{
			var menu = new GenericMenu();

			foreach(PlayMakerFSM _fsm in _eventList)
			{
				string _fsmName = _fsm.FsmName;
				menu.AddItem(new GUIContent(_fsmName), currentSelection.Equals(_fsmName), FsmMenuSelectionCallBack,_fsmName);
			}
			
			return menu;
		}

	}
}