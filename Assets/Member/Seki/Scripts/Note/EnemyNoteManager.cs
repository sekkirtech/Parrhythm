using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class EnemyNoteManager : NotesGenerator
{
    //１LPB経過する時間
    private float BeatSplit = 0;
    //次の攻撃が何LPB目か
    private float NextBeat = 0;
    //次の攻撃が何タイプか
    private int NextAttackType = 4;


    [SerializeField]MainGameManager mainGameManager;

    void Awake()
    {
        BeatSplit = 60 / BPM / LPB;
    }

    void Update()
    {
        
    }

    void NextAttack()
    {
        NextBeat = NotesNum[mainGameManager.AttackCount];
        NextAttackType = AttackType[mainGameManager.AttackCount];
    }


}
