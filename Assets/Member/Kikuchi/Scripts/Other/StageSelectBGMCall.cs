using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StageSelectBGMCall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SoundManager.Instance.BgmAudioSource.isPlaying == false)
        {
            SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title);
        }

        ControllerManager.Instance.StartButtonObservable.Subscribe(_ =>
        {
            SoundManager.Instance.StopBGM();
            FadeManager.Instance.LoadScene("TitleScene", 1.0f);
        });
    }

   
}
