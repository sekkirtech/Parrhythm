using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 結果シーンのビューを管理するクラス
/// </summary>
public class ResultSceneView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText; // スコアテキスト
    [SerializeField]
    private TextMeshProUGUI _timeText; // タイムテキスト
    [SerializeField]
    private TextMeshProUGUI _time; // タイム値テキスト
    [SerializeField]
    private TextMeshProUGUI _percentageText; // パーセンテージテキスト
    [SerializeField]
    private TextMeshProUGUI _percentage; // パーセンテージ値テキスト
    [SerializeField]
    private TextMeshProUGUI _clearRankText; // クリアランクテキスト

    [SerializeField]
    private Image _resultTitle; // 結果タイトル
    [SerializeField]
    private Sprite _winSprite; // 勝利時のスプライト
    [SerializeField]
    private Sprite _loseSprite; // 敗北時のスプライト

    [SerializeField]
    private float _sScore = 100; // Sランクのスコア閾値
    [SerializeField]
    private float _aScore = 75; // Aランクのスコア閾値
    [SerializeField]
    private float _bScore = 50; // Bランクのスコア閾値

    [SerializeField]
    private float _sTime = 30; // Sランクのタイム閾値
    [SerializeField]
    private float _aTime = 90; // Aランクのタイム閾値
    [SerializeField]
    private float _bTime = 120; // Bランクのタイム閾値

    [SerializeField]
    private Color _sColor = Color.yellow; // Sランクの色
    [SerializeField]
    private Color _aColor = Color.green; // Aランクの色
    [SerializeField]
    private Color _bColor = Color.blue; // Bランクの色
    [SerializeField]
    private Color _cColor = Color.red; // Cランクの色
    [SerializeField]
    private Color _dColor = Color.gray; // Dランクの色

    private bool isWin = false; // 勝敗のフラグ

    /// <summary>
    /// クリアランクの定義
    /// </summary>
    private enum ClearRank
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
    private async void Start()
    {
        isWin = PlayerPrefs.GetInt("IsWin", 0) != 0;
        SetLabelTexts();
        SetScoreAndPercentageTexts();
        SetResultTitle();

        if (isWin)
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.Clear);
            try
            {
                await UniTask.Delay(5000, cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            catch (System.OperationCanceledException)
            {
                return;
            }
        }
        else
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.GameOver);
            try
            {
                await UniTask.Delay(3000, cancellationToken: this.GetCancellationTokenOnDestroy());
            }
            catch (System.OperationCanceledException)
            {
                return;
            }
        }
        if(SceneManager.GetActiveScene().name == "ResultScene")
        {
            SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title);
        } 
    }

    /// <summary>
    /// 勝敗に応じたテキストを設定します。
    /// </summary>
    private void SetLabelTexts()
    {
        _timeText.text = "Time";
        _percentageText.text = isWin ? "Parry" : "HP";
    }

    /// <summary>
    /// スコアとパーセンテージテキストを設定します。
    /// </summary>
    private void SetScoreAndPercentageTexts()
    {
        _time.text = PlayerPrefs.GetFloat("Time", 0).ToString("F2");
        var percentage = CalculatePercentage(
                    isWin ? PlayerPrefs.GetInt("EnemyAttackCount", 100) : PlayerPrefs.GetInt("MaxHP", 100),
                    isWin ? PlayerPrefs.GetInt("ParryCount", 1) : PlayerPrefs.GetInt("CurrentHP", 1)
                    );
        _percentage.text = percentage.ToString("F2") + "%";
        SetClearRank(CalculateClearRank(isWin, percentage, PlayerPrefs.GetFloat("Time", 0)));
    }

    private void SetResultTitle()
    {
        _resultTitle.sprite = isWin ? _winSprite : _loseSprite;
    }

    /// <summary>
    /// パーセンテージを計算します。
    /// </summary>
    /// <param name="max">最大値。</param>
    /// <param name="current">現在値。</param>
    /// <returns>計算されたパーセンテージ。</returns>
    private float CalculatePercentage(int max, int current)
    {
        return (float)current / max * 100;
    }

    /// <summary>
    /// クリアランクを設定します。
    /// </summary>
    /// <param name="clearRank">クリアランク。</param>
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

    /// <summary>
    /// クリアランクを計算します。
    /// </summary>
    /// <param name="isWin">勝敗のフラグ。</param>
    /// <param name="percentage">パーセンテージ。</param>
    /// <param name="time">時間。</param>
    /// <returns>計算されたクリアランク。</returns>
    private ClearRank CalculateClearRank(bool isWin, float percentage, float time)
    {
        return isWin switch
        {
            true => DetermineClearRank(percentage, time),
            false => ClearRank.D,
        };
    }

    /// <summary>
    /// クリアランクを判定します。
    /// </summary>
    /// <param name="percentage">パーセンテージ。</param>
    /// <param name="time">時間。</param>
    /// <returns>判定されたクリアランク。</returns>
    private ClearRank DetermineClearRank(float percentage, float time)
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
