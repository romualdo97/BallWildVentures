using UnityEngine;

/// <summary>
/// Controls the ball movement and state
/// </summary>
public partial class Ball : MonoBehaviour
{
    private sealed class PropellingState : BaseState
    {
        public float Fuel => 1f - m_currentPropellingTime / m_totalPropellingTime;

        private float m_currentPropellingTime;
        private float m_totalPropellingTime = 1;

        public PropellingState(Ball ball, BallState allowedTransitionsBitField) : base(ball, allowedTransitionsBitField)
        {
            m_type = BallState.Propelling;
        }

        public void Refuel()
        {
            m_currentPropellingTime = 0;
        }

        public override void OnActivate()
        {
            base.OnActivate();
            m_ball.m_currentJetpackBoost = m_ball.m_jetpackBoostStrength;

            // V = D / T -> D / V = T
            m_totalPropellingTime = m_ball.m_initialJetpackFuel / m_ball.m_jetpackCostFuelPerSecond;
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();
            m_ball.m_currentJetpackBoost = 0;
        }

        public override void FixedUpdateState()
        {
            m_ball.m_rigidBody.AddForce(Vector3.up * m_ball.m_currentJetpackBoost, ForceMode.Acceleration);

            // Debug.Log($"m_currentPropellingTime{m_currentPropellingTime} / m_totalPropellingTime{m_totalPropellingTime} = {m_currentPropellingTime / m_totalPropellingTime}");
            m_currentPropellingTime += Time.fixedDeltaTime;
            if (m_currentPropellingTime >= m_totalPropellingTime)
            {
                m_ball.TryChangeState(m_ball.m_flyingState);
            }
        }

        public override bool CanTransitionFrom(BaseState oldState)
        {
            // Reject the transition coming into if no remaining fuel
            return m_currentPropellingTime < m_totalPropellingTime;
        }
    }
}
