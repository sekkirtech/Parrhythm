using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class SelManagerScript : MonoBehaviour
{
    private int index = 0;

    
    private void Start()
    {
        ControllerManager.Instance.RStickObservable.Subscribe(_ => ChaneIndex(_.x)).AddTo(this.gameObject);
    }

    private void ChaneIndex(float namber)
    {
        if (namber > 0)
        {
            index++;
        }
        else
        {
            index--;
        }
    }

}