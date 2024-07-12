using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class EnemyNoteManager : NotesGenerator
{
    //�PLPB�o�߂��鎞��
    private float BeatSplit = 0;
    //���̍U������LPB�ڂ�
    private float NextBeat = 0;
    //���̍U�������^�C�v��
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
