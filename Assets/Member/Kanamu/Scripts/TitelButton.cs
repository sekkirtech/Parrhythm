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
    private GameObject _titleButton;
    [SerializeField]
    private GameObject _startButtons; 

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

    private bool _isFirstPress = false;


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
        

        ControllerManager.Instance.SouthButtonObservable
        .Where(_ => _isFirstPress)
        .Subscribe(x =>
        {
            titleButtonUtils[currentIndex].OnNext();
        }).AddTo(disposables);

        ControllerManager.Instance.RStickObservable
        .Where(_ => _isFirstPress)
        .Subscribe(x =>
        {
            Debug.Log(x);
            _tempRStickValue = x;
        }).AddTo(disposables); // 菊池追記　右スティックで遊び方へ

        ControllerManager.Instance.RStickObservable
        .Where(_ => _isFirstPress)
        .Subscribe(x =>
        {
            if (Time.time - mooveCD < 0.2f || Mathf.Abs(x.x) <= 0.2f) return;
            if (x.y > 0) index++;
            else index--;
            mooveCD = Time.time;
        }).AddTo(disposables);

        ControllerManager.Instance.LStickObservable
        .Where(_ => _isFirstPress)
        .Subscribe(x =>
        {
            if (Time.time - mooveCD < 0.2f || Mathf.Abs(x.x) <= 0.2f) return;
            if (x.y > 0) index++;
            else index--;
            mooveCD = Time.time;
        }).AddTo(disposables);

        ControllerManager.Instance.DPadObservable
        .Subscribe(x =>
        {
            if (Time.time - mooveCD < 0.2f || Mathf.Abs(x.x) <= 0.2f) return;
            if (x.y > 0) index++;
            else index--;
            mooveCD = Time.time;
        }).AddTo(disposables);

        this.ObserveEveryValueChanged(x => x.index)
        .Where(_ => _isFirstPress)
        .Subscribe(x =>
        {
            var absIndex = Mathf.Abs(index);
            if ((absIndex + 2 ) % 2 == 0) currentIndex = 0;
            else if ((absIndex + 2 ) % 2 == 1) currentIndex = 1;
            SoundManager.Instance.PlaySE(SESoundData.SE.CursorMove);
            Debug.Log(index + " :   " + currentIndex);
        }).AddTo(disposables);

        ControllerManager.Instance.SouthButtonObservable
        .Where(_ => !_isFirstPress)
        .Subscribe(x =>
        {
            _isFirstPress = true;
            _titleButton.SetActive(false);
            _startButtons.SetActive(true);
            SoundManager.Instance.PlaySE(SESoundData.SE.Select);
        }).AddTo(disposables);

        _startButtons.SetActive(false);
    
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
