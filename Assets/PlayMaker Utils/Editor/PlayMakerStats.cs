
using UnityEditor;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;

using System.Collections.Generic;

public class PlayMakerStats : EditorWindow {

    
	List<Fsm> fsms = new List<Fsm>();
	
	int fsmCount = 0;
	int stateCount = 0;
	int variableCount = 0;
	int eventCount = 0;
	int globalEventsCount = 0;
	int globalVariablesCount = 0;
	
	
	
	public Vector2 scrollPosition;

    [MenuItem ("PlayMaker/Addons/Editor Windows/Stats")]
    static void Init () {
        // Get existing open window or if none, make a new one:
        PlayMakerStats window = (PlayMakerStats)EditorWindow.GetWindow (typeof (PlayMakerStats));

		window.parseStats();
    }
	
	void parseStats()
	{
		fsmCount = 0;
		stateCount = 0;
		variableCount = 0;
		eventCount = 0;

		//Fsm.SortedFsmList;
		FsmEditor.RebuildFsmList();
		fsms = FsmEditor.FsmList;
		fsmCount = fsms.Count;

		globalEventsCount = FsmEvent.globalEvents.Count;
		globalVariablesCount = FsmVariables.GlobalVariables.GetAllNamedVariables().Length;

		Debug.Log("parseStats for "+fsmCount+" Fsms");
		foreach (var fsm in fsms)
		{
			eventCount += fsm.Events.Length;
			stateCount += fsm.States.Length;
			variableCount +=fsm.Variables.BoolVariables.Length;
			variableCount +=fsm.Variables.ColorVariables.Length;
			variableCount +=fsm.Variables.FloatVariables.Length;
			variableCount +=fsm.Variables.IntVariables.Length;
			variableCount +=fsm.Variables.MaterialVariables.Length;
			variableCount +=fsm.Variables.ObjectVariables.Length;
			variableCount +=fsm.Variables.QuaternionVariables.Length;
			variableCount +=fsm.Variables.RectVariables.Length;
			variableCount +=fsm.Variables.StringVariables.Length;
			variableCount +=fsm.Variables.TextureVariables.Length;
			variableCount +=fsm.Variables.Vector3Variables.Length;
		}		
		
	}
    
    void OnGUI () {

		if (GUILayout.Button("Scan"))
		{
			parseStats();
		}

	   GUILayout.Label("Fsms              : " + fsmCount);
	   GUILayout.Label("states            : " + stateCount);
	   GUILayout.Label("local variables   : " + variableCount);
		GUILayout.Label("local Events     : " + eventCount);
	   GUILayout.Label("Global Events     : " + globalEventsCount);
	   GUILayout.Label("Global Variables  : " + globalVariablesCount);
	
		/*
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(300), GUILayout.Height(300));
		
			foreach (var fsm in fsms)
			{
				 GUILayout.Label (fsm.Name);
			}
        
        GUILayout.EndScrollView();
		*/

	
		
    }
}