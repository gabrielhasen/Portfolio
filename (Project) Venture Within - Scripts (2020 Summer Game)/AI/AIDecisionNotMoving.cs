using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// This decision will return true if an object on its TargetLayer layermask is within its specified radius, false otherwise. It will also set the Brain's Target to that object.
    /// </summary>
	[AddComponentMenu("Corgi Engine/Character/AI/Decisions/AI Decision Not Moving")]
    [RequireComponent(typeof(Character))]
    public class AIDecisionNotMoving : AIDecision
    {
        /// Time between each check
        public float TimeToCheck = 1.0f;
        /// Should check if the agent has moved some distance on the X axis
        public bool Calculate_X = false;
        /// Should check if the agent has moved some distance on the Y axis
        public bool Calculate_Y = false;
        /// The minimum distance the agent has moved on the X axis since last check
        public float Distance_X = 0.2f;
        /// The minimum distance the agent has moved on the Y axis since last check
        public float Distance_Y = 0.2f;

        protected Vector2 _lastPosition;
        protected float _currentTime;

        /// <summary>
        /// On Decide we evaluate our time
        /// </summary>
        /// <returns></returns>
        public override bool Decide()
        {
            _currentTime += Time.deltaTime;
            return SamePosition();
        }

        /// <summary>
        /// Returns true if enough time has passed since we entered the current state and
        /// haven't moved.
        /// </summary>
        /// <returns></returns>
        protected virtual bool SamePosition()
        {
            if (_brain == null) { return false; }
            if (_currentTime < TimeToCheck) { return false; }
            
            if (Calculate_X) {
                float distance_Min = _lastPosition.x - Distance_X;
                float distance_Max = _lastPosition.x + Distance_X;
                if (distance_Min <= transform.position.x && transform.position.x <= distance_Max) {
                    _lastPosition = transform.position;
                    _currentTime = 0;
                    return true;
                }
            }

            if (Calculate_Y) {
                float distance_Min = _lastPosition.y - Distance_Y;
                float distance_Max = _lastPosition.y + Distance_Y;
                if (distance_Min <= transform.position.y && transform.position.y <= distance_Max) {
                    _lastPosition = transform.position;
                    _currentTime = 0;
                    return true;
                }
            }

            _currentTime = 0;
            _lastPosition = transform.position;
            return false;
        }

        /// <summary>
        /// On init we randomize our next delay
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();
        }

        /// <summary>
        /// On enter state we randomize our next delay
        /// </summary>
        public override void OnEnterState()
        {
            base.OnEnterState();
            _lastPosition = transform.position;
            _currentTime = 0;
        }
    }
}


