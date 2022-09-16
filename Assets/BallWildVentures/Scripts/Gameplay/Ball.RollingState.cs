using UnityEngine;

/// <summary>
/// Controls the ball movement and state
/// </summary>
public partial class Ball : MonoBehaviour
{
    private sealed class RollingState : BaseState
    {
        public RollingState(Ball ball, BallState allowedTransitionsBitField) : base(ball, allowedTransitionsBitField)
        {
            m_type = BallState.Rolling;
        }

        public override void FixedUpdateState()
        {
            Debug.Log("Rolling@FixedUpdateState");
            m_ball.m_rigidBody.angularDrag = 0; // No angular drag when rolling by player
            m_ball.m_rigidBody.AddTorque(m_ball.m_axisAngularAcceleration, ForceMode.Acceleration);
        }
    }
}
