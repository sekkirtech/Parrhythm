using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="NotesScore/ScoreBox")]
public class ScoreBox : ScriptableObject
{
    public List<ScoreInfoList> scoreInfoList = new List<ScoreInfoList>();


    //���X�g�S���Ăяo��
    public List<ScoreInfoList> GetScoreInfoLists() 
    { 
        return scoreInfoList; 
    }

    //�w���Փx�Ăяo��
    public ScoreInfoList GetListInScore(int scoreId)
    {
        return scoreInfoList[scoreId];
    }

}

[System.Serializable]
public class ScoreInfoList
{
    [SerializeField, Header("json�t�@�C���̃p�X")] public TextAsset ScorePath;
    [SerializeField, Header("wav�t�@�C��")] public AudioClip MusicClip;
    [SerializeField, Header("����")] public float MusicVolume;

    //�����擾
    public AudioClip GetClip()
    {
        return MusicClip;
    }
    //���ʎ擾
    public TextAsset GetScore()
    {
        return ScorePath;
    }
    //���ʎ擾
    public float GetVolume()
    {
        return MusicVolume;
    }
}