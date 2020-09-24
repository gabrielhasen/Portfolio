using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;


namespace MoreMountains.CorgiEngine
{
    public class ExtendedGameManager : GameManager
    {
        protected override void Start()
        {
            base.Start();
            MMDebug.DebugLogTime("new game manager");
        }

        public override void AddPoints(int pointsToAdd)
        {
            base.AddPoints(pointsToAdd);
            MMDebug.DebugLogTime("new game manager add points");
        }
    }
}