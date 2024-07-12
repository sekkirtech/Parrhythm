using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class EnemyNoteManager : NotesGenerator
{
    //‚PLPBŒo‰ß‚·‚éŠÔ
    private float BeatSplit = 0;
    //Ÿ‚ÌUŒ‚‚ª‰½LPB–Ú‚©
    private float NextBeat = 0;
    //Ÿ‚ÌUŒ‚‚ª‰½ƒ^ƒCƒv‚©
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
