using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(WorldController))]
public class AutomaticWorldGenerator : Editor {

	public override void OnInspectorGUI()
	{
		WorldController script = ((WorldController)target);
		if(DrawDefaultInspector())
		{
			if(script.AutoUpdate)
			{
				script.GUIdelete();
				script.GUIstart();
			}
		}
		if(GUILayout.Button("Generate World")){
			script.GUIdelete();
			script.GUIstart();
		}
		if(GUILayout.Button("Delete World")){
			script.GUIdelete();
		}
	}
}
