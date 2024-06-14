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
    [SerializeField]
    private TextMeshProUGUI _clearRankText;

    [SerializeField]
    private float _sScore = 100;
    [SerializeField]
    private float _aScore = 75;
    [SerializeField]
    private float _bScore = 50;

    [SerializeField]
    private float _sTime = 30;
    [SerializeField]
    private float _aTime = 90;
    [SerializeField]
    private float _bTime = 120;

    [SerializeField]
    private Color _sColor = Color.yellow;
    [SerializeField]
    private Color _aColor = Color.green;
    [SerializeField]
    private Color _bColor = Color.blue;
    [SerializeField]
    private Color _cColor = Color.red;
    [SerializeField]
    private Color _dColor = Color.gray;

    enum ClearRank
    {
        S,
        A,
        B,
        C,
        D
    };

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
        var percentage = CalcPercentage(
                    isWin ? PlayerPrefs.GetInt("EnemyAttackCount", 100) : PlayerPrefs.GetInt("MaxHP", 100),
                    isWin ? PlayerPrefs.GetInt("ParryCount", 1) : PlayerPrefs.GetInt("CurrentHP", 1)
                    );
        _percentage.text = percentage.ToString("F2") + "%";
        SetClearRank(GetClearRank(isWin, percentage, PlayerPrefs.GetFloat("Time", 0)));
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
        return percentage;
    }

    private void SetClearRank(ClearRank clearRank) 
    {
        switch (clearRank)
        {
            case ClearRank.S:
                _clearRankText.text = "S";
                _clearRankText.color = _sColor;
                break;
            case ClearRank.A:
                _clearRankText.text = "A";
                _clearRankText.color = _aColor;
                break;
            case ClearRank.B:
                _clearRankText.text = "B";
                _clearRankText.color = _bColor;
                break;
            case ClearRank.C:
                _clearRankText.text = "C";
                _clearRankText.color = _cColor;
                break;
            case ClearRank.D:
                _clearRankText.text = "D";
                _clearRankText.color = _dColor;
                break;
        }
    }

    private ClearRank GetClearRank(bool isWin, float percentage, float time) => isWin switch
    {
        true => CalcClearRank(percentage, time),
        false => ClearRank.D,
    };

    private ClearRank CalcClearRank(float percentage, float time)
    {
        if (percentage >= _sScore && time <= _sTime)
            return ClearRank.S;
        else if (percentage >= _aScore && time <= _aTime)
            return ClearRank.A;
        else if (percentage >= _bScore && time <= _bTime)
            return ClearRank.B;
        else
            return ClearRank.C;
    }


}
