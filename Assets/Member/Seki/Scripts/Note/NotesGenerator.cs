using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Ql(ˆê•”‰ü•Ï)Fhttps://qiita.com/tsg4gf/items/64ca86bfe8d7c13739f1


public class NotesGenerator : MonoBehaviour
{
    [SerializeField] ScoreBox scoreData;

    [Serializable]
    public class InputJson
    {
        public Notes[] notes;
        //•ˆ–Ê‚ÌBPM
        public int BPM;
    }

    [Serializable]
    public class Notes
    {
        //‰½”–Ú‚É”­¶‚©
        public int num;
        //ƒ^ƒCƒv
        public int block;
        //ˆê”ŠÔ‚Ì•ªŠ„
        public int LPB;
    }

    private int[] NotesNum;//UŒ‚ƒ^ƒCƒ~ƒ“ƒOŠi”[
    private int[] AttackType;//UŒ‚è’iŠi”[
    private int BPM;
    private int LPB;

    void Awake()
    {
        MusicReading();
    }

    /// <summary>
    /// •ˆ–Ê‚Ì“Ç‚İ‚İ
    /// </summary>
    void MusicReading()
    {
        int num = PlayerPrefs.GetInt("StageNum", 0);

        string inputString = scoreData.GetListInScore(num).GetScorePath();
        InputJson inputJson = JsonUtility.FromJson<InputJson>(inputString);

        NotesNum = new int[inputJson.notes.Length];
        AttackType = new int[inputJson.notes.Length];
        BPM = inputJson.BPM;
        LPB = inputJson.notes[0].LPB;

        for (int i = 0; i < inputJson.notes.Length; i++)
        {
            //UŒ‚ƒ^ƒCƒ~ƒ“ƒOŠi”[
            NotesNum[i] = inputJson.notes[i].num;
            //UŒ‚è’iŠi”[
            AttackType[i] = inputJson.notes[i].block;
        }
    }
}
