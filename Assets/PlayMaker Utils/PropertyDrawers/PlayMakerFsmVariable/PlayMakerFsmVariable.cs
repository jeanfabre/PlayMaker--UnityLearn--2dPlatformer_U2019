// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;
using UnityEngine;
using System.Text.RegularExpressions;

using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{

	public enum VariableSelectionChoice
	{
		Any,
		Float,
		Int,
		Bool,
		GameObject,
		String,
		Vector2,
		Vector3,
		Color,
		Rect,
		Material,
		Texture,
		Quaternion,
		Object
		#if PLAYMAKER_1_8
		,
		Array,
		Enum
		#endif
	}

	/// <summary>
	/// PlayMaker Fsm Variable. Use this class in your Components public interface. The Unity Inspector will use the related PropertyDrawer.
	/// It lets user easily point to a PlayMaker FsmVariable
	/// 
	/// If there is no attribute "FsmVariableTargetVariable" define, the list of events will be all the PlayMaker global events
	/// 
	/// If the attribute "FsmEventTargetVariable" is defined, the PlayMakerFsmVariableTarget variable will be used for the context
	///  the list of events will adapt, and warn the user if the selected event is indeed implemented on the target
	/// </summary>
	[Serializable]
	public class PlayMakerFsmVariable{


		/// <summary>
		/// The type of the variable. Choose any to let the user pick from any type
		/// </summary>
		public VariableSelectionChoice variableSelectionChoice;

		/// <summary>
		/// The type of the selected. Defined after the user has picked a variable from the list
		/// Sorry, I could not find another way to expose this to the propertyDrawer...
		/// </summary>
		public VariableType selectedType = VariableType.Unknown;

		/// <summary>
		/// The name of the event.
		/// </summary>
		public string variableName;

		/// <summary>
		/// The default variable name.
		/// </summary>
		public string defaultVariableName;


		public bool initialized;

		public bool targetUndefined = true;

		string variableNameToUse = "";
		FsmVariables fsmVariables;
		NamedVariable _namedVariable;


		public NamedVariable namedVariable
		{
			get{
				return _namedVariable;
			}
		}

		FsmFloat _float;
		public FsmFloat FsmFloat
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Float)
				    	|| 
				    selectedType != VariableType.Float
				    )
				{
					Debug.LogError("Trying to access a FloatFsm Variable when the variable type is actually "+selectedType);
					return null;
				}

				if (_float==null && fsmVariables!=null && selectedType == VariableType.Float)
				{
					_float = fsmVariables.GetFsmFloat(variableNameToUse);
				}

				return _float;
			}
		}

		FsmInt _int;
		public FsmInt FsmInt
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Int)
				    || 
				    selectedType != VariableType.Int
				    )
				{
					Debug.LogError("Trying to access a FsmInt Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_int==null && fsmVariables!=null && selectedType == VariableType.Int)
				{
					_int = fsmVariables.GetFsmInt(variableNameToUse);
				}
				return _int;
			}
		}

		FsmBool _bool;
		public FsmBool FsmBool
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Bool)
				    || 
				    selectedType != VariableType.Bool
				    )
				{
					Debug.LogError("Trying to access a FsmBool Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_bool==null && fsmVariables!=null && selectedType == VariableType.Bool)
				{
					_bool = fsmVariables.GetFsmBool(variableNameToUse);
				}
				return _bool;
			}
		}

		FsmGameObject _gameObject;
		public FsmGameObject FsmGameObject
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.GameObject)
				    || 
				    selectedType != VariableType.GameObject
				    )
				{
					Debug.LogError("Trying to access a FsmGameObject Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_gameObject==null && fsmVariables!=null && selectedType == VariableType.GameObject)
				{
					_gameObject = fsmVariables.GetFsmGameObject(variableNameToUse);
				}
				return _gameObject;
			}
		}

		FsmColor _color;
		public FsmColor FsmColor
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Color)
				    || 
				    selectedType != VariableType.Color
				    )
				{
					Debug.LogError("Trying to access a FsmColor Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_color==null && fsmVariables!=null && selectedType == VariableType.Color)
				{
					_color = fsmVariables.GetFsmColor(variableNameToUse);
				}
				return _color;
			}
		}

		FsmMaterial _material;
		public FsmMaterial FsmMaterial
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Material)
				    || 
				    selectedType != VariableType.Material
				    )
				{
					Debug.LogError("Trying to access a FsmMaterial Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_material==null && fsmVariables!=null && selectedType == VariableType.Material)
				{
					_material = fsmVariables.GetFsmMaterial(variableNameToUse);
				}
				return _material;
			}
		}

		FsmObject _object;
		public FsmObject FsmObject
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Object)
				    || 
				    selectedType != VariableType.Object
				    )
				{
					Debug.LogError("Trying to access a FsmObject Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_object==null && fsmVariables!=null && selectedType == VariableType.Object)
				{
					_object = fsmVariables.GetFsmObject(variableNameToUse);
				}
				return _object;
			}
		}

		FsmQuaternion _quaternion;
		public FsmQuaternion FsmQuaternion
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Quaternion)
				    || 
				    selectedType != VariableType.Quaternion
				    )
				{
					Debug.LogError("Trying to access a FsmQuaternion Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_quaternion==null && fsmVariables!=null && selectedType == VariableType.Quaternion)
				{
					_quaternion = fsmVariables.GetFsmQuaternion(variableNameToUse);
				}
				return _quaternion;
			}
		}

		FsmRect _rect;
		public FsmRect FsmRect
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Rect)
				    || 
				    selectedType != VariableType.Rect
				    )
				{
					Debug.LogError("Trying to access a FsmRect Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_rect==null && fsmVariables!=null && selectedType == VariableType.Rect)
				{
					_rect = fsmVariables.GetFsmRect(variableNameToUse);
				}
				return _rect;
			}
		}

		FsmString _string;
		public FsmString FsmString
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.String)
				    || 
				    selectedType != VariableType.String
				    )
				{
					Debug.LogError("Trying to access a FsmString Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_string==null && fsmVariables!=null && selectedType == VariableType.String)
				{
					_string = fsmVariables.GetFsmString(variableNameToUse);
				}
				return _string;
			}
		}

		FsmTexture _texture;
		public FsmTexture FsmTexture
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Texture)
				    || 
				    selectedType != VariableType.Texture
				    )
				{
					Debug.LogError("Trying to access a FsmTexture Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_texture==null && fsmVariables!=null && selectedType == VariableType.Texture)
				{
					_texture = fsmVariables.GetFsmTexture(variableNameToUse);
				}
				return _texture;
			}
		}

		FsmVector2 _vector2;
		public FsmVector2 FsmVector2
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Vector2)
				    || 
				    selectedType != VariableType.Vector2
				    )
				{
					Debug.LogError("Trying to access a FsmVector2 Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_vector2==null && fsmVariables!=null && selectedType == VariableType.Vector2)
				{
					_vector2 = fsmVariables.GetFsmVector2(variableNameToUse);
				}
				return _vector2;
			}
		}

		FsmVector3 _vector3;
		public FsmVector3 FsmVector3
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Vector3)
				    || 
				    selectedType != VariableType.Vector3
				    )
				{
					Debug.LogError("Trying to access a FsmVector3 Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_vector3==null && fsmVariables!=null && selectedType == VariableType.Vector3)
				{
					_vector3 = fsmVariables.GetFsmVector3(variableNameToUse);
				}
				return _vector3;
			}
		}

		#if PLAYMAKER_1_8
		FsmArray _fsmArray;
		public FsmArray FsmArray
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Array)
				    || 
				    selectedType != VariableType.Array
				    )
				{
					Debug.LogError("Trying to access a FsmArray Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_fsmArray==null && fsmVariables!=null && selectedType == VariableType.Array)
				{
					_fsmArray = fsmVariables.GetFsmArray(variableNameToUse);
				}
				return _fsmArray;
			}
		}

		FsmEnum _fsmEnum;
		public FsmEnum FsmEnum
		{
			get{
				if ( 
				    (variableSelectionChoice!= VariableSelectionChoice.Any && variableSelectionChoice!= VariableSelectionChoice.Enum)
				    || 
				    selectedType != VariableType.Enum
				    )
				{
					Debug.LogError("Trying to access a FsmEnum Variable when the variable type is actually "+selectedType);
					return null;
				}
				if (_fsmEnum==null && fsmVariables!=null && selectedType == VariableType.Enum)
				{
					_fsmEnum = fsmVariables.GetFsmEnum(variableNameToUse);
				}
				return _fsmEnum;
			}
		}
		#endif


		public PlayMakerFsmVariable(){}

		public PlayMakerFsmVariable(VariableSelectionChoice variableSelectionChoice)
		{
			this.variableSelectionChoice = variableSelectionChoice;
		}

		public PlayMakerFsmVariable(string defaultVariableName)
		{
			this.defaultVariableName = defaultVariableName;
		}

		public PlayMakerFsmVariable(VariableSelectionChoice variableSelectionChoice, string defaultVariableName)
		{
			this.variableSelectionChoice = variableSelectionChoice;

			this.defaultVariableName = defaultVariableName;
		}


		public bool GetVariable(PlayMakerFsmVariableTarget variableTarget)
		{
			initialized = true;
			targetUndefined = true;
			if (variableTarget.FsmVariables !=null)
			{
				targetUndefined = false;
				variableNameToUse = string.IsNullOrEmpty(variableName)?defaultVariableName:variableName;

				fsmVariables = variableTarget.FsmVariables;
				_namedVariable = fsmVariables.GetVariable(variableNameToUse);

				if (_namedVariable !=null)
				{
					#if PLAYMAKER_1_8
					selectedType = _namedVariable.VariableType;
					#else
					selectedType = GetTypeFromChoice(variableSelectionChoice);
					#endif
					return true;
				}
			}

			selectedType = VariableType.Unknown;
			
			return false;
		}


		public override string ToString ()
		{

			string _variableName = "<color=blue>"+variableName+"</color>";

			if (string.IsNullOrEmpty(_variableName))
			{
				_variableName = "<color=red>None</color>";
			}
			return string.Format ("PlayMaker Variable<{0}>: {1} ("+_namedVariable+")", this.variableSelectionChoice, _variableName);
		}

		public static VariableType GetTypeFromChoice(VariableSelectionChoice choice)
		{
		//	Debug.Log("GetTypeFromChoice"+choice);

			if (choice == VariableSelectionChoice.Any) return VariableType.Unknown;
			if (choice == VariableSelectionChoice.Float) return VariableType.Float;
			if (choice == VariableSelectionChoice.Int) return VariableType.Int;
			if (choice == VariableSelectionChoice.Bool) return VariableType.Bool;
			if (choice == VariableSelectionChoice.GameObject) return VariableType.GameObject;
			if (choice == VariableSelectionChoice.String) return VariableType.String;
			if (choice == VariableSelectionChoice.Vector2) return VariableType.Vector2;
			if (choice == VariableSelectionChoice.Vector3) return VariableType.Vector3;
			if (choice == VariableSelectionChoice.Color) return VariableType.Color;
			if (choice == VariableSelectionChoice.Rect) return VariableType.Rect;
			if (choice == VariableSelectionChoice.Material) return VariableType.Material;
			if (choice == VariableSelectionChoice.Texture) return VariableType.Texture;
			if (choice == VariableSelectionChoice.Quaternion) return VariableType.Quaternion;
			if (choice == VariableSelectionChoice.Object) return VariableType.Object;
			#if PLAYMAKER_1_8
			if (choice == VariableSelectionChoice.Array) return VariableType.Array;
			if (choice == VariableSelectionChoice.Enum) return VariableType.Enum;
			#endif
			return VariableType.Unknown;
		}
	}
	
}