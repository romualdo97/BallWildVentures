using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Forward actions to the player based on the detected input
/// </summary>
public class BallPlayerController
{
    private InputDetector   m_inputDetector;
    private Ball            m_ballPlayer;
    private bool            m_isActive = false;

    public BallPlayerController(InputDetector inputDetector, Ball ballPlayer)
    {
        m_inputDetector = inputDetector;
        m_ballPlayer = ballPlayer;
    }

    public void Activate()
    {
        if (m_isActive) return;

        m_inputDetector.Enable = true;
        m_inputDetector.OnRollRequest += HandleInputChange;
        m_inputDetector.OnRollRequestComplete += m_ballPlayer.Brake;
        m_inputDetector.OnJumpRequest += m_ballPlayer.Jump;
        m_inputDetector.OnRocketPropelRequest += m_ballPlayer.PropelRocket;
        m_inputDetector.OnRocketPropelRequestComplete += m_ballPlayer.StopPropelRocket;
        m_isActive = true;
    }

    public void Deactivate()
    {
        if (!m_isActive) return;

        m_inputDetector.Enable = true;
        m_inputDetector.OnRollRequest -= HandleInputChange;
        m_inputDetector.OnRollRequestComplete -= m_ballPlayer.Brake;
        m_inputDetector.OnJumpRequest -= m_ballPlayer.Jump;
        m_inputDetector.OnRocketPropelRequest -= m_ballPlayer.PropelRocket;
        m_inputDetector.OnRocketPropelRequestComplete -= m_ballPlayer.StopPropelRocket;
        m_isActive = false;
    }

    private void HandleInputChange(Vector3 inputForward, Vector3 inputRight)
    {
        m_ballPlayer.SetRollAxis(inputRight);
    }
}
