using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmAudioSource;
    public AudioSource BgmAudioSource => bgmAudioSource;
    [SerializeField] List<AudioSource> seAudioSources; // 複数のSE再生用のオーディオソース

    [SerializeField] List<BGMSoundData> bgmSoundDatas;
    [SerializeField] List<SESoundData> seSoundDatas;

    public float masterVolume = 1;
    public float bgmMasterVolume = 1;
    public float seMasterVolume = 1;

    public static SoundManager Instance { get; private set; }

    private Dictionary<SESoundData.SE, float> lastPlayedTime = new Dictionary<SESoundData.SE, float>();

    private Tween _fadeTween;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 初期化時にSEオーディオソースをプールに追加
        seAudioSources = new List<AudioSource>();
        for (int i = 0; i < 5; i++) // 初期値として5つのSEオーディオソースを作成
        {
            AudioSource newSEAudioSource = gameObject.AddComponent<AudioSource>();
            seAudioSources.Add(newSEAudioSource);
        }
    }

    public void PlayBGM(BGMSoundData.BGM bgm)
    {
        BGMSoundData data = bgmSoundDatas.Find(data => data.bgm == bgm);
        bgmAudioSource.clip = data.audioClip;
        bgmAudioSource.volume = data.volume * bgmMasterVolume * masterVolume;
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void StopBGMAfterSeconds(float seconds)
    {
        StartCoroutine(StopBGMAfterSecondsCoroutine(seconds));
    }

    private IEnumerator StopBGMAfterSecondsCoroutine(float seconds)
    {
        _fadeTween = bgmAudioSource.DOFade(0, seconds);
        yield return new WaitForSeconds(seconds);
        StopBGM();
    }

    public void PlaySE(SESoundData.SE se)
    {
        SESoundData data = seSoundDatas.Find(data => data.se == se);
        if (!CanPlaySE(se))
        {
            return;
        }

        AudioSource seAudioSource = GetAvailableSEAudioSource();
        seAudioSource.volume = data.volume * seMasterVolume * masterVolume;
        seAudioSource.PlayOneShot(data.audioClip);
        lastPlayedTime[se] = Time.time;
    }

    public void StopSE(SESoundData.SE se)
    {
        foreach (AudioSource audioSource in seAudioSources)
        {
            if (audioSource.isPlaying && audioSource.clip == seSoundDatas.Find(data => data.se == se).audioClip)
            {
                audioSource.Stop();
                break; // 最初に見つけた対象のSEを停止してループを抜ける
            }
        }
    }

    public void StopSEAfterSeconds(SESoundData.SE se, float seconds)
    {
        StartCoroutine(StopSEAfterSecondsCoroutine(se, seconds));
    }

    private IEnumerator StopSEAfterSecondsCoroutine(SESoundData.SE se, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StopSE(se);
    }

    public void StopAllSE()
    {
        foreach (AudioSource audioSource in seAudioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    public void StopAllSEAfterSeconds(float seconds)
    {
        StartCoroutine(StopAllSEAfterSecondsCoroutine(seconds));
    }

    private IEnumerator StopAllSEAfterSecondsCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StopAllSE();
    }

    private bool CanPlaySE(SESoundData.SE se)
    {
        if (lastPlayedTime.ContainsKey(se))
        {
            float currentTime = Time.time;
            float timeSinceLastPlay = currentTime - lastPlayedTime[se];
            if (timeSinceLastPlay < 0.2f) // 高速再生を防ぐための時間間隔
            {
                return false;
            }
        }
        return true;
    }

    private AudioSource GetAvailableSEAudioSource()
    {
        foreach (AudioSource audioSource in seAudioSources)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        // すべてのオーディオソースが再生中の場合は、新しいオーディオソースを作成して追加
        AudioSource newSEAudioSource = gameObject.AddComponent<AudioSource>();
        seAudioSources.Add(newSEAudioSource);
        return newSEAudioSource;
    }

    private void OnDestroy()
    {
        if (_fadeTween != null)
        {
            _fadeTween?.Kill();
        }
    }
}

[System.Serializable]
public class BGMSoundData
{
    public enum BGM
    {
        //ラベル
        Title,
        InGame,
        Result,
    }

    public BGM bgm;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}

[System.Serializable]
public class SESoundData
{
    public enum SE
    {
        //ラベル
        Clear,
        GameOver,
        Start,
        Select,
        CursorMove,
    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}

