using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameManager
{
    [RequireComponent(typeof(MainMenuScreenManager))]
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private Animator m_transitionAnimator;
        [SerializeField] private float m_transitionTime = 1f;
        
        private MainMenuScreenManager m_mainMenuScreenManager;

        private void Awake()
        {
            m_mainMenuScreenManager = GetComponent<MainMenuScreenManager>();            
        }

        private void Start()
        {
            m_mainMenuScreenManager.DisplayScreen();

            Time.timeScale = 1;
        }

        public void PlayButton_Click()
        {
            StartCoroutine(LoadLevel("NormalLevel"));
        }

        public void ExitButton_Click()
        {
            StartCoroutine(QuitGame());
        }

        private void SetAnimationTrigger(string triggerName)
        {
            Time.timeScale = 1f;
            m_transitionAnimator.SetTrigger(triggerName);
        }

        private IEnumerator LoadLevel(string levelName)
        {
            SetAnimationTrigger("Start");

            yield return new WaitForSeconds(m_transitionTime);

            SceneManager.LoadScene(levelName);
        }

        private IEnumerator QuitGame()
        {
            SetAnimationTrigger("Exit");

            yield return new WaitForSeconds(m_transitionTime);

            Application.Quit();
        }
    }
}
