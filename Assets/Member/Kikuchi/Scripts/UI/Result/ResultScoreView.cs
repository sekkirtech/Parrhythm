using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class ResultSceneView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText; // 今後Image予定
    [SerializeField]
    private TextMeshProUGUI _timeText;
    [SerializeField]
    private TextMeshProUGUI _time;
    [SerializeField]
    private TextMeshProUGUI _percentageText;
    [SerializeField]
    private TextMeshProUGUI _percentage;

    /// <summary>
    /// 初期化時にテキストを設定します。
    /// </summary>
    private void Start()
    {
        SetExpoText();
        SetScoreText();
    }

    /// <summary>
    /// 勝敗に応じたテキストを設定します。
    /// </summary>
    private void SetExpoText()
    {
        bool isWin = PlayerPrefs.GetInt("IsWin", 0) != 0;
        _timeText.text = "Time";
        _percentageText.text = isWin ? "Parry" : "HP";
    }

    /// <summary>
    /// スコアテキストを設定します。
    /// </summary>
    private void SetScoreText()
    {
        bool isWin = PlayerPrefs.GetInt("IsWin", 0) != 0;
        _time.text = PlayerPrefs.GetFloat("Time", 0).ToString("F2");
        _percentage.text = CalcPercentage(
            isWin ? PlayerPrefs.GetInt("EnemyAttackCount", 100) : PlayerPrefs.GetInt("MaxHP", 100),
            isWin ? PlayerPrefs.GetInt("ParryCount", 1) : PlayerPrefs.GetInt("CurrentHP", 1)
        ).ToString() + "%";
    }
    /// <summary>
    /// パーセンテージを計算します。
    /// </summary>
    /// <param name="max">最大値。</param>
    /// <param name="current">現在値。</param>
    /// <returns>計算されたパーセンテージ。</returns>
    private float CalcPercentage(int max, int current)
    {
        float percentage = (float)current / max * 100;
        Debug.Log(percentage);
        return percentage;
    }
}
