using System;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Controls the ball movement and state
/// </summary>
public partial class Ball : MonoBehaviour
{
    public Action<GameObject> OnBallLanded;
    public float Fuel => m_propellingState.Fuel;

    private BallState           m_state;
    private BaseState           m_activeState;
    private Rigidbody           m_rigidBody;
    private SphereCollider      m_sphereCollider;
    private Vector3             m_axisAngularAcceleration;
    private float               m_currentJetpackBoost;

    private RollingState        m_rollingState;
    private PropellingState     m_propellingState;
    private BrakingState        m_brakingState;
    private LandedState         m_landedState;
    private JumpingState        m_jumpingState;
    private FlyingState         m_flyingState;
    private StaticState         m_staticState;

    [Header("Movement Settings")]
    [SerializeField, Tooltip("Max speed in m/s")]
    private float m_maxSpeed = 1;
    [SerializeField, Tooltip("How high the sphere can jump?")]
    private float m_maxJumpDistance = 1;
    [SerializeField, Tooltip("Angular accel in deg/s which determines how fast the ball will start rolling.")]
    private float m_angularAcceleration = 1;
    [SerializeField, Tooltip("How fast should the ball stop rotating when player not interacting?"), Range(0, 1)]
    private float m_angularStopFactor = 1;

    [Header("Jetpack Settings")]
    [SerializeField, Tooltip("Jetpack fuel")]
    private float m_initialJetpackFuel = 10f;
    [SerializeField, Tooltip("Boost strength (this is an acceleration)")]
    private float m_jetpackBoostStrength = 10f;
    [SerializeField, Tooltip("How much fuel is burnt per second")]
    private float m_jetpackCostFuelPerSecond = 5f;

    private void Awake()
    {
        // Get components
        m_rigidBody = GetComponent<Rigidbody>();
        m_sphereCollider = GetComponent<SphereCollider>();

        // Create state handlers
        m_rollingState = new RollingState(this, BallState.Braking | BallState.Jumping | BallState.Propelling);
        m_propellingState = new PropellingState(this, BallState.Flying);
        m_brakingState = new BrakingState(this, BallState.Propelling | BallState.Static | BallState.Jumping);
        m_landedState = new LandedState(this, BallState.Rolling | BallState.Braking | BallState.Jumping);
        m_jumpingState = new JumpingState(this, BallState.Flying);
        m_flyingState = new FlyingState(this, BallState.Propelling | BallState.Landed);
        m_staticState = new StaticState(this, BallState.Rolling | BallState.Propelling | BallState.Jumping);

        // Set initial state
        TryChangeState(m_flyingState);

        // Some checks
        Assert.IsNotNull(m_rigidBody, "Missing rigidBody reference");
        Assert.IsNotNull(m_sphereCollider, "Missing sphereCollider reference");
    }

    private void Update()
    {
        m_state = m_activeState.Type;
        m_activeState.UpdateState();
        Debug.DrawLine(transform.position, transform.position + m_rigidBody.velocity, Color.red);
    }

    private void FixedUpdate()
    {
        // http://labman.phys.utk.edu/phys221core/modules/m6/kinematics.html
        // In terms of the angular speed w, the speed v of the point P is v = wr;
        // so calculate the max angular velocity from the max tangential velocity
        m_rigidBody.maxAngularVelocity = m_maxSpeed / m_sphereCollider.radius;

        m_activeState.FixedUpdateState();
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_activeState.OnCollisionEnter(collision);
        OnBallLanded?.Invoke(collision.gameObject);
    }

    public void SetRollAxis(Vector3 rollAroundAxis)
    {
        m_axisAngularAcceleration = rollAroundAxis * m_angularAcceleration;
        TryChangeState(m_rollingState);
    }

    public void Brake()
    {
        TryChangeState(m_brakingState);
    }

    public void Jump()
    {
        TryChangeState(m_jumpingState);
    }

    public void PropelRocket()
    {
        TryChangeState(m_propellingState);
    }

    public void StopPropelRocket()
    {
        TryChangeState(m_flyingState);
    }
}
