using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TitelButton : MonoBehaviour
{

   private CompositeDisposable disposables = new CompositeDisposable();
    private void Start()
    {
        ControllerManager.Instance.SouthButtonObservable.Subscribe(x => FadeManager.Instance.LoadScene("StageSelect", 1.0f)).AddTo(disposables);
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }

}
