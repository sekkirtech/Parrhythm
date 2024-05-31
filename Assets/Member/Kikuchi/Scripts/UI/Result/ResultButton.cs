using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultButton : ButtonUIUtil
{
    [SerializeField]
    private string _sceneName;

    private string BGMStopScene = "MainScene";


    public override void OnNext()
    {
        if (_sceneName == BGMStopScene)
        {
            SoundManager.Instance.bgmAudioSource.Stop();//ŠÖ’Ç‰Á
        }
        SoundManager.Instance.PlaySE(SESoundData.SE.Streamer);//ŠÖ’Ç‰Á
        FadeManager.Instance.LoadScene(_sceneName, 1.0f);
    }
}
