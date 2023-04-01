using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts.Spawner
{
    public class Timer
    {
        public event Action OnTimerEnd;
        public event Action OnTimerTick;
        public bool IsActive { get => m_isRunning; }

        private DateTime m_startTime;
        private TimeSpan m_waitTime;
        private Thread m_updateThread;
        private bool m_isRunning = false;

        public Timer(float seconds)
        {
            m_waitTime = TimeSpan.FromSeconds(seconds);
            m_updateThread = new Thread(Update);
        }
        ~Timer()
        {
            OnTimerEnd = null; 
            OnTimerTick = null;
        }


        public void Start()
        {
            m_startTime = DateTime.Now;
            m_isRunning = true;
            if (m_updateThread.ThreadState != ThreadState.Running)
                m_updateThread.Start();
        }

        public void Stop() => m_isRunning = false;

        private void Update()
        {
            while (m_isRunning)
            {
                DateTime currentTime = DateTime.Now;
                TimeSpan elapsedTime = currentTime - m_startTime;
                OnTimerTick?.Invoke();

                if (elapsedTime >= m_waitTime)
                {
                    OnTimerEnd?.Invoke();
                    Stop();
                }

                Thread.Sleep(50); // check again after 50ms  
            }
        }

    }
}
