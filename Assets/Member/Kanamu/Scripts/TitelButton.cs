using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.UI;
using System.Linq;

public class TitelButton : MonoBehaviour
{
    [SerializeField]
    private Image _buttonTextRTF;// 菊池追記　ボタンのテキスト
    [SerializeField]
    private GameObject _licensePanel;// 菊池追記　ライセンスパネル
    [SerializeField]
    private Scrollbar _scrollbar;

    [SerializeField]
    private float _fadeTime = 1.5f;
    [SerializeField]
    private float _fadeValue = 0f;

    [SerializeField]
    private List<GameObject> Buttons = new List<GameObject>();

    private List<TitelButtonUtil> titleButtonUtils = new List<TitelButtonUtil>();

    private Vector2 _tempRStickValue = Vector2.zero;

    private CompositeDisposable disposables = new CompositeDisposable();

    private Tween _fadeTween;

    private int index = 0;

    private int currentIndex = 0;

    private float mooveCD = 0f;


    private void Start()
    {
        foreach(var x in Buttons)
        {
            titleButtonUtils.Add(x.GetComponent<TitelButtonUtil>());
        }
        //ControllerManager.Instance.SouthButtonObservable.Subscribe(x =>
        //{
        //    FadeManager.Instance.LoadScene("StageSelect", 1.0f);
        //    SoundManager.Instance.PlaySE(SESoundData.SE.Select);
        //}).AddTo(disposables); // 菊池修正　xボタンに

        //ControllerManager.Instance.TouchPadObservable.Subscribe(x =>
        //{
        //    if (_licensePanel.activeSelf)
        //    {
        //        _licensePanel.SetActive(false);
        //        SoundManager.Instance.PlaySE(SESoundData.SE.Select);
        //    }
        //    else
        //    {
        //        _licensePanel.SetActive(true);
        //        SoundManager.Instance.PlaySE(SESoundData.SE.Select);
        //    }
        //}).AddTo(disposables); // 菊池追記　ライセンス表記の追加

        ControllerManager.Instance.SouthButtonObservable.Subscribe(x =>
        {
            titleButtonUtils[currentIndex].OnNext();
        }).AddTo(disposables);

        ControllerManager.Instance.RStickObservable.Subscribe(x =>
        {
            Debug.Log(x);
            _tempRStickValue = x;
        }).AddTo(disposables); // 菊池追記　右スティックで遊び方へ

        ControllerManager.Instance.RStickObservable.Subscribe(x =>
        {
            if (Time.time - mooveCD < 0.2f) return;
            if (x.x > 0) index++;
            else index--;
            mooveCD = Time.time;
        }).AddTo(disposables);

        ControllerManager.Instance.LStickObservable.Subscribe(x =>
        {
            if (Time.time - mooveCD < 0.2f) return;
            if (x.x > 0) index++;
            else index--;
            mooveCD = Time.time;
        }).AddTo(disposables);

        this.ObserveEveryValueChanged(x => x.index).Subscribe(x =>
        {
            if ((index + 2 ) % 2 == 0) currentIndex = 0;
            else if ((index + 2 ) % 2 == 1) currentIndex = 1;
        }).AddTo(disposables);
    
        SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title);
        _fadeTween = _buttonTextRTF.DOFade(_fadeValue, _fadeTime).SetLoops(-1, LoopType.Yoyo);// 菊池追記　ボタンのテキストを点滅させる
    }

    private void Update()
    {
        if(_tempRStickValue.y > 0.5f)
        {
            if(_scrollbar.value >= 1.0f) return;
            _scrollbar.value += 0.1f * Time.deltaTime;
        }
        else if(_tempRStickValue.y < -0.5f)
        {
            if(_scrollbar.value <= 0.0f) return;
            _scrollbar.value -= 0.1f * Time.deltaTime; 
        }

        var y = titleButtonUtils.Select((x, i) => new { Index = i, Value = x }).ToList();
        foreach (var item in y)
        {
            if (item.Index == currentIndex) item.Value.isSelect = true;
            else item.Value.isSelect = false;
        }
    }

    private void OnDestroy()
    {
        disposables.Dispose();
        _fadeTween?.Kill();
    }

}
