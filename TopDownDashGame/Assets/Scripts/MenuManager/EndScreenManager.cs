using Assets.Scripts.MenuManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenManager : ScreenManager
{                      
    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private TextMeshProUGUI m_highscoreText;

    public override void DisplayScreen()
    {
        base.DisplayScreen();        
        UpdateDataTextFields();
    }

    private void UpdateDataTextFields()
    {
        float score = GameManager.Instance.Score;
        float highscore = GameManager.Instance.Highscore;

        m_scoreText.text = $"Your Score : {score}";
        m_highscoreText.text = $"Highscore : {highscore}";
    }
}
