using System;
using UnityEngine;

public class EnemyNoteManager : MonoBehaviour
{
    //１LPB経過する時間
    [SerializeField] private float BeatSplit = 0;
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




    private float ReadSpan = 0.01f;//何秒ごとに実行するか
    private float nowTime;// 音楽の再生されている時間
    private int beatNum;// 今の拍数
    private int beatCount;// json配列用(拍数)のカウント
    private bool isBeat;// ビートを打っているか(生成のタイミング)

    void Awake()
    {
        if (scoreData == null)
        {
            Debug.LogError("ScoreDataがアタッチされてない");
        }
        if (mainGameManager == null)
        {
            Debug.LogError("MainGameManagerがアタッチされていない byEnemyNoteManager");
        }

        //読み込み
        MusicReading();
        //一定時間後にmoveSpan間隔で指定関数を実行（移設必須）
        InvokeRepeating("EnemyAttackIns", 3f, ReadSpan);
    }


    /// <summary>
    /// 譜面上の時間とゲームの時間のカウントと制御
    /// </summary>
    void GetScoreTime()
    {
        //今の音楽の時間の取得
        nowTime += ReadSpan;

        //ノーツがないなら終了
        if (beatCount > NotesNum.Length) return;

        //楽譜上でどこかの取得
        beatNum = (int)(nowTime * BPM / 60 * LPB);
    }

    /// <summary>
    /// 攻撃タイプの読み取り
    /// </summary>
    void EnemyAttackIns()
    {
        GetScoreTime();

        //カウントの一致でisBeatをtrueに
        if (beatCount < NotesNum.Length)
        {
            isBeat = (NotesNum[beatCount] == beatNum);
        }

        //生成のタイミングなら
        if (isBeat)
        {
            //Type0でBGM再生
            if (AttackType[beatCount] == 0)
            {
                //BGM再生
                audioSource.Play();
            }
            else //0以外の時アタック関数にタイプを渡して実行
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
    /// 譜面の読み込み
    /// </summary>
    void MusicReading()
    {
        //ステージ番号取得
        int StageNum = PlayerPrefs.GetInt("StageNum",100);

        if (StageNum == 100)
        {
            Debug.LogError("ステージナンバーが格納されてない");
        }
        else
        {
            Debug.Log("ステージナンバー格納済");
        }

        //jsonファイルが格納されてる場所のパス取得
        string inputString = scoreData.GetListInScore(StageNum).GetScore().ToString();
        Debug.Log(inputString);
        //jsonファイル取得
        InputJson inputJson = JsonUtility.FromJson<InputJson>(inputString);

        //各サイズ格納
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
}
