using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : UIUtility
{

    [SerializeField]
    private int _stageNum = 0;
    override public void OnNext()
    {
        DataManager.Instance.StageNum = _stageNum;
        Debug.Log("StageNum:" + DataManager.Instance.StageNum);
        //ƒV[ƒ“‘JˆÚˆ—
        //SceneManager.LoadScene("GameScene");
    }
}
