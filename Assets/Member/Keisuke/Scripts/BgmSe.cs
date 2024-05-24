using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmSe : MonoBehaviour
{
    [SerializeField]
    AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        //テスト用
        //SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title);
        //SoundManager.Instance.PlaySE(SESoundData.SE.Streamer2);
    }

    // Update is called once per frame
    void Update()
    {
        //SoundManager.Instance.PlaySE(SESoundData.SE.Streamer);
        
    }
}
