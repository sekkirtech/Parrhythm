using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectBGMCall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SoundManager.Instance.BgmAudioSource.isPlaying == false)
        {
            SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title);
        }
    }

   
}
