using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to detect the input for the gameplay on PC
/// </summary>
public class InputDetector
{
    public Action<Vector3/* Input forward */, Vector3/* Input right */> OnRollRequest;
    public Action OnRollRequestComplete;

    public Action OnRocketPropelRequest;
    public Action OnRocketPropelRequestComplete;

    public Action OnJumpRequest;

    public bool Enable { get; set; } = false;
    private bool m_processingMovementInput = false;
    private bool m_processingPropelInput = false;

    public void UpdateInput()
    {
        if (!Enable) return;
        ProcessRocketPropel();
        ProcessJumpInput();
        ProcessMovementInput();
    }

    protected virtual void ProcessJumpInput() // Virtual so we can override when targeting other platforms
    {
        if (Input.GetKey(KeyCode.Space))
        {
            OnJumpRequest?.Invoke();
        }
    }

    protected virtual void ProcessRocketPropel()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            OnRocketPropelRequest?.Invoke();
            m_processingPropelInput = true;
        }
        else if (m_processingPropelInput)
        {
            OnRocketPropelRequestComplete?.Invoke();
            m_processingPropelInput = false;
        }
    }

    protected virtual void ProcessMovementInput()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        if (Mathf.Approximately(horizontalAxis, 0) && Mathf.Approximately(verticalAxis, 0))
        {
            if (m_processingMovementInput)
            {
                OnRollRequestComplete?.Invoke();
                m_processingMovementInput = false;
            }
            return;
        }

        m_processingMovementInput = true;
        float angleDeg = Mathf.Atan2(verticalAxis, horizontalAxis) * Mathf.Rad2Deg;
        Vector3 inputForward = Quaternion.Euler(0, -angleDeg, 0) * Vector3.right;
        Vector3 inputRight = Vector3.Cross(Vector3.up, inputForward);

        OnRollRequest?.Invoke(inputForward, inputRight);

        // Debug input vectors
        Debug.DrawLine(Vector3.zero, inputForward, Color.red);
        Debug.DrawLine(Vector3.zero, inputRight, Color.green);
    }
}
