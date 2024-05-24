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
        FadeManager.Instance.LoadScene("MainScene", 0.5f);
        Debug.Log("StageNum:" + _stageNum);
        //ƒV[ƒ“‘JˆÚˆ—
        //SceneManager.LoadScene("GameScene");
    }
}
