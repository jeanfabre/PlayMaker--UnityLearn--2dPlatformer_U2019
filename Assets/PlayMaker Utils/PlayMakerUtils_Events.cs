// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;

using HutongGames.PlayMaker;

public partial class PlayMakerUtils {


	public static void CreateIfNeededGlobalEvent(string globalEventName)
	{
		if (!FsmEvent.IsEventGlobal(globalEventName))
		{
			// Setup global events
			FsmEvent _event = new FsmEvent(globalEventName);
			_event.IsGlobal = true;
			FsmEvent.AddFsmEvent(_event);
		}
	}

	public static void SendEventToGameObject(PlayMakerFSM fromFsm,GameObject target,string fsmEvent,bool includeChildren)
	{
		SendEventToGameObject(fromFsm,target,fsmEvent,includeChildren,null);
	}
	
	public static void SendEventToGameObject(PlayMakerFSM fromFsm,GameObject target,string fsmEvent)
	{
		SendEventToGameObject(fromFsm,target,fsmEvent,false,null);
	}
	
	public static void SendEventToGameObject(PlayMakerFSM fromFsm,GameObject target,string fsmEvent,FsmEventData eventData)
	{
		SendEventToGameObject(fromFsm,target,fsmEvent,false,eventData);
	}
	
	public static void SendEventToGameObject(PlayMakerFSM fromFsm,GameObject target,string fsmEvent,bool includeChildren,FsmEventData eventData)
	{
		if (eventData!=null)
		{
			HutongGames.PlayMaker.Fsm.EventData = eventData;
		}
		
		if (fromFsm == null)
		{
			return;
		}
		
		FsmEventTarget _eventTarget = new FsmEventTarget();
		_eventTarget.excludeSelf = false;
		_eventTarget.sendToChildren = includeChildren;

		_eventTarget.target = FsmEventTarget.EventTarget.GameObject;	

		FsmOwnerDefault owner = new FsmOwnerDefault();
		owner.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
		owner.GameObject = new FsmGameObject();
		owner.GameObject.Value = target;

		_eventTarget.gameObject = owner;

		fromFsm.Fsm.Event(_eventTarget,fsmEvent);

	}

	
	public static bool DoesTargetImplementsEvent(FsmEventTarget target,string eventName)
	{
		
		if (target.target == FsmEventTarget.EventTarget.BroadcastAll)
		{
			return FsmEvent.IsEventGlobal(eventName);
		}
		
		if (target.target == FsmEventTarget.EventTarget.FSMComponent)
		{
			return DoesFsmImplementsEvent(target.fsmComponent,eventName);
		}
		
		if (target.target == FsmEventTarget.EventTarget.GameObject)
		{
			return DoesGameObjectImplementsEvent(target.gameObject.GameObject.Value,eventName);
		}
		
		if (target.target == FsmEventTarget.EventTarget.GameObjectFSM)
		{
			return DoesGameObjectImplementsEvent(target.gameObject.GameObject.Value,target.fsmName.Value, eventName);
		}
		
		if (target.target == FsmEventTarget.EventTarget.Self)
		{
			Debug.LogError("Self target not supported yet");
		}
		
		if (target.target == FsmEventTarget.EventTarget.SubFSMs)
		{
			Debug.LogError("subFsms target not supported yet");
		}
		
		if (target.target == FsmEventTarget.EventTarget.HostFSM)
		{
			Debug.LogError("HostFSM target not supported yet");
		}
		
		return false;
	}
	
	public static bool DoesGameObjectImplementsEvent(GameObject go, string fsmEvent)
	{
		if (go==null || string.IsNullOrEmpty(fsmEvent))
		{
			return false;
		}
		
		foreach(PlayMakerFSM _fsm in go.GetComponents<PlayMakerFSM>())
		{
			if (DoesFsmImplementsEvent(_fsm,fsmEvent))
			{
				return true;
			}
		}
		return false;
	}
	
	public static bool DoesGameObjectImplementsEvent(GameObject go,string fsmName, string fsmEvent)
	{
		if (go==null || string.IsNullOrEmpty(fsmEvent))
		{
			return false;
		}
		
		bool checkFsmName = !string.IsNullOrEmpty(fsmName);
		
		foreach(PlayMakerFSM _fsm in go.GetComponents<PlayMakerFSM>())
		{
			if ( checkFsmName &&  string.Equals(_fsm,fsmName) )
			{
				if (DoesFsmImplementsEvent(_fsm,fsmEvent))
				{
					return true;
				}
			}
		}
		return false;
	}
	
	public static bool DoesFsmImplementsEvent(PlayMakerFSM fsm, string fsmEvent)
	{
		
		if (fsm==null || string.IsNullOrEmpty(fsmEvent))
		{
			return false;
		}
		
		foreach(FsmTransition _transition in fsm.FsmGlobalTransitions)
		{
			if (_transition.EventName.Equals(fsmEvent))
			{
				return true;
			}
		}
		
		foreach(FsmState _state in fsm.FsmStates)
		{
			
			foreach(FsmTransition _transition in _state.Transitions)
			{
				
				if (_transition.EventName.Equals(fsmEvent))
				{
					return true;
				}
			}
		}
		
		return false;
	}

	public FsmEvent CreateGlobalEvent(string EventName)
	{
		bool _existsAlready;
		return CreateGlobalEvent(EventName,out _existsAlready);
	}

	/// <summary>
	/// Creates the global event if needed.
	/// </summary>
	/// <returns><c>true</c>, if global event was created <c>false</c> if event existed already.</returns>
	/// <param name="EventName">Event name.</param>
	public FsmEvent CreateGlobalEvent(string EventName,out bool ExistsAlready)
	{
		FsmEvent _event = FsmEvent.GetFsmEvent(EventName);
		ExistsAlready = FsmEvent.EventListContains(EventName);

		if (ExistsAlready)
		{
			if (_event!=null && _event.IsGlobal)
			{
				_event.IsGlobal = true;
			}

			return _event;
		}

		_event = new FsmEvent(EventName);
		_event.IsGlobal = true;
		FsmEvent.AddFsmEvent(_event);

		return _event;
	}

	/*
	public bool DoesTargetMissEventImplementation(PlayMakerFSM fsm, string fsmEvent)
	{
		if (DoesTargetImplementsEvent(fsm,fsmEvent))
		{
			return false;
		}
		
		foreach(FsmEvent _event in fsm.FsmEvents)
		{
			if (_event.Name.Equals(fsmEvent))
			{
				return true;
			}
		}
		
		return false;
	}*/

}
