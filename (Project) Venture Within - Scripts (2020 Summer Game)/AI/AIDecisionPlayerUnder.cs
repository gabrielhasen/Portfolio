using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// This action performs the defined number of jumps. Look below for a breakdown of how this class works.
    /// </summary>
	[AddComponentMenu("Corgi Engine/Character/AI/Actions/AI Decision Player Under")]
    public class AIDecisionPlayerUnder : AIDecision
    {
        /// the layermask to detect
        public LayerMask DetectionRayMask;
        
        protected int _numberOfJumps = 0;
        protected RaycastHit2D downRay;
        protected CorgiController _controller;
        protected Character _character;
        protected CharacterHorizontalMovement _characterHorizontalMovement;

        /// <summary>
        /// On init we grab our AI components
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();
            downRay = new RaycastHit2D();
            _controller = GetComponent<CorgiController>();
            _character = LevelManager.Instance.PlayerPrefabs[0];
            _characterHorizontalMovement = GetComponent<CharacterHorizontalMovement>();
        }

        /// <summary>
        /// On PerformAction we jump
        /// </summary>
        public override bool Decide()
        {
            return PlayerUnder();
        }

        /// <summary>
        /// Calls CharacterJump's JumpStart method to initiate the jump
        /// </summary>
        protected virtual bool PlayerUnder()
        {
            if (_character == null) {
                return false;
            }
            if ((_character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
                || (_character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Frozen)) {
                return false;
            }

            if ((_controller.State.IsGrounded)) {
                _numberOfJumps = 0;
            }

            CheckForCollision();
            return true;
        }

        protected virtual void CheckForCollision()
        {
            
        }

    }
}
