using System;
using UnityEngine;

public class EnemyNoteManager : MonoBehaviour
{
    private bool _standby=false;
    [SerializeField] private int[] _attackTiming;
    [SerializeField] private int[] _attackType;

    /// <summary>
    /// 一分間に発生する拍の数（要はリズムの速さ）
    /// </summary>
    private int _BPM;

    /// <summary>
    /// 譜面上で1BPM何分割されているか
    /// </summary>
    private int _LPB;

    [SerializeField] private MainGameManager _mainGameObj;

    /// <summary>
    /// 譜面データオブジェクト
    /// </summary>
    [SerializeField] private ScoreBox _scoreData;

    [SerializeField] private AudioSource _musicSource;

    [SerializeField] private bool _testMode;

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




    private const float _loadSpan = 0.01f;//何秒ごとに実行するか
    private float _noteNowTime;// 音楽再生開始からの経過時間
    private int _nowBeatNum;// 譜面上で今何拍目か
    private int _beatCount;// 何回攻撃したか
    private bool _beatNow;//攻撃生成用フラグ
    private float _clipLegth;//音源の長さ
    private AudioClip _clipSource;//音源ファイル
    [SerializeField, Header("音源終了後何秒後に遷移するか")] private float _endWaitTime = 0;
    private bool _endLoad = true;//FadeOut中か
    private float _endWaitSumLegth;//遷移までの時間と音源の長さを足して格納
    private float _fadeDeltaTime;//音源Fade用


    void Awake()
    {
        _standby = false;
        //nullチェック
        if (_scoreData == null)
        {
            if (!_testMode)
            {
                Debug.LogError("ScoreDataがアタッチされてない");
            }
        }
        if (_mainGameObj == null)
        {
            Debug.LogError("MainGameManagerがアタッチされていない");
        }
        if(_musicSource == null)
        {
            _musicSource=this.GetComponent<AudioSource>();
        }
        //譜面読み込み
        MusicReading();

        Debug.Log(_endWaitTime);
    }

    private void Update()
    {

        if (_endLoad)
        {
            //音源が終了し一定時間経過したら遷移を起動
            if (_noteNowTime > _endWaitSumLegth)
            {
                _endLoad = false;
                PlayerPrefs.SetInt("IsWin", 0);
            }
        }
        if (!_endLoad)
        {
            //音源FadeOut
            _fadeDeltaTime += Time.deltaTime;
            _musicSource.volume = (float)(_musicSource.volume - _fadeDeltaTime / 1.0f);
            //FadeOut終了後シーン遷移させる
            if(_musicSource.volume<=0)
            {
                //コントローラーがバイブレーション中なら待つ
                if (!_mainGameObj._padVibration) _mainGameObj.toResult();
            }
        }
    }

    /// <summary>
    /// ゲーム開始の準備が完了したらMainGameManagerから呼び出し
    /// </summary>
    public void EnemyAttackStart()
    {
        //第二引数時間後に第三引数間隔で第一引数関数を実行
        InvokeRepeating("EnemyAttackIns", 1f, _loadSpan);
    }


    /// <summary>
    /// 譜面上の時間とゲームの時間のカウントと制御
    /// </summary>
    void GetScoreTime()
    {
        //今の音楽の時間の取得
        _noteNowTime += _loadSpan;

        //ノーツがないなら終了(アウトロの存在も考えfadeはしない)
        if (_beatCount > _attackTiming.Length) return;

        //楽譜上で今どこなのかの取得
        _nowBeatNum = (int)(_noteNowTime * _BPM / 60 * _LPB);
    }


    /// <summary>
    /// 攻撃タイプの読み取り
    /// InvokeRepeatingで繰り返し実行される
    /// </summary>
    void EnemyAttackIns()
    {
        GetScoreTime();

        //カウントの一致でisBeatをtrueに
        if (_beatCount < _attackTiming.Length)
        {
            _beatNow = (_attackTiming[_beatCount] == _nowBeatNum);
        }

        //生成のタイミングなら
        if (_beatNow)
        {
            //Type0でBGM再生
            if (_attackType[_beatCount] == 0)
            {
                //BGM再生
                _musicSource.Play();
            }
            else //0以外の時アタック用関数にタイプを渡して実行
            {
                StartCoroutine(_mainGameObj.EnemmyAttack(_attackType[_beatCount], (float)60 / (float)_BPM));
            }


            _beatCount++;
            _beatNow = false;
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
            if (_testMode)
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
        string JsonPath = _scoreData.GetListInScore(StageNum).GetScore().ToString();
        //jsonファイル取得
        NoteJsonClass NoteJson = JsonUtility.FromJson<NoteJsonClass>(JsonPath);

        //wav音源取得
        _clipSource=_scoreData.GetListInScore(StageNum).GetClip();
        //音源セット
        _musicSource.clip = _clipSource;
        //音量セット
        _musicSource.volume=_scoreData.GetListInScore(StageNum).GetVolume();
        //音源長さ格納
        _clipLegth=_clipSource.length;

        //音源終わって遷移するまでの時間を格納
        _endWaitSumLegth=_clipLegth+_endWaitTime;

        //各サイズ格納
        _attackTiming = new int[NoteJson.notes.Length];
        _attackType = new int[NoteJson.notes.Length];
        //情報格納
        _BPM = NoteJson.BPM;
        _LPB = NoteJson.notes[0].LPB;

        for (int i = 0; i < NoteJson.notes.Length; i++)
        {
            //攻撃タイミング格納
            _attackTiming[i] = NoteJson.notes[i].num;
            //攻撃手段格納
            _attackType[i] = NoteJson.notes[i].block;
        }

        //準備完了
        _standby = true;
    }

    /// <summary>
    /// updateにある音楽fadeoutスクリプトを起動
    /// </summary>
    public void MusicFade()
    {
        _endLoad=false;
    }

    /// <summary>
    /// BPMの取得
    /// </summary>
    /// <returns></returns>
    public float GetBPM()
    {
        return _BPM;
    }

    /// <summary>
    /// 準備完了状態取得
    /// </summary>
    /// <returns></returns>
    public bool GetStandby()
    {
        return _standby; 
    }

    public int GetEnemyHp(int x)
    {
        return _scoreData.GetListInScore(x).GetEnemyHP();
    }

    public float GetNowTime()
    {
        return _noteNowTime;
    }
}
