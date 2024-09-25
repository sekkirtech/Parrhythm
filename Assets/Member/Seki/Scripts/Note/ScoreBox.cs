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
    [SerializeField, Header("jsonファイルのパス")] public TextAsset ScorePath;
    [SerializeField, Header("wavファイル")] public AudioClip MusicClip;
    [SerializeField, Header("音量")] public float MusicVolume;

    //音源取得
    public AudioClip GetClip()
    {
        return MusicClip;
    }
    //譜面取得
    public TextAsset GetScore()
    {
        return ScorePath;
    }
    //音量取得
    public float GetVolume()
    {
        return MusicVolume;
    }
}