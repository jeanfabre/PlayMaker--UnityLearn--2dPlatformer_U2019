// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using HutongGames.Editor;
using HutongGames.PlayMakerEditor;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	#pragma warning disable 219, 414
	[CustomPropertyDrawer (typeof (PlayMakerFsmVariable))]
	public class PlayMakerFsmVariableDrawer : PlayMakerPropertyDrawerBaseClass 
	{
		
		/// <summary>
		/// Flag to know that we have checked for the attributes
		/// </summary>
		bool attributeScanned;
		
		/// <summary>
		/// Use the attribute "VariableTargetVariable" to point to that variable
		/// </summary>
		SerializedProperty variableTargetVariable;

		// these three property are external, and comes from the attribution of a PlayMakerFsmVariableTarget Variable via
		// the custom Attribute FsmVariableTargetVariable
		// if no VariableTarget is defined, then this Variable Drawer will show Global Variables
		SerializedProperty variableTarget;
		SerializedProperty gameObject;
		SerializedProperty fsmComponent;
		SerializedProperty isTargetAvailable;

		// PlayMakerFsmVariable Component properties
		SerializedProperty variableSelectionChoice;
		SerializedProperty selectedType;
		SerializedProperty variableName;
		SerializedProperty defaultVariableName;

		bool _debug;
		string defaultVariableNameValue;

		PlayMakerFsmVariable _target;
		PlayMakerFsmVariableTarget _variabletargetTarget;

		/// <summary>
		/// The row count. Computed and set by inheriting class
		/// </summary>
		int rowCount;

		bool _noVariablesSelectable;

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			_target = PlayMakerInspectorUtils.GetBaseProperty<PlayMakerFsmVariable>(property);

			// TODO: allow option to force a particular type of variable
			ScanAttributesIfNeeded(property);

			rowCount = _noVariablesSelectable?2:3; // ideal scenario


			// we let the user choose amongst globavariables if not target attribute and when target is defined.
			// else we error out
			if (_variabletargetTarget!=null && !_variabletargetTarget.isTargetAvailable )
			{
				rowCount = 1;
			}

			return base.GetPropertyHeight(property,label) * (rowCount);
		}
		

		void ScanAttributesIfNeeded(SerializedProperty property)
		{
			if (!attributeScanned)
			{
				attributeScanned = true;

			
				// check for FsmVariableTargetVariable Attribute
				object[] _variableTargets = this.fieldInfo.GetCustomAttributes(typeof(FsmVariableTargetVariable),true);
				
				if (_variableTargets.Length>0)
				{
					string variableName = (_variableTargets[0] as FsmVariableTargetVariable).variable;
					variableTargetVariable = property.serializedObject.FindProperty(variableName);

					_variabletargetTarget =  PlayMakerInspectorUtils.GetBaseProperty<PlayMakerFsmVariableTarget>(variableTargetVariable);
				}

				if (variableTargetVariable!=null)
				{
					variableTarget = variableTargetVariable.FindPropertyRelative("variableTarget");
					gameObject = variableTargetVariable.FindPropertyRelative("gameObject");
					fsmComponent = variableTargetVariable.FindPropertyRelative("fsmComponent");
					isTargetAvailable = variableTargetVariable.FindPropertyRelative("isTargetAvailable");
				}

				// check for FsmVariableType Attribute
			//	object[] _variableTypes = this.fieldInfo.GetCustomAttributes(typeof(),true);

				/*
				if (_variableTypes.Length>0)
				{
					VariableType variableType = (_variableTypes[0] as FsmVariableType).variableType;
				}
*/

				
			}
		}


		GUIStyle controlLabelStyle;
		public override void OnGUI (Rect pos, SerializedProperty prop, GUIContent label) {

			int row =0;

			// set style to use rich text.
			if (controlLabelStyle==null)
			{
				controlLabelStyle = GUI.skin.GetStyle("ControlLabel");
				controlLabelStyle.richText = true;
			}

			
			ScanAttributesIfNeeded(prop);

			// we let the user choose amongst globavariables if not target attribute and when target is defined.
			if (variableTargetVariable!=null && isTargetAvailable!=null && ! isTargetAvailable.boolValue )
			{
				EditorGUI.LabelField(
					GetRectforRow(pos,++row -1),
					" ",
					"<color=red>Target is undefined</color>"
					);

				return;
			}


			variableSelectionChoice = prop.FindPropertyRelative("variableSelectionChoice");
			int _variableSelectionChoiceIndex = variableSelectionChoice.enumValueIndex;

			selectedType = prop.FindPropertyRelative("selectedType");
			VariableType _selectedType = PlayMakerInspectorUtils.GetVariableTypeFromEnumIndex(selectedType.enumValueIndex);
			//	PlayMakerInspectorUtils.GetVariableTypeFromEnumIndex(variableSelectionChoice.enumValueIndex);


			//VariableType _variableType = (VariableType)Enum.GetValues(typeof(VariableType)).GetValue(variableType.enumValueIndex);
			//Debug.Log(variableSelectionChoice.enumValueIndex+" "+_variableSelectionChoice);

			variableName = prop.FindPropertyRelative("variableName");
			string _variableName = variableName.stringValue;
			
			defaultVariableName = prop.FindPropertyRelative("defaultVariableName");
			defaultVariableNameValue = defaultVariableName.stringValue;
			
			CacheOwnerGameObject(prop.serializedObject);
			
		
			bool _isTargetAvailable = _variabletargetTarget.isTargetAvailable;
			//Debug.Log("GetVariablesStringList _isTargetAvailable?"+_isTargetAvailable);
			
			if (_isTargetAvailable)
			{
				// variable type selection
				Rect _typeRect= GetRectforRow(pos,++row -1);

				EditorGUI.PropertyField(
					_typeRect,
					variableSelectionChoice,new GUIContent("Variable Type"),true);

				VariableSelectionChoice _variableSelectionChoice = VariableSelectionChoice.Any;

				if (variableSelectionChoice.enumValueIndex!= _variableSelectionChoiceIndex)
				{
					_variableSelectionChoice = 
						(VariableSelectionChoice)Enum
							.GetValues(typeof(VariableSelectionChoice))
							.GetValue(variableSelectionChoice.enumValueIndex);

				
					_variableSelectionChoiceIndex = variableSelectionChoice.enumValueIndex;
					//Debug.Log ("Hello _variableSelectionChoice"+_variableSelectionChoice+" _variableSelectionChoiceIndex"+_variableSelectionChoiceIndex+" _selectedType"+_selectedType+" variableType"+PlayMakerFsmVariable.GetTypeFromChoice(_variableSelectionChoice));
					if (_variableSelectionChoiceIndex>0 && _selectedType!= PlayMakerFsmVariable.GetTypeFromChoice(_variableSelectionChoice))
					{
					//	Debug.Log("reset variable name");
						_variableName = "";
						variableName.stringValue = _variableName;
					}

				}else{
					_variableSelectionChoice = 
						(VariableSelectionChoice)Enum
							.GetValues(typeof(VariableSelectionChoice))
							.GetValue(variableSelectionChoice.enumValueIndex);
				}

			
				string[] _variableList = new string[0];
				
				bool isVariableImplemented = false; // not in use, TODO: clean up


				// Get the list of events
				_variableList = PlayMakerInspectorUtils.GetVariablesStringList
					(
						_variabletargetTarget.FsmVariables,
						true,
						PlayMakerFsmVariable.GetTypeFromChoice(_variableSelectionChoice)
						);


				_noVariablesSelectable = _variableList.Length<=1;

				// find the index of the serialized event name in the list of events
				int selected = 0;
				if (! string.IsNullOrEmpty(_variableName))
				{
					//Debug.Log("?!");
					selected = ArrayUtility.IndexOf<string>(_variableList,_variableName);
				}

				// set to none if not found
				if (selected ==-1)
				{
					_variableName = "";
					variableName.stringValue = _variableName;
					selected = 0;
				}



				if (! _noVariablesSelectable)
				{
					Rect _rect= GetRectforRow(pos,++row -1);
					
					string _popupLabel = label.text;
					
					if(selected!=0 && variableTarget!=null && variableTarget.enumValueIndex!=2) // not none and not globalVariables
					{
						if ((selected>0 && !isVariableImplemented )|| selected ==-1)
						{	
							_popupLabel = "<color=red>"+_popupLabel+"</color>";
						}
					}


					// Variable Popup 
					Rect _contentRect = EditorGUI.PrefixLabel(_rect,label);
					//_contentRect.width -= 0;
					if (GUI.Button(
						_contentRect,
						string.IsNullOrEmpty(_variableName)?"none":_variableName, 
						EditorStyles.popup))
					{
						GenericMenu menu = GenerateVariableMenu(_variableList,_variableName);
						menu.DropDown(_rect);
						
					}
				}
				
				if (_target.GetVariable(_variabletargetTarget))
				{
					EditorGUI.LabelField(
						GetRectforRow(pos,++row -1),
						" ",
						_target.namedVariable.ToString()
						);
						
				}else{

					string _feedbackMessage = _noVariablesSelectable?"No "+_variableSelectionChoice+"(s) variable in target":"Please Select a variable";
					string _feedbackLabel = _noVariablesSelectable?"":" ";
					EditorGUI.LabelField(
						GetRectforRow(pos,++row -1),
						_feedbackLabel,
						"<color=red>"+_feedbackMessage+"</color>"
						);
						
				}
			}else{
				EditorGUI.LabelField(
					GetRectforRow(pos,++row -1),
					" ",
					"<color=red>Select a valid target</color>"
					);
			}
			// attempt to refresh UI and avoid glitch
			if (row!=rowCount)
			{
				prop.serializedObject.ApplyModifiedProperties();
				prop.serializedObject.Update();
			}

			rowCount = row;
		}
		
		void ResetToDefault()
		{
			variableName.stringValue = defaultVariableNameValue;
			variableName.serializedObject.ApplyModifiedProperties();
		}
		
		void ShowImplementedEvents()
		{
			
		}
		
		void ShowAllEvents()
		{
			
		}
		
		void VariableMenuSelectionCallBack(object userdata)
		{
			
			if (userdata==null) // none
			{
				variableName.stringValue = "";
			}else{
				variableName.stringValue = (string)userdata;
			}
			
			variableName.serializedObject.ApplyModifiedProperties();
			
		}
		
		GenericMenu GenerateVariableMenu(string[] _variableList,string currentSelection)
		{
			var menu = new GenericMenu();
			menu.AddItem(new GUIContent("none"), currentSelection.Equals("none"), VariableMenuSelectionCallBack, null);
			
			foreach(string _name in _variableList)
			{
				menu.AddItem(new GUIContent(_name), currentSelection.Equals(_name), VariableMenuSelectionCallBack,_name);
			}
			
			return menu;
		}
	}
}