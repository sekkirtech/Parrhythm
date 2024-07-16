using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class EnemyNoteManager : MonoBehaviour
{
    //�PLPB�o�߂��鎞��
    [SerializeField] private float BeatSplit = 0;
    //���̍U������LPB�ڂ�
    private float NextBeat = 0;
    //���̍U�������^�C�v��
    private int NextAttackType = 4;
    //�������ł�����
    public bool EnemyNoteManagerFix=false;
    [SerializeField] public int[] NotesNum;//�U���^�C�~���O�i�[
    [SerializeField] public int[] AttackType;//�U����i�i�[
    public int BPM;
    public int LPB;//�ꔌ�Ԃɉ���������Ă��邩


    [SerializeField]MainGameManager mainGameManager;

    [SerializeField] ScoreBox scoreData;

    [Serializable]
    public class InputJson
    {
        public Notes[] notes;
        //���ʂ�BPM
        public int BPM;
    }

    [Serializable]
    public class Notes
    {
        //�����ڂɔ�����
        public int num;
        //�^�C�v
        public int block;
        //�ꔏ�Ԃ̕���
        public int LPB;
    }


    void Awake()
    {
        PlayerPrefs.SetInt("StageNum", 0);
        if (scoreData == null)
        {
            Debug.LogError("ScoreData���A�^�b�`����ĂȂ�");
        }
        MusicReading();
    }

    /// <summary>
    /// ���ʂ̓ǂݍ���
    /// </summary>
    void MusicReading()
    {
        //�X�e�[�W�ԍ��擾
        int StageNum = PlayerPrefs.GetInt("StageNum", 0);

        //json�t�@�C�����i�[����Ă�ꏊ�̃p�X�擾
        string inputString = scoreData.GetListInScore(StageNum).GetScore().ToString();
        Debug.Log(inputString);
        //json�t�@�C���擾
        InputJson inputJson = JsonUtility.FromJson<InputJson>(inputString);

        //�T�C�Y�i�[
        NotesNum = new int[inputJson.notes.Length];
        AttackType = new int[inputJson.notes.Length];
        //���i�[
        BPM = inputJson.BPM;
        LPB = inputJson.notes[0].LPB;

        for (int i = 0; i < inputJson.notes.Length; i++)
        {
            //�U���^�C�~���O�i�[
            NotesNum[i] = inputJson.notes[i].num;
            //�U����i�i�[
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
