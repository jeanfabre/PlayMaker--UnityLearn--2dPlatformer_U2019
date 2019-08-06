// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;


namespace HutongGames.PlayMaker.Ecosystem.Utils
{

	[CustomPropertyDrawer (typeof(PlayMakerFsmVariableTarget))]
	public class PlayMakerFsmVariableTargetDrawer : PlayMakerPropertyDrawerBaseClass 
	{

		/// <summary>
		/// The row count. Computed and set by inheriting class
		/// </summary>
		int rowCount;

		SerializedProperty fsmVariableTarget;
		SerializedProperty gameObject;
		SerializedProperty fsmName;
		SerializedProperty fsmComponent;

		PlayMakerFSM[] fsmList;

		PlayMakerFsmVariableTarget _target;

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{


			fsmVariableTarget = property.FindPropertyRelative("variableTarget");

			rowCount = 2;
			if (fsmVariableTarget.enumValueIndex==1) // targeting GameObject:
			{
				rowCount = 3;
			}else if (fsmVariableTarget.enumValueIndex == 2 ) // targeting globalvariables
			{
				rowCount = 1;
			}

			return base.GetPropertyHeight(property,label) * rowCount;
		}



		public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label)
		{
			_target = PlayMakerInspectorUtils.GetBaseProperty<PlayMakerFsmVariableTarget>(prop);

			bool isDirty = false;

			fsmVariableTarget = prop.FindPropertyRelative("variableTarget");
			int _variableTargetIndex = fsmVariableTarget.enumValueIndex;

			gameObject = prop.FindPropertyRelative("gameObject");
			fsmName = prop.FindPropertyRelative("fsmName");
			fsmComponent = prop.FindPropertyRelative("_fsmComponent");

			CacheOwnerGameObject(prop.serializedObject);

			int row = 0;

			// draw the enum popup Field
			int oldEnumIndex = fsmVariableTarget.enumValueIndex;

			// force the GameObject value to be the owner when switching to it
			// this is just to fall back nicely on a preset that is the expected one, as opposed to target nothing
			if (oldEnumIndex==0 && gameObject.objectReferenceValue!=ownerGameObject)
			{
				gameObject.objectReferenceValue = ownerGameObject;
				fsmName.stringValue = "";
				isDirty = true;
			}

			EditorGUI.PropertyField(
				GetRectforRow(pos,++row -1),
				fsmVariableTarget,label,true);

			if (_variableTargetIndex != fsmVariableTarget.enumValueIndex)
			{
				prop.serializedObject.ApplyModifiedProperties();
				prop.serializedObject.Update();
				_target.Initialize(true);

				//Debug.Log("Initialized isTargetAvailable?"+_target.isTargetAvailable);
				return;
			}

			// force the GameObject value to be the owner when switching to it
			// this is just to fall back nicely on a preset that is the expected one, as opposed to target nothing
			if (oldEnumIndex==0 && gameObject.objectReferenceValue!=ownerGameObject)
			{
				gameObject.objectReferenceValue = ownerGameObject;
				fsmName.stringValue = "";
				isDirty = true;
			}

			// Additional fields
			if (fsmVariableTarget.enumValueIndex==0 || fsmVariableTarget.enumValueIndex==1) // targeting Owner or GameObject:
			{

				if (string.IsNullOrEmpty(fsmName.stringValue) || !updateFsmList())
				{
					GameObject _go = (GameObject)gameObject.objectReferenceValue;
					if (_go!=null)
					{
						PlayMakerFSM _fsm = _go.GetComponent<PlayMakerFSM>();
						if (_fsm!=null)
						{
							fsmName.stringValue = _fsm.FsmName;
							isDirty = true;
						}
					}
				}

				EditorGUI.indentLevel++;

				if (fsmVariableTarget.enumValueIndex==1)
				{
					int _goId =  gameObject.objectReferenceInstanceIDValue;
					EditorGUI.PropertyField(
						GetRectforRow(pos,++row -1),
						gameObject,new GUIContent("Game Object"),true);

					if (_goId!=gameObject.objectReferenceInstanceIDValue)
					{
						isDirty=true;
					}
				}

				Rect _rect= GetRectforRow(pos,++row -1);


				string _name = fsmName.stringValue;
				Rect _fieldRect = new Rect(_rect);
				_fieldRect.width -= 18;
				EditorGUI.PropertyField(
					_fieldRect,
					fsmName,new GUIContent("Fsm Name"),true);

				if (_name!= fsmName.stringValue)
				{
					isDirty = true;
				}

				_rect.xMin = _rect.xMax -16;
			
				// Fsm Name Popup 
				if (GUI.Button(
					_rect,
					"", 
					EditorStyles.popup)
				    )
				{	
					updateFsmList();

					GenericMenu menu = GenerateFsmMenu(fsmList,fsmName.stringValue);
					menu.DropDown(_fieldRect);

					// move the focus out to reflect the change of the menu selection
					GUI.SetNextControlName("noFocus");
					GUI.FocusControl("noFocus");
				}


				EditorGUI.indentLevel--;
			}else if (fsmVariableTarget.enumValueIndex==3) { // targeting  FsmComponent;

				EditorGUI.indentLevel++;

				int _objId = fsmComponent.objectReferenceInstanceIDValue;
				EditorGUI.PropertyField(
					GetRectforRow(pos,++row -1),
					fsmComponent,new GUIContent("Fsm Component"),true);

				if (_objId!=fsmComponent.objectReferenceInstanceIDValue)
				{
					Debug.Log("yo "+_objId+" "+fsmComponent.objectReferenceInstanceIDValue);
					isDirty = true;
				}

				EditorGUI.indentLevel--;
			}

			// attempt to refresh UI and avoid glitch
			if (row!=rowCount || isDirty)
			{
				prop.serializedObject.ApplyModifiedProperties();
				prop.serializedObject.Update();

				_target.Initialize(true);
			}

		

			// update the rowCount to compute the right interface height
			rowCount = row;
		}


		bool updateFsmList()
		{
			GameObject _target = (GameObject)gameObject.objectReferenceValue;
			
			fsmList = _target.GetComponents<PlayMakerFSM>();

			return fsmList.Length!=0;
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

			_target.Initialize(true);

			//GUIUtility.hotControl = 0;
			GUI.FocusControl("noFocus");
			//GUIUtility.keyboardControl = 0 ;
		}

		GenericMenu GenerateFsmMenu(PlayMakerFSM[] _FsmNameList,string currentSelection)
		{
			var menu = new GenericMenu();


			foreach(PlayMakerFSM _fsm in _FsmNameList)
			{
				string _fsmName = _fsm.FsmName;
				menu.AddItem(new GUIContent(_fsmName), currentSelection.Equals(_fsmName), FsmMenuSelectionCallBack,_fsmName);
			}
			
			return menu;
		}

	}
}