using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeomancerHolder : MonoBehaviour
{
    public Geomancer info;
    private GeomancerInspectorManager inspector;

    void Start()
    {
        inspector = this.transform.parent.GetComponent<GeomancerInspectorManager>();
    }

}
