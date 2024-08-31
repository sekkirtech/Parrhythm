using System;
using System.Collections;
using UnityEngine;

public class EnemyNoteManager : MonoBehaviour
{
    [SerializeField] private float BeatSplit = 0;//１LPB経過する時間
    public bool EnemyNoteManagerFix=false; //準備ができたか
    [SerializeField] public int[] NotesNum;//攻撃タイミング格納
    [SerializeField] public int[] AttackType;//攻撃手段格納
    public int BPM;
    public int LPB;//一伯間に何分割されているか

    private int ScoreLegth;



    [SerializeField]MainGameManager mainGameManager;

    [SerializeField] ScoreBox scoreData;

    [SerializeField] float AttackTime;
    [SerializeField] AudioSource audioSource;

    [SerializeField] bool TestMode;

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
    public float NotenowTime;// 音楽の再生されている時間
    private int beatNum;// 今の拍数
    private int beatCount;// json配列用(拍数)のカウント
    private bool isBeat;// ビートを打っているか(生成のタイミング)
    public float ClipLegth;//曲の長さ
    private AudioClip ClipSource;//音源
    [SerializeField, Header("音源終了後何秒後に遷移するか")] float EndWaitTime = 0;
    private bool EndLoad = true;//動いているか
    private float EndWaitSumLegth;//遷移までの時間と音源の長さを足して格納
    private float FadeDeltaTime;


    void Awake()
    {
        if (scoreData == null)
        {
            if (!TestMode)
            {
                Debug.LogError("ScoreDataがアタッチされてない");
            }
        }
        if (mainGameManager == null)
        {
            Debug.LogError("MainGameManagerがアタッチされていない byEnemyNoteManager");
        }
        //読み込み
        MusicReading();
    }

    private void Update()
    {

        if (EndLoad)
        {
            //音源が終了し一定時間経過したら遷移させる
            if (NotenowTime > EndWaitSumLegth)
            {
                EndLoad = false;
                PlayerPrefs.SetInt("IsWin", 0);
                mainGameManager.toResult();
            }
        }
        if (!EndLoad)
        {
            FadeDeltaTime += Time.deltaTime;
            audioSource.volume = (float)(1.0 - FadeDeltaTime / 1.0f);
        }
    }

    /// <summary>
    /// ゲーム開始の準備が完了したらMainGameManagerから呼び出し
    /// </summary>
    public void EnemyAttackStart()
    {
        //第二引数時間後に第三引数間隔で第一引数関数を実行（移設必須）
        InvokeRepeating("EnemyAttackIns", 3f, ReadSpan);
    }


    /// <summary>
    /// 譜面上の時間とゲームの時間のカウントと制御
    /// </summary>
    void GetScoreTime()
    {
        //今の音楽の時間の取得
        NotenowTime += ReadSpan;

        //ノーツがないなら終了
        if (beatCount > NotesNum.Length) return;

        //楽譜上でどこなのかの得
        beatNum = (int)(NotenowTime * BPM / 60 * LPB);
    }


    /// <summary>
    /// 攻撃タイプの読み取り
    /// InvokeRepeatingで実行される
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
                audioSource.volume = 0.5f;
                //BGM再生
                audioSource.Play();
            }
            else //0以外の時アタック用関数にタイプを渡して実行
            {
                StartCoroutine(mainGameManager.EnemmyAttack(AttackType[beatCount], (float)60 / (float)BPM));
            }


            beatCount++;
            isBeat = false;
        }
    }


    /// <summary>
    /// 譜面の読み込み
    /// </summary>
    void MusicReading()
    {
        //ステージ番号取得
        int StageNum = PlayerPrefs.GetInt("StageNum",555);

        //ステージ番号が格納されてるか確認
        if (StageNum == 555)
        {
            if (TestMode)
            {
                StageNum = 0;
            }
            else
            {
                Debug.LogError("ステージナンバーが格納されてない");
            }
        }
        else
        {
            Debug.Log("ステージナンバー格納済");
        }

        //jsonファイルが格納されてる場所のパス取得
        string inputString = scoreData.GetListInScore(StageNum).GetScore().ToString();
        //Debug.Log(inputString);
        //jsonファイル取得
        InputJson inputJson = JsonUtility.FromJson<InputJson>(inputString);

        //wav格納
        ClipSource=scoreData.GetListInScore(StageNum).GetClip();
        audioSource.clip = ClipSource;
        //長さ格納
        ClipLegth=ClipSource.length;

        //音源終わって遷移するまでの時間を算出
        EndWaitSumLegth=ClipLegth+EndWaitTime;

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
        EnemyNoteManagerFix = true;
    }

    public void MusicFade()
    {
        EndLoad=false;
    }
}
