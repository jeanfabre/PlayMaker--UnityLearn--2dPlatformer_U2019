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
	public enum ProxyFsmTarget {Owner,GameObject,FsmComponent};


	/// <summary>
	/// PlayMaker Fsm Target. Use this class in your Components public interface. The Unity Inspector will use the related PropertyDrawer.
	/// It lets user easily choose a PlayMaker Fsm Component: 
	/// Options are: Owner, GameObject, or FsmComponent
	/// For Owner and GameObject targets, the fsm will pick the first one or the fsm named if defined.
	/// This class works on its own.
	/// </summary>
	[Serializable]
	public class PlayMakerFsmTarget{

		public ProxyFsmTarget target;

		public GameObject gameObject;
		public string fsmName = null;

		[SerializeField]
		PlayMakerFSM _fsmComponent;

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




		public PlayMakerFsmTarget()
		{
		}

		public PlayMakerFsmTarget(ProxyFsmTarget target)
		{
			this.target = target;
		}

		public void Initialize()
		{
			if (!Application.isPlaying) 
			{
				return;
			}

			if (_initialized)
			{
				return;
			}

			//Debug.Log("Initializing "+target);

			_initialized = true;

			if (target == ProxyFsmTarget.FsmComponent)
			{

				return;
			}

			if (gameObject!=null)
			{
				fsmComponent = PlayMakerUtils.FindFsmOnGameObject(gameObject,fsmName);
			}

			if (fsmComponent==null)
			{
				Debug.LogError("Initialized with no FsmComponent found");
			}
			//Debug.Log("Initialized with fsmComponent<"+fsmComponent.FsmName+">");
		}
	}

}