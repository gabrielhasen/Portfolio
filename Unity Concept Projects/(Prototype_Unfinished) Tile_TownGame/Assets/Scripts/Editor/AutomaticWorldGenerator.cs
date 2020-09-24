using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(WorldController))]
public class AutomaticWorldGenerator : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if(GUILayout.Button("Generate World")){
			WorldController script = ((WorldController)target);
			script.GUIdelete();
			script.GUIstart();
		}
		if(GUILayout.Button("Delete World")){
			WorldController script = ((WorldController)target);
			script.GUIdelete();
		}
	}
}
