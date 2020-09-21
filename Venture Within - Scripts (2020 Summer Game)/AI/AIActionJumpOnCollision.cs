using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{
    /// <summary>
    /// This action performs the defined number of jumps. Look below for a breakdown of how this class works.
    /// </summary>
	[AddComponentMenu("Corgi Engine/Character/AI/Actions/AI Action Jump On Collision")]
    [RequireComponent(typeof(CharacterJump))]
    public class AIActionJumpOnCollision : AIAction
    {
        /// the number of jumps to perform while in this state
        public int NumberOfJumps = 1;

        protected Vector2 _direction;
        protected int _numberOfJumps = 0;
        protected CharacterJump _characterJump;
        protected CorgiController _controller;
        protected Character _character;
        protected CharacterHorizontalMovement _characterHorizontalMovement;

        /// <summary>
        /// On init we grab our AI components
        /// </summary>
        protected override void Initialization()
        {
            _controller = GetComponent<CorgiController>();
            _character = GetComponent<Character>();
            _characterHorizontalMovement = GetComponent<CharacterHorizontalMovement>();
            _characterJump = GetComponent<CharacterJump>();
            _direction = _character.IsFacingRight ? Vector2.right : Vector2.left;
        }

        /// <summary>
        /// On PerformAction we jump
        /// </summary>
        public override void PerformAction()
        {
            Jump();
        }

        /// <summary>
        /// Calls CharacterJump's JumpStart method to initiate the jump
        /// </summary>
        protected virtual void Jump()
        {
            if (_character == null) {
                return;
            }
            if ((_character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
                || (_character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Frozen)) {
                return;
            }

            if ((_controller.State.IsGrounded)) {
                _numberOfJumps = 0;
            }

            _direction = _character.IsFacingRight ? Vector2.right : Vector2.left;
            CheckForWalls();
        }

        protected virtual void CheckForWalls()
        {
            // if the agent is colliding with something, make it turn around
            if ((_direction.x < 0 && _controller.State.IsCollidingLeft) || (_direction.x > 0 && _controller.State.IsCollidingRight)) {
                InitiateJump();
            }
        }

        protected virtual void InitiateJump()
        {
            if (_numberOfJumps < NumberOfJumps) {
                _characterJump.JumpStart();
                _numberOfJumps++;
            }
        }
    }
}
