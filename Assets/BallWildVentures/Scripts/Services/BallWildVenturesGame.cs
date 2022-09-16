using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Service locator class, simple app bootstraper
/// </summary>
public class BallWildVenturesGame : MonoBehaviour
{
    public static BallWildVenturesGame Instance { get; private set; }
    public float TotalJumps { get; private set; }
    public Ball Ball => m_ball;


    [SerializeField] Transform             m_playerStartPosition;
    [SerializeField] GameObject            m_player;
    [SerializeField] CameraController      m_cameraController;
    [SerializeField] GameObject            m_padPrefab;

    Ball m_ball;
    InputDetector                          m_inputDetector;
    BallPlayerController                   m_playerController;
    GameObject                             m_nextPad;

    private void Awake()
    {
        Instance = this;

        // Create the default input detector
        m_inputDetector = new InputDetector();

        // Set the player position
        m_player.transform.SetPositionAndRotation(m_playerStartPosition.position, m_playerStartPosition.rotation);
        m_ball = m_player.GetComponent<Ball>();
        m_ball.OnBallLanded += HandleBallLanded;

        // Setup the camera controller
        m_cameraController.SetTarget(m_player.transform);

        // Setup the player controller
        m_playerController = new BallPlayerController(m_inputDetector, m_ball);
        m_playerController.Activate();

        // Create next pad
        GenerateNextPad(15);
    }

    private void Update()
    {
        // Update the input detector
        m_inputDetector.UpdateInput();

        // Simple restart because this is a simple game
        if (m_ball.transform.position.y < -20)
        {
            SceneManager.LoadScene(0);
            Debug.Log("Restarting");
        }
    }

    private void OnDestroy()
    {
        m_playerController.Deactivate();
    }

    private void GenerateNextPad(float z)
    {
        m_nextPad = Instantiate(m_padPrefab);
        m_nextPad.transform.position = new Vector3(Random.Range(-5, 5), 0, z);
    }

    private void HandleBallLanded(GameObject pad)
    {
        if (pad == m_nextPad)
        {
            GenerateNextPad(m_nextPad.transform.position.z + Random.Range(5f, 12f));
            ++TotalJumps;
        }
    }
}
