using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class EnemyNoteManager : MonoBehaviour
{
    //１LPB経過する時間
    [SerializeField] private float BeatSplit = 0;
    //次の攻撃が何LPB目か
    private float NextBeat = 0;
    //次の攻撃が何タイプか
    private int NextAttackType = 4;
    //準備ができたか
    public bool EnemyNoteManagerFix=false;
    [SerializeField] public int[] NotesNum;//攻撃タイミング格納
    [SerializeField] public int[] AttackType;//攻撃手段格納
    public int BPM;
    public int LPB;//一伯間に何分割されているか


    [SerializeField]MainGameManager mainGameManager;

    [SerializeField] ScoreBox scoreData;

    [Serializable]
    public class InputJson
    {
        public Notes[] notes;
        //譜面のBPM
        public int BPM;
    }

    [Serializable]
    public class Notes
    {
        //何拍目に発生か
        public int num;
        //タイプ
        public int block;
        //一拍間の分割
        public int LPB;
    }


    void Awake()
    {
        PlayerPrefs.SetInt("StageNum", 0);
        if (scoreData == null)
        {
            Debug.LogError("ScoreDataがアタッチされてない");
        }
        MusicReading();
    }

    /// <summary>
    /// 譜面の読み込み
    /// </summary>
    void MusicReading()
    {
        //ステージ番号取得
        int StageNum = PlayerPrefs.GetInt("StageNum", 0);

        //jsonファイルが格納されてる場所のパス取得
        string inputString = scoreData.GetListInScore(StageNum).GetScore().ToString();
        Debug.Log(inputString);
        //jsonファイル取得
        InputJson inputJson = JsonUtility.FromJson<InputJson>(inputString);

        //サイズ格納
        NotesNum = new int[inputJson.notes.Length];
        AttackType = new int[inputJson.notes.Length];
        //情報格納
        BPM = inputJson.BPM;
        LPB = inputJson.notes[0].LPB;

        for (int i = 0; i < inputJson.notes.Length; i++)
        {
            //攻撃タイミング格納
            NotesNum[i] = inputJson.notes[i].num;
            //攻撃手段格納
            AttackType[i] = inputJson.notes[i].block;
        }
        BeatSplit = (float)60/(float)BPM/(float)LPB;

        Debug.Log(BeatSplit);
    }

    void NextAttack()
    {
        NextBeat = NotesNum[mainGameManager.AttackCount];
        NextAttackType = AttackType[mainGameManager.AttackCount];
    }


}
