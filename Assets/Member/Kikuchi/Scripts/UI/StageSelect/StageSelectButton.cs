using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageSelectButton : ButtonUIUtil
{
    [SerializeField]
    private int _stageNum = 0;
    [SerializeField]
    private TextMeshProUGUI _textMeshPro;

    /// <summary>
    /// 初期化処理を行います。
    /// </summary>
    private void Start()
    {
        _textMeshPro.text = "Stage" + (_stageNum + 1).ToString();
    }

    /// <summary>
    /// ボタンが押されたときの処理を行います。
    /// </summary>
    public override void OnNext()
    {
        PlayerPrefs.SetInt("StageNum", _stageNum);
        var flag = FadeManager.Instance.LoadScene("MainScene", 1.0f);
        if(flag)
        {
            SoundManager.Instance.StopBGMAfterSeconds(1.0f);
        }
    }
}
