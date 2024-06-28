using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Ql(êüÏ)Fhttps://qiita.com/tsg4gf/items/64ca86bfe8d7c13739f1


public class NotesGenerator : MonoBehaviour
{
    [SerializeField] ScoreBox scoreData;

    [Serializable]
    public class InputJson
    {
        public Notes[] notes;
        //ÊÌBPM
        public int BPM;
    }

    [Serializable]
    public class Notes
    {
        //½ÚÉ­¶©
        public int num;
        //^Cv
        public int block;
        //êÔÌª
        public int LPB;
    }

    private int[] NotesNum;//U^C~Oi[
    private int[] AttackType;//Uèii[
    private int BPM;
    private int LPB;

    void Awake()
    {
        MusicReading();
    }

    /// <summary>
    /// ÊÌÇÝÝ
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
            //U^C~Oi[
            NotesNum[i] = inputJson.notes[i].num;
            //Uèii[
            AttackType[i] = inputJson.notes[i].block;
        }
    }
}
