using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class ResultSceneView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText; // ����Image�\��
    [SerializeField]
    private TextMeshProUGUI _timeText;
    [SerializeField]
    private TextMeshProUGUI _time;
    [SerializeField]
    private TextMeshProUGUI _percentageText;
    [SerializeField]
    private TextMeshProUGUI _percentage;

    /// <summary>
    /// ���������Ƀe�L�X�g��ݒ肵�܂��B
    /// </summary>
    private void Start()
    {
        SetExpoText();
        SetScoreText();
    }

    /// <summary>
    /// ���s�ɉ������e�L�X�g��ݒ肵�܂��B
    /// </summary>
    private void SetExpoText()
    {
        bool isWin = PlayerPrefs.GetInt("IsWin", 0) != 0;
        _timeText.text = "Time";
        _percentageText.text = isWin ? "Parry" : "HP";
    }

    /// <summary>
    /// �X�R�A�e�L�X�g��ݒ肵�܂��B
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
    /// �p�[�Z���e�[�W���v�Z���܂��B
    /// </summary>
    /// <param name="max">�ő�l�B</param>
    /// <param name="current">���ݒl�B</param>
    /// <returns>�v�Z���ꂽ�p�[�Z���e�[�W�B</returns>
    private float CalcPercentage(int max, int current)
    {
        float percentage = (float)current / max * 100;
        Debug.Log(percentage);
        return percentage;
    }
}
