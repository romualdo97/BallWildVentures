using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFuelIndicator : MonoBehaviour
{
    [SerializeField] private Slider m_fuelBar;

    private IEnumerator Start()
    {
        yield return new WaitWhile(() => BallWildVenturesGame.Instance == null);
    }

    // Update is called once per frame
    private void Update()
    {
        m_fuelBar.value = BallWildVenturesGame.Instance.Ball.Fuel;
    }
}
