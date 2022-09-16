using UnityEngine;

/// <summary>
/// Controls the ball movement and state
/// </summary>
public partial class Ball : MonoBehaviour
{
    private sealed class BrakingState : BaseState
    {
        public BrakingState(Ball ball, BallState allowedTransitionsBitField) : base(ball, allowedTransitionsBitField)
        {
            m_type = BallState.Braking;
        }

        public override void FixedUpdateState()
        {
            //Debug.Log("Landed@FixedUpdate");
            // How is angular damping applied in PhysX? https://forum.unity.com/threads/how-is-angular-drag-applied-to-a-rigidbody-in-unity-how-is-angular-damping-applied-in-physx.369599/
            float fixedFps = 1 / Time.fixedDeltaTime;
            m_ball.m_rigidBody.angularDrag = fixedFps * m_ball.m_angularStopFactor; // Reset angular drag

            // Detect change to static
            if (m_ball.m_rigidBody.angularVelocity.sqrMagnitude <= 1f)
            {
                m_ball.m_rigidBody.velocity = Vector3.zero;
                m_ball.TryChangeState(m_ball.m_staticState);
            }
        }
    }
}
