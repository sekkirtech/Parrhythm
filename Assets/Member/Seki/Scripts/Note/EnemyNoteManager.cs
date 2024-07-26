using System;
using UnityEngine;

public class EnemyNoteManager : MonoBehaviour
{
    //�PLPB�o�߂��鎞��
    [SerializeField] private float BeatSplit = 0;
    //���̍U������LPB�ڂ�
    [SerializeField]private float NextBeat = 0;
    //���̍U�������^�C�v��
    [SerializeField] private int NextAttackType = 4;
    //�������ł�����
    public bool EnemyNoteManagerFix=false;
    [SerializeField] public int[] NotesNum;//�U���^�C�~���O�i�[
    [SerializeField] public int[] AttackType;//�U����i�i�[
    public int BPM;
    public int LPB;//�ꔌ�Ԃɉ���������Ă��邩

    private int ScoreLegth;



    [SerializeField]MainGameManager mainGameManager;

    [SerializeField] ScoreBox scoreData;

    [SerializeField] float AttackTime;
    [SerializeField] AudioSource audioSource;

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



    [SerializeField]
    private GameObject notesPre;

    private float moveSpan = 0.01f;
    private float nowTime;// ���y�̍Đ�����Ă��鎞��
    private int beatNum;// ���̔���
    private int beatCount;// json�z��p(����)�̃J�E���g
    private bool isBeat;// �r�[�g��ł��Ă��邩(�����̃^�C�~���O)

    void Awake()
    {
        PlayerPrefs.SetInt("StageNum", 0);
        if (scoreData == null)
        {
            Debug.LogError("ScoreData���A�^�b�`����ĂȂ�");
        }
        if (mainGameManager == null)
        {
            Debug.LogError("MainGameManager���A�^�b�`����Ă��Ȃ� byEnemyNoteManager");
        }
        MusicReading();
        InvokeRepeating("NotesIns", 10f, moveSpan);
    }


    /// <summary>
    /// ���ʏ�̎��ԂƃQ�[���̎��Ԃ̃J�E���g�Ɛ���
    /// </summary>
    void GetScoreTime()
    {
        //���̉��y�̎��Ԃ̎擾
        nowTime += moveSpan; //(1)

        //�m�[�c�������Ȃ����珈���I��
        if (beatCount > NotesNum.Length) return;

        //�y����łǂ����̎擾
        beatNum = (int)(nowTime * BPM / 60 * LPB); //(2)
    }

    /// <summary>
    /// �m�[�c�𐶐�����
    /// </summary>
    void NotesIns()
    {
        GetScoreTime();

        //json��ł̃J�E���g�Ɗy����ł̃J�E���g�̈�v
        if (beatCount < NotesNum.Length)
        {
            isBeat = (NotesNum[beatCount] == beatNum); //(3)
        }

        //�����̃^�C�~���O�Ȃ�
        if (isBeat)
        {
            //�m�[�c0�̐���
            if (AttackType[beatCount] == 0)
            {
                audioSource.Play();
            }

            //�m�[�c1�̐���
            if (AttackType[beatCount] == 1)
            {
                StartCoroutine(mainGameManager.EnemmyAttack(1,(float)60/(float)BPM));
            }
            if (AttackType[beatCount] == 2)
            {
                StartCoroutine(mainGameManager.EnemmyAttack(2, (float)60 / (float)BPM));
            }
            if (AttackType[beatCount] == 3)
            {
                StartCoroutine(mainGameManager.EnemmyAttack(3, (float)60 / (float)BPM));
            }

            beatCount++; //(5)
            isBeat = false;

        }
    }

    /*
      void Start()
       {
           PlayerPrefs.SetInt("StageNum", 0);
           if (scoreData == null)
           {
               Debug.LogError("ScoreData���A�^�b�`����ĂȂ�");
           }
           if (mainGameManager == null)
           {
               Debug.LogError("MainGameManager���A�^�b�`����Ă��Ȃ� byEnemyNoteManager");
           }
           MusicReading();
       }

    
       void Update()
       {
           if (EnemyNoteManagerFix)
           {
               AttackTime = (float)60 / (float)BPM / (float)LPB * (float)NextBeat;
               if (AttackTime <=mainGameManager.BattleTime)
               {
                   Debug.Log(AttackTime);
                   StartCoroutine(mainGameManager.EnemmyAttack(NextAttackType,(float)BeatSplit*LPB));
                   NextAttack();
               }
           }
       }*/



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
        ScoreLegth = NotesNum.Length;
        Debug.Log("ScoreLegth" + ScoreLegth);
        //NextAttack();
        EnemyNoteManagerFix = true;
    }

    /*void NextAttack()
    {
        mainGameManager.AttackCount++;
        if (mainGameManager.AttackCount > ScoreLegth+1)
        {
            Debug.Log("�I��");
            EnemyNoteManagerFix=false;
            return;
        }
        NextBeat = NotesNum[mainGameManager.AttackCount];
        NextAttackType = AttackType[mainGameManager.AttackCount-1];
    }*/
}
