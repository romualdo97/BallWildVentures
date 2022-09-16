using UnityEngine;

/// <summary>
/// Controls the ball movement and state
/// </summary>
public partial class Ball : MonoBehaviour
{
    private sealed class LandedState : BaseState
    {
        public LandedState(Ball ball, BallState allowedTransitionsBitField) : base(ball, allowedTransitionsBitField)
        {
            m_type = BallState.Landed;
        }

        public override void OnActivate()
        {
            base.OnActivate();

            m_ball.TryChangeState(m_ball.m_brakingState);
        }
    }
}
