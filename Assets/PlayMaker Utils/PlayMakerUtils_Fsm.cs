// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

using HutongGames.PlayMaker;

public partial class PlayMakerUtils {

	public static PlayMakerFSM FindFsmOnGameObject(GameObject go,string fsmName)
	{
		if (go==null || string.IsNullOrEmpty(fsmName))
		{
			return null;
		}

		foreach(PlayMakerFSM _fsm in go.GetComponents<PlayMakerFSM>())
		{
			if (string.Equals(_fsm.FsmName,fsmName))
			{
				return _fsm;
			}
		}

		return null;
	}
}
