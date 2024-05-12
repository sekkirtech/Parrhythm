using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    protected override bool dontDestroyOnLoad => true;

    private int _score = 0;
    public int Score { get => _score; set => _score = value; }

    private int _stageNum = 0;
    public int StageNum { get => _stageNum; set => _stageNum = value; }

    override protected void Awake()
    {
        base.Awake();
    }
}
