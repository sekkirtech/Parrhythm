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
    /// �������������s���܂��B
    /// </summary>
    private void Start()
    {
        _textMeshPro.text = "Stage" + (_stageNum + 1).ToString();
    }

    /// <summary>
    /// �{�^���������ꂽ�Ƃ��̏������s���܂��B
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
