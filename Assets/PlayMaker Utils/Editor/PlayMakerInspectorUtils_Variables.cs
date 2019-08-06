// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	public partial class PlayMakerInspectorUtils {

		/// <summary>
		/// Display an _selectionIndex the fsm variable from a list of variables ( from an fsm likely).
		/// This is to paliate for the PlayMaker 1.8 that deprecated the api call VariableEditor.FsmVarPopup()
		/// </summary>
		/// <returns>The fsm variable GU.</returns>
		/// <param name="fieldLabel">Field label.</param>
		/// <param name="fsmVariables">Fsm variables.</param>
		/// <param name="selection">Selection.</param>
		/// <param name="GuiChanged">GUI changed flag</param>
		public static FsmVar EditorGUILayout_FsmVarPopup(string fieldLabel,NamedVariable[] namedVariables,FsmVar selection,out bool GuiChanged)
		{
			GuiChanged = false;

			if (namedVariables==null)
			{
				Debug.LogWarning("EditorGUILayout_FsmVarPopup: namedVariables is null");
				return null;
			}



			int _selectionIndex = 0;

			string[] _variableChoices = new string[namedVariables.Length+1];
			_variableChoices[0] = "None";
			for(int i=0;i<namedVariables.Length;i++)
			{
				if (string.Equals(selection.variableName,namedVariables[i].Name))
				{
					_selectionIndex = i+1;
				}
				_variableChoices[i+1] = namedVariables[i].Name;
			}
			
			if (_variableChoices.Length!=0)
			{

				int _choiceIndex =  EditorGUILayout.Popup(fieldLabel,_selectionIndex,_variableChoices);
				if (_choiceIndex != _selectionIndex)
				{
					GuiChanged = true;

					if (_choiceIndex==0)
					{
						return new FsmVar();
					}else{
						FsmVar _newSelection = new FsmVar(namedVariables[_choiceIndex-1]);
						_newSelection.useVariable = true;
						return _newSelection;
					}

				}
			}

			return selection;
		}

		
		/// <summary>
		/// Gets specific types variables list as strings from FsmVariables. 
		/// Use this method to populate popup selection in inspectors.
		/// </summary>
		/// <returns>The list of variables of defined variableType as strings.</returns>
		/// <param name="fsmVariables">the List of Variables to filter</param>
		/// <param name="includeNone">If set to <c>true</c> include a "none" option. Useful for popup to select an event or not</param>
		/// <param name="variableType">Optional, default to Unknown, else will filter by specific type</param>
		public static string[] GetVariablesStringList(FsmVariables fsmVariables,bool includeNone = false, VariableType variableType = VariableType.Unknown)
		{

			string[] list = new string[0];

			if (fsmVariables!=null)
			{
				//Debug.Log("GetVariablesStringList internal "+variableType+" "+fsmVariables.GetNamedVariables(variableType).Length);

				if (variableType == VariableType.Unknown)
				{
					NamedVariable[] namedVariables = fsmVariables.GetAllNamedVariables();// fsmVariables.GetNamedVariables(variableType);// PlayMakerGlobals.Instance.Variables.GetAllNamedVariables();
					
					list = new string[namedVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in namedVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}
				
				if (variableType == VariableType.Bool)
				{
					list = new string[fsmVariables.BoolVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.BoolVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.Color)
				{
					list = new string[fsmVariables.ColorVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.ColorVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.Float)
				{
					list = new string[fsmVariables.FloatVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.FloatVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.GameObject)
				{
					list = new string[fsmVariables.GameObjectVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.GameObjectVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.Int)
				{
					list = new string[fsmVariables.IntVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.IntVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.Material)
				{
					list = new string[fsmVariables.MaterialVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.MaterialVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.Object)
				{
					list = new string[fsmVariables.ObjectVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.ObjectVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.Quaternion)
				{
					list = new string[fsmVariables.QuaternionVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.QuaternionVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.Rect)
				{
					list = new string[fsmVariables.RectVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.RectVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.String)
				{
					list = new string[fsmVariables.StringVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.StringVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.Texture)
				{
					list = new string[fsmVariables.TextureVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.TextureVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.Vector2)
				{
					list = new string[fsmVariables.Vector2Variables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.Vector2Variables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.Vector3)
				{
					list = new string[fsmVariables.Vector3Variables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.Vector3Variables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				#if PLAYMAKER_1_8
				if (variableType == VariableType.Array)
				{
					list = new string[fsmVariables.ArrayVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.ArrayVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}

				if (variableType == VariableType.Enum)
				{
					list = new string[fsmVariables.EnumVariables.Length];
					
					int i=0;
					foreach(NamedVariable _variable in fsmVariables.EnumVariables)
					{
						list[i] = _variable.Name;
						i++;
					}
				}
				#endif
			}

			if (includeNone)
			{
				ArrayUtility.Insert<string>(ref list,0,"none");
			}

			return list;
		}

		/// <summary>
		/// When using SerializedProperty, enums comes with an index, and I haven't yet figured out how to convert it back into the enum reliably
		/// so I made this lut. 
		/// Used in the PlayMakerFsmVariablePropertyDrawer.cs for example.
		/// </summary>
		/// <returns>The variable type from enum index.</returns>
		/// <param name="index">Index.</param>
		public static VariableType GetVariableTypeFromEnumIndex(int index)
		{
			if (index== 0) return VariableType.Unknown;
			if (index== 1) return VariableType.Float;
			if (index== 2) return VariableType.Int;
			if (index== 3) return VariableType.Bool;
			if (index== 4) return VariableType.GameObject;
			if (index== 5) return VariableType.String;
			if (index== 6) return VariableType.Vector2;
			if (index== 7) return VariableType.Vector3;
			if (index== 8) return VariableType.Color;
			if (index== 9) return VariableType.Rect;
			if (index== 10) return VariableType.Material;
			if (index== 11) return VariableType.Texture;
			if (index== 12) return VariableType.Quaternion;
			if (index== 13) return VariableType.Object;
			#if PLAYMAKER_1_8
			if (index== 14) return VariableType.Array;
			if (index== 15) return VariableType.Enum;
			#endif
				
			return VariableType.Unknown;
		}

	}
}