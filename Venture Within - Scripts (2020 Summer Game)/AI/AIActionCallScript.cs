using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.Events;

namespace MoreMountains.CorgiEngine
{

    public class AIActionCallScript : AIAction
    {
        // Add this component to an AI Brain and agent speed will flicker the same way when damage is taken but enless.
        // Muppo (2018)

        public UnityEvent CallEvent;

        private bool hasBeenCalled;

        ///
        protected override void Initialization()
        {
            hasBeenCalled = false;
        }

        ///
        public override void PerformAction()
        {
            if (!hasBeenCalled) {
                hasBeenCalled = true;
                CallEvent.Invoke();
            }
        }
    }
}
