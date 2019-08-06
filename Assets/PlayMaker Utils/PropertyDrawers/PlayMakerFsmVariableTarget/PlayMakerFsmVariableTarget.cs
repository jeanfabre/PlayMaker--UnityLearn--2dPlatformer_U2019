// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;
using UnityEngine;
using System.Text.RegularExpressions;

using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	
	/// <summary>
	/// Options to define an fsm target
	/// </summary>
	public enum ProxyFsmVariableTarget {Owner,GameObject,GlobalVariable,FsmComponent};
	
	
	/// <summary>
	/// PlayMaker Fsm Variable Target. Use this class in your Components public interface. The Unity Inspector will use the related PropertyDrawer.
	/// It lets user easily choose where to look for a FsmVariable: 
	/// Options are: Owner, GameObject, GlobalVariable or FsmComponent
	/// This class works on its own.
	/// </summary>
	[Serializable]
	public class PlayMakerFsmVariableTarget{
		
		public ProxyFsmVariableTarget variableTarget;
		
		public GameObject gameObject;
		public string fsmName = null;

		[SerializeField]
		PlayMakerFSM _fsmComponent;


		public bool isTargetAvailable
		{
			get{
				//Debug.Log("isTargetAvailable _fsmComponent:"+_fsmComponent+" FsmVariables?:"+(FsmVariables != null));
				return FsmVariables != null;
			}
		}

		FsmVariables _fsmVariables;
		public FsmVariables FsmVariables
		{
			get{
				//Debug.Log("Get FsmVariables "+_fsmVariables+" "+_initialized);
				if ( _fsmVariables == null || ! _initialized)
				{
					Initialize();
				}
				return _fsmVariables;
			}
		}

		[NonSerialized]
		bool _initialized = false;
		
		public PlayMakerFSM fsmComponent
		{
			get{
				Initialize();
				return _fsmComponent;
			}
			set{
				_fsmComponent = value;
			}
		}

		public PlayMakerFsmVariableTarget()
		{
		}
		
		public PlayMakerFsmVariableTarget(ProxyFsmVariableTarget target)
		{
			this.variableTarget = target;
		}


		public void Initialize(bool forceRefresh=false)
		{
			//Debug.Log("Initializing "+variableTarget+" _initialized:"+_initialized+" forceRefresh:"+forceRefresh);
			if (_initialized && !forceRefresh)
			{
				return;
			}

			_initialized = true;
			_fsmVariables = null;

			if (variableTarget == ProxyFsmVariableTarget.GlobalVariable)
			{
				_fsmVariables = PlayMaker.FsmVariables.GlobalVariables;
			//	Debug.LogWarning("Setting FsmVariables for "+variableTarget);
			}else{

				if (variableTarget == ProxyFsmVariableTarget.FsmComponent)
				{
					if (_fsmComponent!=null)
					{
						_fsmVariables = _fsmComponent.FsmVariables;
						Debug.LogWarning("Setting FsmVariables for "+variableTarget+" _fsmComponent= "+_fsmComponent);
					}
					return;
				}
				
				if (gameObject!=null)
				{
					fsmComponent = PlayMakerUtils.FindFsmOnGameObject(gameObject,fsmName);
				}
				
				if (fsmComponent==null)
				{
					_fsmVariables = null;
					//Debug.LogError("Initialized with no FsmComponent found");
				}else{
					_fsmVariables = _fsmComponent.FsmVariables;
					//Debug.LogWarning("Setting FsmVariables for "+variableTarget+" _fsmComponent= "+_fsmComponent);
				}
			}

			//Debug.Log("Initialized with fsmComponent<"+fsmComponent.FsmName+">");
		}

		public override string ToString ()
		{
			return string.Format ("[PlayMakerFsmVariableTarget: FsmVariables={0}, fsmComponent={1}]", FsmVariables, fsmComponent);
		}
	}
	
}