using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform   m_target;
    private float       offsetZ = 4;
    private float       offsetY = 3;

    private void FixedUpdate()
    {
        transform.LookAt(m_target.position);
        transform.position = m_target.position - Vector3.forward * offsetZ + Vector3.up * offsetY;
    }

    public void SetTarget(Transform target)
    {
        m_target = target;
    }
}
