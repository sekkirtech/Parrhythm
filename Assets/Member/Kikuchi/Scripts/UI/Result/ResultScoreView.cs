using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ���ʃV�[���̃r���[���Ǘ�����N���X
/// </summary>
public class ResultSceneView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText; // �X�R�A�e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _timeText; // �^�C���e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _time; // �^�C���l�e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _percentageText; // �p�[�Z���e�[�W�e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _percentage; // �p�[�Z���e�[�W�l�e�L�X�g
    [SerializeField]
    private TextMeshProUGUI _clearRankText; // �N���A�����N�e�L�X�g

    [SerializeField]
    private Image _resultTitle; // ���ʃ^�C�g��
    [SerializeField]
    private Sprite _winSprite; // �������̃X�v���C�g
    [SerializeField]
    private Sprite _loseSprite; // �s�k���̃X�v���C�g

    [SerializeField]
    private float _sScore = 100; // S�����N�̃X�R�A臒l
    [SerializeField]
    private float _aScore = 75; // A�����N�̃X�R�A臒l
    [SerializeField]
    private float _bScore = 50; // B�����N�̃X�R�A臒l

    [SerializeField]
    private float _sTime = 30; // S�����N�̃^�C��臒l
    [SerializeField]
    private float _aTime = 90; // A�����N�̃^�C��臒l
    [SerializeField]
    private float _bTime = 120; // B�����N�̃^�C��臒l

    [SerializeField]
    private Color _sColor = Color.yellow; // S�����N�̐F
    [SerializeField]
    private Color _aColor = Color.green; // A�����N�̐F
    [SerializeField]
    private Color _bColor = Color.blue; // B�����N�̐F
    [SerializeField]
    private Color _cColor = Color.red; // C�����N�̐F
    [SerializeField]
    private Color _dColor = Color.gray; // D�����N�̐F

    private bool isWin = false; // ���s�̃t���O

    /// <summary>
    /// �N���A�����N�̒�`
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
    /// ���������Ƀe�L�X�g��ݒ肵�܂��B
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
    /// ���s�ɉ������e�L�X�g��ݒ肵�܂��B
    /// </summary>
    private void SetLabelTexts()
    {
        _timeText.text = "Time";
        _percentageText.text = isWin ? "Parry" : "HP";
    }

    /// <summary>
    /// �X�R�A�ƃp�[�Z���e�[�W�e�L�X�g��ݒ肵�܂��B
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
    /// �p�[�Z���e�[�W���v�Z���܂��B
    /// </summary>
    /// <param name="max">�ő�l�B</param>
    /// <param name="current">���ݒl�B</param>
    /// <returns>�v�Z���ꂽ�p�[�Z���e�[�W�B</returns>
    private float CalculatePercentage(int max, int current)
    {
        return (float)current / max * 100;
    }

    /// <summary>
    /// �N���A�����N��ݒ肵�܂��B
    /// </summary>
    /// <param name="clearRank">�N���A�����N�B</param>
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
    /// �N���A�����N���v�Z���܂��B
    /// </summary>
    /// <param name="isWin">���s�̃t���O�B</param>
    /// <param name="percentage">�p�[�Z���e�[�W�B</param>
    /// <param name="time">���ԁB</param>
    /// <returns>�v�Z���ꂽ�N���A�����N�B</returns>
    private ClearRank CalculateClearRank(bool isWin, float percentage, float time)
    {
        return isWin switch
        {
            true => DetermineClearRank(percentage, time),
            false => ClearRank.D,
        };
    }

    /// <summary>
    /// �N���A�����N�𔻒肵�܂��B
    /// </summary>
    /// <param name="percentage">�p�[�Z���e�[�W�B</param>
    /// <param name="time">���ԁB</param>
    /// <returns>���肳�ꂽ�N���A�����N�B</returns>
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
