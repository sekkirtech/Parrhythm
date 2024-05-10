using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UniRx;

public class Test : MonoBehaviour
{
    private ControllerManager controllerManager;

    private void Start()
    {
        controllerManager = ControllerManager.Instance;


        controllerManager.EastButtonObservable.Subscribe(_ => Debug.Log("EastButton"));
        controllerManager.LStickObservable.Subscribe(v => Debug.Log("LStick: " + v));
    }

    
}
