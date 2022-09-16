using UnityEngine;

/// <summary>
/// Controls the ball movement and state
/// </summary>
public partial class Ball : MonoBehaviour
{
    private sealed class JumpingState : BaseState
    {
        public JumpingState(Ball ball, BallState allowedTransitionsBitField) : base(ball, allowedTransitionsBitField)
        {
            m_type = BallState.Jumping;
        }

        public override void OnActivate()
        {
            base.OnActivate();

            // Calculate initial velocity required to match jump height: https://www.toppr.com/guides/physics/motion-in-a-plane/projectile-motion/#:~:text=Important%20Points%20of%20Projectile%20Motion,-The%20linear%20momentum&text=The%20path%20of%20a%20projectile%20is%20parabolic.&text=Throughout%20the%20motion%2C%20the%20acceleration,of%20h%20denotes%20the%20height.
            // h_max = (u * sin)^2 / 2g where sin = 1 because this is a vertical jump
            // h_max = u^2 / 2g -> h_max * 2g = u^2 -> sqrt(h_max * 2g) = u
            float initialSpeed = Mathf.Sqrt(m_ball.m_maxJumpDistance * 2 * Mathf.Abs(Physics.gravity.y));
            m_ball.m_rigidBody.AddForce(Vector3.up * initialSpeed, ForceMode.VelocityChange);
            m_ball.TryChangeState(m_ball.m_flyingState);
        }
    }
}
