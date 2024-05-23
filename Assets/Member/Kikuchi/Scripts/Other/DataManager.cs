using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    protected override bool dontDestroyOnLoad => true;

    private int _time = 0;
    public int Time => _time;

    private int _stageNum = 0;
    public int StageNum => _stageNum;

    private int _enemyAttackCount = 0;
    public int EnemyAttackCount => _enemyAttackCount;

    private int _parryCount = 0;
    public int ParryCount => _parryCount;

    private int _maxHP = 0;
    public int MaxHP => _maxHP;

    private int _currentHP = 0;
    public int CurrentHP => _currentHP;

    override protected void Awake()
    {
        base.Awake();
    }

    public void Initialize()
    {
        _time = 0;
        _stageNum = 0;
        _enemyAttackCount = 0;
        _parryCount = 0;

    }

    public void SetStageNumData(int stageNum)
    {
        _stageNum = stageNum;
    }

    public void ResultDataSet(int time, int enemyAttackCount, int parryCount)
    {
        _time = time;
        _enemyAttackCount = enemyAttackCount;
        _parryCount = parryCount;
    }

    public void ResultDataSet(int time, int enemyAttackCount, int parryCount, int maxHP, int currentHP)
    {
        _time = time;
        _enemyAttackCount = enemyAttackCount;
        _parryCount = parryCount;
        _maxHP = maxHP;
        _currentHP = currentHP;
    }
    
}
