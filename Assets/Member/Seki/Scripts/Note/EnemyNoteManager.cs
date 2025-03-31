using System;
using UnityEngine;

public class EnemyNoteManager : MonoBehaviour
{
    public bool EnemyNoteManagerStandby=false; //準備ができたか
    [SerializeField] public int[] AttackTiming;//攻撃タイミング格納
    [SerializeField] public int[] AttackType;//攻撃手段格納
    public int BPM;//曲の速さ、一分間に打たれる伯の数
    public int LPB;//一伯間に何分割されているか

    [SerializeField]MainGameManager MainGameObj;

    [SerializeField] public ScoreBox scoreData;//音源、譜面データ

    [SerializeField] AudioSource MusicSource;//音源再生用

    [SerializeField] bool TestMode;//デバッグ用

    [Serializable]
    public class NoteJsonClass
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
        //攻撃タイプ
        public int block;
        //一拍間の分割
        public int LPB;
    }




    private float LoadSpan = 0.01f;//何秒ごとに実行するか
    public float NotenowTime;// 音楽再生開始からの経過時間
    private int NowBeatNum;// 譜面上で今何拍目か
    private int BeatCount;// 何回攻撃したか
    private bool BeatNow;//攻撃生成用フラグ
    public float ClipLegth;//音源の長さ
    private AudioClip ClipSource;//音源ファイル
    [SerializeField, Header("音源終了後何秒後に遷移するか")] float EndWaitTime = 0;
    private bool EndLoad = true;//FadeOut中か
    private float EndWaitSumLegth;//遷移までの時間と音源の長さを足して格納
    private float FadeDeltaTime;//音源Fade用


    void Awake()
    {
        //nullチェック
        if (scoreData == null)
        {
            if (!TestMode)
            {
                Debug.LogError("ScoreDataがアタッチされてない");
            }
        }
        if (MainGameObj == null)
        {
            Debug.LogError("MainGameManagerがアタッチされていない EnemyNoteManager");
        }
        if(MusicSource == null)
        {
            MusicSource=this.GetComponent<AudioSource>();
        }
        //譜面読み込み
        MusicReading();
    }

    private void Update()
    {

        if (EndLoad)
        {
            //音源が終了し一定時間経過したら遷移を起動
            if (NotenowTime > EndWaitSumLegth)
            {
                EndLoad = false;
                PlayerPrefs.SetInt("IsWin", 0);
            }
        }
        if (!EndLoad)
        {
            //音源FadeOut
            FadeDeltaTime += Time.deltaTime;
            MusicSource.volume = (float)(MusicSource.volume - FadeDeltaTime / 1.0f);
            //FadeOut終了後シーン遷移させる
            if(MusicSource.volume<=0)
            {
                //コントローラーがバイブレーション中なら待つ
                if (!MainGameObj.PadVibration) MainGameObj.toResult();
            }
        }
    }

    /// <summary>
    /// ゲーム開始の準備が完了したらMainGameManagerから呼び出し
    /// </summary>
    public void EnemyAttackStart()
    {
        //第二引数時間後に第三引数間隔で第一引数関数を実行
        InvokeRepeating("EnemyAttackIns", 1f, LoadSpan);
    }


    /// <summary>
    /// 譜面上の時間とゲームの時間のカウントと制御
    /// </summary>
    void GetScoreTime()
    {
        //今の音楽の時間の取得
        NotenowTime += LoadSpan;

        //ノーツがないなら終了(アウトロの存在も考えfadeはしない)
        if (BeatCount > AttackTiming.Length) return;

        //楽譜上で今どこなのかの取得
        NowBeatNum = (int)(NotenowTime * BPM / 60 * LPB);
    }


    /// <summary>
    /// 攻撃タイプの読み取り
    /// InvokeRepeatingで繰り返し実行される
    /// </summary>
    void EnemyAttackIns()
    {
        GetScoreTime();

        //カウントの一致でisBeatをtrueに
        if (BeatCount < AttackTiming.Length)
        {
            BeatNow = (AttackTiming[BeatCount] == NowBeatNum);
        }

        //生成のタイミングなら
        if (BeatNow)
        {
            //Type0でBGM再生
            if (AttackType[BeatCount] == 0)
            {
                //BGM再生
                MusicSource.Play();
            }
            else //0以外の時アタック用関数にタイプを渡して実行
            {
                StartCoroutine(MainGameObj.EnemmyAttack(AttackType[BeatCount], (float)60 / (float)BPM));
            }


            BeatCount++;
            BeatNow = false;
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
        string JsonPath = scoreData.GetListInScore(StageNum).GetScore().ToString();
        //jsonファイル取得
        NoteJsonClass NoteJson = JsonUtility.FromJson<NoteJsonClass>(JsonPath);

        //wav音源取得
        ClipSource=scoreData.GetListInScore(StageNum).GetClip();
        //音源セット
        MusicSource.clip = ClipSource;
        //音量セット
        MusicSource.volume=scoreData.GetListInScore(StageNum).GetVolume();
        //音源長さ格納
        ClipLegth=ClipSource.length;

        //音源終わって遷移するまでの時間を格納
        EndWaitSumLegth=ClipLegth+EndWaitTime;

        //各サイズ格納
        AttackTiming = new int[NoteJson.notes.Length];
        AttackType = new int[NoteJson.notes.Length];
        //情報格納
        BPM = NoteJson.BPM;
        LPB = NoteJson.notes[0].LPB;

        for (int i = 0; i < NoteJson.notes.Length; i++)
        {
            //攻撃タイミング格納
            AttackTiming[i] = NoteJson.notes[i].num;
            //攻撃手段格納
            AttackType[i] = NoteJson.notes[i].block;
        }

        //準備完了
        EnemyNoteManagerStandby = true;
    }

    /// <summary>
    /// updateにある音楽fadeoutスクリプトを起動
    /// </summary>
    public void MusicFade()
    {
        EndLoad=false;
    }
}
