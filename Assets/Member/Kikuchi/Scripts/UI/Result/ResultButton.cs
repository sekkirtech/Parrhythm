using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultButton : ButtonUIUtil
{
    [SerializeField]
    private string _sceneName;

    public override void OnNext()
    {
        FadeManager.Instance.LoadScene(_sceneName, 0.5f);
    }
}
