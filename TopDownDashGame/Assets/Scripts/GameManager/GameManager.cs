using Assets.Scripts.Spawner;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using System;
using static System.TimeZoneInfo;
using System.Collections;

[RequireComponent(typeof(CountdownTimer))]
public class GameManager : MonoBehaviour
{
    public event Action OnUIValuesChanged;

    public static GameManager Instance { get; private set; }
    public static PlayerSpawner PlayerSpawnerInstance { get => PlayerSpawner.Instance; }

    public int Score { get; private set; }
    public int Highscore { get => m_highscoreSO.Score; }

    [SerializeField] private HighscoreSO m_highscoreSO;
    [SerializeField] private GameObject m_ingameUIScreenManagerObject;
    [SerializeField] private GameObject m_endScreenManagerObject;
    private InGameUIScreenManager m_ingameUIScreenManager;
    private EndScreenManager m_endScreenManager;

    [SerializeField] private Animator m_transitionAnimator;
    [SerializeField] private float m_transitionTime = 1f;
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
        m_ingameUIScreenManager = m_ingameUIScreenManagerObject.GetComponent<InGameUIScreenManager>();
        m_endScreenManager = m_endScreenManagerObject.GetComponent<EndScreenManager>();

        StartGame();
    }

    private void OnDestroy()
    {
        OnUIValuesChanged = null;
        m_countdownTimer.OnTimerEnded -= HandleTimerEnded;
    }

    public void RestartButton_Click()
    {
        SetAnimationTrigger("Start");

        StartCoroutine(LoadScene(SceneManager.GetActiveScene().name));
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

        m_ingameUIScreenManager.DisplayScreen();
        m_endScreenManager.HideScreen();

        Score = 0;
        OnUIValuesChanged?.Invoke();
    }

    private void EndGame()
    {
        Time.timeScale = 0;

        if (Score > m_highscoreSO.Score)
        {
            m_highscoreSO.Score = Score;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_ingameUIScreenManager.HideScreen();        
        m_endScreenManager.DisplayScreen();        
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
}
