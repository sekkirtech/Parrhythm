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

    private void Start()
    {
        _textMeshPro.text = "Stage" + (_stageNum + 1).ToString();
    }

    override public void OnNext()
    {
        PlayerPrefs.SetInt("StageNum", _stageNum);
        FadeManager.Instance.LoadScene("MainScene", 1.0f);
        Debug.Log("StageNum:" + _stageNum);
        //シーン遷移処理
        //SceneManager.LoadScene("GameScene");
    }
}
