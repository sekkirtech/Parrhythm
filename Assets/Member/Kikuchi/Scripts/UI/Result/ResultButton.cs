using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultButton : ButtonUIUtil
{
    [SerializeField]
    private string _sceneName;

    public override void OnNext()
    {
        FadeManager.Instance.LoadScene(_sceneName, 1.0f);
        if(_sceneName == "StageSelect")
        {
            if(SoundManager.Instance.BgmAudioSource.isPlaying == false)
            {
                SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title);
            }
        }
    }
}
