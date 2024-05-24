using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : UIUtility
{

    [SerializeField]
    private int _stageNum = 0;
    override public void OnNext()
    {
        PlayerPrefs.SetInt("StageNum", _stageNum);
        Debug.Log("StageNum:" + _stageNum);
        //シーン遷移処理
        //SceneManager.LoadScene("GameScene");
    }
}
