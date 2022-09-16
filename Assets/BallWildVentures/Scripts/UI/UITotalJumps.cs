using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITotalJumps : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_score;

    private IEnumerator Start()
    {
        yield return new WaitWhile(() => BallWildVenturesGame.Instance == null);
    }

    // Update is called once per frame
    private void Update()
    {
        m_score.text = BallWildVenturesGame.Instance.TotalJumps.ToString();
    }
}
