using System;
using UnityEngine;

/// <summary>
/// Controls the ball movement and state
/// </summary>
public partial class Ball : MonoBehaviour
{
    // Rolling transitions
    // Rolling -> Landed | Rolling -> Flying | Rolling -> Propelling

    // Propelling transtitions
    // Propelling -> Flying

    // Landed transitions
    // Landed -> Propelling | Landed -> Static | Landed -> Flying

    // Flying transitions
    // Flying -> Propelling | Flying -> Landed

    // Static transitions
    // Static -> Propelling | Static -> Flying

    /// <summary>
    /// All ball states
    /// </summary>
    [Flags]
    private enum BallState
    {
        None = 0,

        /// <summary>
        /// (Player) Rolling on ground.
        /// </summary>
        Rolling = 1 << 0,

        /// <summary>
        /// (Player) Bursting jetpack fuel
        /// </summary>
        Propelling = 1 << 1,

        /// <summary>
        /// Moving on ground without player interaction
        /// </summary>
        Braking = 1 << 2,

        /// <summary>
        /// Landed after flying
        /// </summary>
        Landed = 1 << 3,

        /// <summary>
        /// Start flying because of jump
        /// </summary>
        Jumping = 1 << 5,

        /// <summary>
        /// Flying without player interaction
        /// </summary>
        Flying = 1 << 6,

        /// <summary>
        /// Static on ground
        /// </summary>
        Static = 1 << 7
    }

    private abstract class BaseState
    {
        public BallState Type => m_type;
        protected Ball m_ball;
        protected BallState m_type;
        protected BallState m_allowedTransition;

        public BaseState(Ball ball, BallState allowedTransitionsBitField = BallState.None)
        {
            m_ball = ball;
            m_allowedTransition = allowedTransitionsBitField;
        }

        public virtual void OnActivate() { }
        public virtual void OnDeactivate() { }
        public virtual void UpdateState() { }
        public virtual void FixedUpdateState() { }
        public virtual void OnCollisionEnter(Collision collision) { }

        public virtual bool CanTransitionTo(BaseState newState)
        {
            return (m_allowedTransition & newState.m_type) > 0;
        }

        public virtual bool CanTransitionFrom(BaseState oldState)
        {
            return true; // Let the new state decide if can accept becoming the active state
        }
    }

    private bool TryChangeState(BaseState newState)
    {
        if (m_activeState != null && newState != null &&
            m_activeState.CanTransitionTo(newState) &&
            newState.CanTransitionFrom(m_activeState))
        {
            m_activeState.OnDeactivate();
            m_activeState = newState;
            m_activeState.OnActivate();
            return true;
        }

        if (m_activeState == null && newState != null)
        {
            m_activeState = newState;
            m_activeState.OnActivate();
            return true;
        }
        else if (m_activeState != null && newState == null)
        {
            m_activeState.OnDeactivate();
            m_activeState = null;
            return true;
        }

        return false;
    }

}
