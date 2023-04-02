using Assets.Scripts.Spawner;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public event EventHandler<TimeEventArgs> OnTimerStarted;
    public event EventHandler<TimeEventArgs> OnTimerElapsed;
    public event Action OnTimerEnded;

    //[SerializeField] private Text m_countdownText;
    [SerializeField] private float m_remainingTimeStart = 10f;
    private float m_remainingTimeCurrent;

    private void Start()
    {
        m_remainingTimeCurrent = m_remainingTimeStart;
        OnTimerStarted?.Invoke(this, new TimeEventArgs(m_remainingTimeCurrent));

        // Invoke StartCountdown after 1 second
        Invoke("StartCountdown", 1.5f);       
    }

    private void OnDestroy()
    {
        OnTimerStarted = null;
        OnTimerElapsed = null;
        OnTimerEnded = null;
    }

    public void Stop()
    {
        CancelInvoke("Countdown");
    }
    private void StartCountdown()
    {
        m_remainingTimeCurrent = m_remainingTimeStart;

        OnTimerStarted?.Invoke(this, new TimeEventArgs(m_remainingTimeCurrent));

        // Invoke Countdown every 1 second without wait time
        InvokeRepeating("Countdown", 0f, 1f);       
        
    }

    private void Countdown()
    {
        m_remainingTimeCurrent--;
        OnTimerElapsed?.Invoke(this, new TimeEventArgs(m_remainingTimeCurrent));

        if (m_remainingTimeCurrent < 0)
        {
            m_remainingTimeCurrent = 0;
            OnTimerEnded?.Invoke();
        }
    }
    
    public class TimeEventArgs : EventArgs
    {
        public float Time;

        public TimeEventArgs(float time)
        {
            Time = time;
        }
    }
}
