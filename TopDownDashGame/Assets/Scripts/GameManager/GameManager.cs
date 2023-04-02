using Assets.Scripts.Spawner;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using System;
using System.Collections;
using static System.TimeZoneInfo;

namespace Assets.Scripts.GameManager
{
    [RequireComponent(typeof(CountdownTimer))]
    public class GameManager : MonoBehaviour
    {
        public event Action OnUIValuesChanged;
        public event Action OnGameEnded;

        public static GameManager Instance { get; private set; }
        public static PlayerSpawner PlayerSpawnerInstance { get => PlayerSpawner.Instance; }

        public int Score { get; private set; }
        public int Highscore { get => m_highscoreSO.Score; }

        [Header("UI")]
        [SerializeField] private HighscoreSO m_highscoreSO;
        [SerializeField] private GameObject m_ingameUIScreenManagerObject;
        [SerializeField] private GameObject m_endScreenManagerObject;
        private InGameUIScreenManager m_ingameUIScreenManager;
        private EndScreenManager m_endScreenManager;

        [Header("Animation")]
        [SerializeField] private Animator m_transitionAnimator;
        [SerializeField] private float m_transitionTime = 1f;

        [Header("Portal")]
        [SerializeField] private GameObject m_portalPrefab;
        [SerializeField] private Transform m_portalSpawn;

        private CountdownTimer m_countdownTimer;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            else
            {
                Instance = this;
            }

            if (m_transitionAnimator == null)
                Debug.LogWarning("No Animator selected!");

            InitCountdownTimer();
        }

        private void Start()
        {
            m_endScreenManager = m_endScreenManagerObject.GetComponent<EndScreenManager>();

            if (SceneManager.GetActiveScene().name == "NormalLevel")
            {
                m_ingameUIScreenManager = m_ingameUIScreenManagerObject.GetComponent<InGameUIScreenManager>();                
            }                

            StartGame();

            //PlayerSpawnerInstance.GetActivePlayer().GetComponent<Health>().OnDied += HandlePlayerDied;
        }

        private void OnDestroy()
        {
            OnUIValuesChanged = null;
            m_countdownTimer.OnTimerEnded -= HandleTimerEnded;
        }

        public void PlayOnButton_Click()
        {
            // Spawn Portal
            SpawnPortal();

            // Stop Enemy Spawn
            //m_countdownTimer = null;

            // Gegner wegmeddeln
            GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach(e => Destroy(e.gameObject));

            // Hide UI like on start
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //m_ingameUIScreenManager?.DisplayScreen();
            m_endScreenManager?.HideScreen();            
        }

        public void RestartButton_Click()
        {
            SetAnimationTrigger("Start");

            StartCoroutine(LoadScene("NormalLevel"));
        }

        public void MainMenuButton_Click()
        {
            SetAnimationTrigger("MainMenu");

            StartCoroutine(LoadScene("MainMenu"));
        }

        public void ExitButton_Click()
        {
            StartCoroutine(QuitGame());
        }

        public void IncreaseScore()
        {
            Score++;
            OnUIValuesChanged?.Invoke();
        }

        public void LoadBossLevel()
        {
            SetAnimationTrigger("Start");
            StartCoroutine(LoadScene("BossLevel"));
        }

        private void InitCountdownTimer()
        {
            m_countdownTimer = GetComponent<CountdownTimer>();
            m_countdownTimer.OnTimerEnded += HandleTimerEnded;
        }

        private void HandleTimerEnded()
        {
            m_countdownTimer.Stop();
            //m_countdownTimer.OnTimerEnded -= HandleTimerEnded;
            EndGame();
        }

        private void StartGame()
        {
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            m_ingameUIScreenManager?.DisplayScreen();
            m_endScreenManager?.HideScreen();

            Score = 0;
            OnUIValuesChanged?.Invoke();            
        }        

        private void EndGame()
        {
            Time.timeScale = 0;

            OnGameEnded?.Invoke();

            if (Score > m_highscoreSO.Score)
            {
                m_highscoreSO.Score = Score;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            m_ingameUIScreenManager?.HideScreen();
            m_endScreenManager?.DisplayScreen();

            //if (SceneManager.GetActiveScene().name == "NormalLevel")
            //{
            //    SetAnimationTrigger("Start");
            //    StartCoroutine(LoadScene("BossLevel"));
            //}
            //else if (SceneManager.GetActiveScene().name == "BossLevel")
            //{
            //    Cursor.lockState = CursorLockMode.None;
            //    Cursor.visible = true;
            //
            //    m_ingameUIScreenManager?.HideScreen();
            //    m_endScreenManager?.DisplayScreen();
            //}

        }

        private void SetAnimationTrigger(string triggerName)
        {
            Time.timeScale = 1;
            m_transitionAnimator.SetTrigger(triggerName);
        }

        private IEnumerator QuitGame()
        {
            SetAnimationTrigger("Exit");

            yield return new WaitForSeconds(m_transitionTime);

            Application.Quit();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return new WaitForSeconds(m_transitionTime);

            SceneManager.LoadScene(sceneName);
        }
        private void SpawnPortal()
        {
            Instantiate(m_portalPrefab, m_portalSpawn.position, m_portalSpawn.localRotation);
        }

        public void BossKilled()
        {
            m_endScreenManager.ChangedHeader("You won!");

            EndGame();
        }

        public void PlayerKilled()
        {
            m_endScreenManager.ChangedHeader("You lost!");
            
            Invoke("EndGame", .25f);
        }

        private void HandlePlayerDied()
        {
            m_endScreenManager.ChangedHeader("You lost!");

            

            Invoke("EndGame", .25f);
        }
    }
}