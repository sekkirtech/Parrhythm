using System;
using UnityEngine;

public class EnemyNoteManager : MonoBehaviour
{
    //�PLPB�o�߂��鎞��
    [SerializeField] private float BeatSplit = 0;
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




    private float ReadSpan = 0.01f;//���b���ƂɎ��s���邩
    private float nowTime;// ���y�̍Đ�����Ă��鎞��
    private int beatNum;// ���̔���
    private int beatCount;// json�z��p(����)�̃J�E���g
    private bool isBeat;// �r�[�g��ł��Ă��邩(�����̃^�C�~���O)

    void Awake()
    {
        if (scoreData == null)
        {
            Debug.LogError("ScoreData���A�^�b�`����ĂȂ�");
        }
        if (mainGameManager == null)
        {
            Debug.LogError("MainGameManager���A�^�b�`����Ă��Ȃ� byEnemyNoteManager");
        }

        //�ǂݍ���
        MusicReading();
        //��莞�Ԍ��moveSpan�Ԋu�Ŏw��֐������s�i�ڐݕK�{�j
        InvokeRepeating("EnemyAttackIns", 3f, ReadSpan);
    }


    /// <summary>
    /// ���ʏ�̎��ԂƃQ�[���̎��Ԃ̃J�E���g�Ɛ���
    /// </summary>
    void GetScoreTime()
    {
        //���̉��y�̎��Ԃ̎擾
        nowTime += ReadSpan;

        //�m�[�c���Ȃ��Ȃ�I��
        if (beatCount > NotesNum.Length) return;

        //�y����łǂ����̎擾
        beatNum = (int)(nowTime * BPM / 60 * LPB);
    }

    /// <summary>
    /// �U���^�C�v�̓ǂݎ��
    /// </summary>
    void EnemyAttackIns()
    {
        GetScoreTime();

        //�J�E���g�̈�v��isBeat��true��
        if (beatCount < NotesNum.Length)
        {
            isBeat = (NotesNum[beatCount] == beatNum);
        }

        //�����̃^�C�~���O�Ȃ�
        if (isBeat)
        {
            //Type0��BGM�Đ�
            if (AttackType[beatCount] == 0)
            {
                //BGM�Đ�
                audioSource.Play();
            }
            else //0�ȊO�̎��A�^�b�N�֐��Ƀ^�C�v��n���Ď��s
            {
                StartCoroutine(mainGameManager.EnemmyAttack(AttackType[beatCount], (float)60 / (float)BPM));
            }


            beatCount++;
            isBeat = false;

        }
    }

    /*
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
        int StageNum = PlayerPrefs.GetInt("StageNum",100);

        if (StageNum == 100)
        {
            Debug.LogError("�X�e�[�W�i���o�[���i�[����ĂȂ�");
        }
        else
        {
            Debug.Log("�X�e�[�W�i���o�[�i�[��");
        }

        //json�t�@�C�����i�[����Ă�ꏊ�̃p�X�擾
        string inputString = scoreData.GetListInScore(StageNum).GetScore().ToString();
        Debug.Log(inputString);
        //json�t�@�C���擾
        InputJson inputJson = JsonUtility.FromJson<InputJson>(inputString);

        //�e�T�C�Y�i�[
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
}
