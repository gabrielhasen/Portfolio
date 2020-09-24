using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataSystem
{
    [Serializable]
    public class SerializedGeomancer
    {
        public GeomancerIdentifier GeoType;
        public bool UnlockStatus;
        public SerializedGeomancer(GeomancerIdentifier _geoType, bool _unlockStatus)
        {
            GeoType = _geoType;
            UnlockStatus = _unlockStatus;
        }
    }

    [Serializable]
    public class SerializedGeomancerManager
    {
        public SerializedGeomancer[] geomancers;
    }
}

