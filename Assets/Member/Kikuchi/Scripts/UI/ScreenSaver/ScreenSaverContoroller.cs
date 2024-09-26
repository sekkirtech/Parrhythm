using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
using UniRx;

public class ScreenSaverContoroller : MonoBehaviour
{
    private bool isActived = false;
    public bool IsActived => isActived;
    

    private VideoPlayer videoPlayer;

    [SerializeField]
    private VideoClip videoClip;
    [SerializeField]
    private GameObject screenSaver;

    private float timer;

    private float screenSaverTime = 30.0f;

    private void Awake() 
    {
        videoPlayer = screenSaver.GetComponent<VideoPlayer>();
        videoPlayer.clip = videoClip;
        videoPlayer.isLooping = true;
        videoPlayer.gameObject.SetActive(false);
    }

    private void Start()
    {
        ControllerManager.Instance.AnyButtonObservable.Subscribe(_ =>
        {
            timer = 0;
            if(isActived)
            {
                StopScreenSaver();
            }
        }).AddTo(this);
    }

    private void Update() 
    {
        timer += Time.deltaTime;
        if(timer > screenSaverTime && !isActived)
        {
            StartScreenSaver();
        }
        
    }

    public async void StartScreenSaver()
    {
        videoPlayer.Prepare();
        videoPlayer.gameObject.SetActive(true);
        try{
             await UniTask.WaitUntil(() => videoPlayer.isPrepared, cancellationToken: this.GetCancellationTokenOnDestroy());
        }catch(System.OperationCanceledException){
            Debug.Log("ScreenSaverContoroller:StartScreenSaver:UniTask.WaitUntil is canceled");
        }
        isActived = true;
        SoundManager.Instance.StopBGM();
        videoPlayer.Play();
    }

    public void StopScreenSaver()
    {
        isActived = false;
        videoPlayer.Stop();
        videoPlayer.targetTexture.Release();
        videoPlayer.gameObject.SetActive(false);
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title);
    }
}
