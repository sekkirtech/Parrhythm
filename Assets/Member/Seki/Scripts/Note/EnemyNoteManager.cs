using System;
using UnityEngine;

public class EnemyNoteManager : MonoBehaviour
{
    //１LPB経過する時間
    [SerializeField] private float BeatSplit = 0;
    //次の攻撃が何LPB目か
    [SerializeField]private float NextBeat = 0;
    //次の攻撃が何タイプか
    [SerializeField] private int NextAttackType = 4;
    //準備ができたか
    public bool EnemyNoteManagerFix=false;
    [SerializeField] public int[] NotesNum;//攻撃タイミング格納
    [SerializeField] public int[] AttackType;//攻撃手段格納
    public int BPM;
    public int LPB;//一伯間に何分割されているか

    private int ScoreLegth;



    [SerializeField]MainGameManager mainGameManager;

    [SerializeField] ScoreBox scoreData;

    [SerializeField] float AttackTime;
    [SerializeField] AudioSource audioSource;

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



    [SerializeField]
    private GameObject notesPre;

    private float moveSpan = 0.01f;
    private float nowTime;// 音楽の再生されている時間
    private int beatNum;// 今の拍数
    private int beatCount;// json配列用(拍数)のカウント
    private bool isBeat;// ビートを打っているか(生成のタイミング)

    void Awake()
    {
        PlayerPrefs.SetInt("StageNum", 0);
        if (scoreData == null)
        {
            Debug.LogError("ScoreDataがアタッチされてない");
        }
        if (mainGameManager == null)
        {
            Debug.LogError("MainGameManagerがアタッチされていない byEnemyNoteManager");
        }
        MusicReading();
        InvokeRepeating("NotesIns", 10f, moveSpan);
    }


    /// <summary>
    /// 譜面上の時間とゲームの時間のカウントと制御
    /// </summary>
    void GetScoreTime()
    {
        //今の音楽の時間の取得
        nowTime += moveSpan; //(1)

        //ノーツが無くなったら処理終了
        if (beatCount > NotesNum.Length) return;

        //楽譜上でどこかの取得
        beatNum = (int)(nowTime * BPM / 60 * LPB); //(2)
    }

    /// <summary>
    /// ノーツを生成する
    /// </summary>
    void NotesIns()
    {
        GetScoreTime();

        //json上でのカウントと楽譜上でのカウントの一致
        if (beatCount < NotesNum.Length)
        {
            isBeat = (NotesNum[beatCount] == beatNum); //(3)
        }

        //生成のタイミングなら
        if (isBeat)
        {
            //ノーツ0の生成
            if (AttackType[beatCount] == 0)
            {
                audioSource.Play();
            }

            //ノーツ1の生成
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
               Debug.LogError("ScoreDataがアタッチされてない");
           }
           if (mainGameManager == null)
           {
               Debug.LogError("MainGameManagerがアタッチされていない byEnemyNoteManager");
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
            Debug.Log("終了");
            EnemyNoteManagerFix=false;
            return;
        }
        NextBeat = NotesNum[mainGameManager.AttackCount];
        NextAttackType = AttackType[mainGameManager.AttackCount-1];
    }*/
}
