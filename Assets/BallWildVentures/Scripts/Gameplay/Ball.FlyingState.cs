using UnityEngine;

/// <summary>
/// Controls the ball movement and state
/// </summary>
public partial class Ball : MonoBehaviour
{
    private sealed class FlyingState : BaseState
    {
        public FlyingState(Ball ball, BallState allowedTransitionsBitField) : base(ball, allowedTransitionsBitField)
        {
            m_type = BallState.Flying;
        }

        public override void OnCollisionEnter(Collision collision)
        {
            m_ball.m_propellingState.Refuel(); // Make sure to refuel jetpack when grounded
            m_ball.TryChangeState(m_ball.m_landedState);
        }
    }
}
