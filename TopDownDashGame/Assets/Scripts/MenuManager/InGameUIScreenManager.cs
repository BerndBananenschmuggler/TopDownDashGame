using Assets.Scripts.MenuManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIScreenManager : ScreenManager
{
    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private TextMeshProUGUI m_countdownText;

    private void Start()
    {
        GameManager.Instance.OnUIValuesChanged += HandleUIValuesChanged;

        CountdownTimer countdownTimer = GameManager.Instance.GetComponent<CountdownTimer>();
        countdownTimer.OnTimerStarted += HandleTimerStarted;
        countdownTimer.OnTimerElapsed += HandleTimerCounted;
    }

    private void OnDestroy()
    {
        CountdownTimer countdownTimer = GameManager.Instance.GetComponent<CountdownTimer>();
        countdownTimer.OnTimerStarted -= HandleTimerStarted;
        countdownTimer.OnTimerElapsed -= HandleTimerCounted;
    }

    private void HandleTimerStarted(object sender, CountdownTimer.TimeEventArgs e)
    {
        m_countdownText.text = e.Time.ToString("0");
    }

    private void HandleTimerCounted(object sender, CountdownTimer.TimeEventArgs e)
    {
        m_countdownText.text = e.Time.ToString("0");
    }

    private void HandleUIValuesChanged()
    {
        if (m_screen.activeSelf)
        {
            m_scoreText.text = GameManager.Instance.Score.ToString();
        }
    }
}
