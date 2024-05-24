using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class ResultSceneView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText; // ç°å„Imageó\íË
    [SerializeField]
    private TextMeshProUGUI _timeText;
    [SerializeField]
    private TextMeshProUGUI _time;
    [SerializeField]
    private TextMeshProUGUI _percentageText;
    [SerializeField]
    private TextMeshProUGUI _percentage;

    private void Start()
    {
        SetExpoText();
        SetScoreText();
    }

    private void SetExpoText()
    {
        if (PlayerPrefs.GetInt("IsWin", 0)  == 0)
        {
            _timeText.text = "Time";
            _percentageText.text = "HP";
        }
        else
        {
            _timeText.text = "Time";
            _percentageText.text = "Parry";
        }
    }

    private void SetScoreText()
    {

        _time.text = PlayerPrefs.GetFloat("Time", 0).ToString("F0");
        if(PlayerPrefs.GetInt("IsWin", 0) == 0)
        {
            _percentage.text = CalcPercentage(PlayerPrefs.GetInt("MaxHP", 100), PlayerPrefs.GetInt("CurrentHP", 1)).ToString() + "%";
        }
        else
        {
            _percentage.text = CalcPercentage(PlayerPrefs.GetInt("EnemyAttackCount", 100), PlayerPrefs.GetInt("ParryCount", 1)).ToString() + "%";
        }
    }

    private float CalcPercentage(int max, int current)
    {
        float x = (float)current / (float)max;
        Debug.Log(x);
        return x * 100;

    }
}
