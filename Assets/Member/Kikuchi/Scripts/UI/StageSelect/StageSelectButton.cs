using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : ButtonUIUtil
{

    [SerializeField]
    private int _stageNum = 0;

    override public void OnNext()
    {
        PlayerPrefs.SetInt("StageNum", _stageNum);
        FadeManager.Instance.LoadScene("MainScene", 0.5f);
        Debug.Log("StageNum:" + _stageNum);
        //ƒV[ƒ“‘JˆÚˆ—
        //SceneManager.LoadScene("GameScene");
    }
}
