using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using DG.Tweening;

public class TitelButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _buttonTextRTF;// 菊池追記　ボタンのテキスト

    [SerializeField]
    private float _fadeTime = 1.5f;
    [SerializeField]
    private float _fadeValue = 0f;

   private CompositeDisposable disposables = new CompositeDisposable();
    private void Start()
    {
        ControllerManager.Instance.AnyButtonObservable.Subscribe(x => FadeManager.Instance.LoadScene("StageSelect", 1.0f)).AddTo(disposables); // 菊池修正　xボタンからanyボタンに変更
        _buttonTextRTF.DOFade(_fadeValue, _fadeTime).SetLoops(-1, LoopType.Yoyo);// 菊池追記　ボタンのテキストを点滅させる
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }

}
