// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using System.Collections;

using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	public class PlayMakerEventProxy : MonoBehaviour {

		public PlayMakerEventTarget eventTarget = new PlayMakerEventTarget(false);
		
		[EventTargetVariable("eventTarget")]
		//[ShowOptions]
		public PlayMakerEvent fsmEvent;

		public bool debug;

		protected void SendPlayMakerEvent()
		{

			if (debug || !Application.isPlaying)
			{
				UnityEngine.Debug.Log("Send "+fsmEvent.ToString()+" on "+eventTarget.ToString(),this);
			}

			if (!Application.isPlaying)
			{
				UnityEngine.Debug.Log("<color=RED>Application must run to send a PlayMaker Event, but the proxy at least works</color>",this);
				return;
			}



			fsmEvent.SendEvent(null,eventTarget);
		}
	}
}