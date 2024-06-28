using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="NotesScore/ScoreBox")]
public class ScoreBox : ScriptableObject
{
    public List<ScoreInfoList> scoreInfoList = new List<ScoreInfoList>();


    //リスト全部呼び出し
    public List<ScoreInfoList> GetScoreInfoLists() 
    { 
        return scoreInfoList; 
    }

    //指定難易度呼び出し
    public ScoreInfoList GetListInScore(int scoreId)
    {
        return scoreInfoList[scoreId];
    }

}

[System.Serializable]
public class ScoreInfoList
{
    [SerializeField, Header("jsonファイルのパス")] public string ScorePath;
    [SerializeField, Header("wavファイル")] public AudioClip MusicClip;


    public AudioClip GetClip()
    {
        return MusicClip;
    }
    public string GetScorePath()
    {
        return ScorePath;
    }
}