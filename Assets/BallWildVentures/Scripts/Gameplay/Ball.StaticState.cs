using UnityEngine;

/// <summary>
/// Controls the ball movement and state
/// </summary>
public partial class Ball : MonoBehaviour
{
    private sealed class StaticState : BaseState
    {
        public StaticState(Ball ball, BallState allowedTransitionsBitField) : base(ball, allowedTransitionsBitField)
        {
            m_type = BallState.Static;
        }
    }
}
