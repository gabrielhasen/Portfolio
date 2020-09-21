using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeomancerList", menuName = "Data System/Geomancer List")]
public class GeomancerList : ScriptableObject
{
    public string GeomancerListID;
    public List<Geomancer> Geomancers;
}
