using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Ql(κόΟ)Fhttps://qiita.com/tsg4gf/items/64ca86bfe8d7c13739f1


public class NotesGenerator : MonoBehaviour
{
    [SerializeField] ScoreBox scoreData;

    [Serializable]
    public class InputJson
    {
        public Notes[] notes;
        //ΚΜBPM
        public int BPM;
    }

    [Serializable]
    public class Notes
    {
        //½ΪΙ­Ά©
        public int num;
        //^Cv
        public int block;
        //κΤΜͺ
        public int LPB;
    }

    private int[] NotesNum;//U^C~Oi[
    private int[] AttackType;//Uθii[
    private int BPM;
    private int LPB;

    void Awake()
    {
        MusicReading();
    }

    /// <summary>
    /// ΚΜΗέέ
    /// </summary>
    void MusicReading()
    {
        //Xe[WΤζΎ
        int StageNum = PlayerPrefs.GetInt("StageNum", 0);

        //jsont@Cͺi[³κΔικΜpXζΎ
        string inputString = scoreData.GetListInScore(StageNum).GetScorePath();
        //jsont@CζΎ
        InputJson inputJson = JsonUtility.FromJson<InputJson>(inputString);

        //TCYi[
        NotesNum = new int[inputJson.notes.Length];
        AttackType = new int[inputJson.notes.Length];
        //ξρi[
        BPM = inputJson.BPM;
        LPB = inputJson.notes[0].LPB;

        for (int i = 0; i < inputJson.notes.Length; i++)
        {
            //U^C~Oi[
            NotesNum[i] = inputJson.notes[i].num;
            //Uθii[
            AttackType[i] = inputJson.notes[i].block;
        }
    }
}
