using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class MainGameManager : MonoBehaviour
{
    private int _playerHp = 3;
    
    [SerializeField] private PlayerManager _playerObj;
    
    [SerializeField] private EnemyManager _enemyObj;

    private float _battleTime = 0.0f;
    
    private bool _gameStart=false;
    
    [SerializeField] private GameObject _parryTimingSprite ;//いまだ！sprite
    
    private int _attackCount = 0;//敵が攻撃した回数（パリィ率表記用）
    
    private int _parryCount = 0;//パリィ成功回数
    
    public bool _guardnow = false;//ガード中フラグ

    [SerializeField] private HpManager _hpMana;
    
    private bool _parryReception = false;//パリィ受付フラグ
    
    private Gamepad _myPad;//コントローラー格納

    public bool _padVibration=false; //コントローラーがバイブレーション中か

    [SerializeField,Header("スタミナ")] private GuardController _guardController;

    [SerializeField, Header("ガードコスト")] private float _guardCost = 25.0f;

    
    [SerializeField]private EnemyHandAnimation _enemyHandAnimation;//パンチアニメーション
    
    private bool _handType=false;//パンチアニメーション左右制御用
    
    [SerializeField] private BeamParticleSpeed _beamMana;//ビームエフェクト制御
                                                        

    [SerializeField] private EnemyNoteManager _noteMana;


    //SE用諸々
    private AudioSource _damageSource;
    private AudioSource _girdSource;
    [SerializeField, Header("ダメージSE")] private AudioClip _damageSE;
    [SerializeField, Header("盾ガードSE")] private AudioClip _girdSE;

    [SerializeField] private RobotKnockback _robotKnockback;　//ロボ動作制御

    [SerializeField] private bool _testMode=false;
    

    void Start()
    {
        //フレームを60に固定
        Application.targetFrameRate = 60;

        //初期化
        _battleTime = 0.0f;
        _attackCount = 0;
        _parryCount = 0;
        _parryReception = false;
        _myPad = Gamepad.current;
        _parryTimingSprite.SetActive(false);

        //盾ガードSEセットアップ
        if (_girdSource == null) _girdSource = this.gameObject.AddComponent<AudioSource>();
        _girdSource.volume = 0.5f;
        _girdSource.clip = _girdSE;
        _girdSource.loop = false;

        //ダメージSEセットアップ
        if (_damageSource == null) _damageSource = this.gameObject.AddComponent<AudioSource>();
        _damageSource.clip = _damageSE;
        _damageSource.loop = false;
    }

    void Update()
    {
        //譜面上タイム取得
        if (_gameStart)
        {
            _battleTime=_noteMana.GetNowTime();
        }
       

        //ゲームが開始していないか
        if (_gameStart == false)
        {
            //譜面の読み込みが完了しているか
            if (_noteMana.GetStandby() == true)
            {
                _gameStart=true;
                _noteMana.EnemyAttackStart();
            }
        }
        if (_testMode) _playerHp = 3;
    }

    //勝敗が決したときに呼び出す
    public void toResult()
    {
        _gameStart = false;

        int[]x=_enemyObj.GetEnemyEndHp();
        //敵残HP
        PlayerPrefs.SetInt("CurrentHP", x[0]);
        //敵最大HP
        PlayerPrefs.SetInt("MaxHP", x[1]);
        //戦闘時間
        PlayerPrefs.SetFloat("Time", _battleTime);
        //敵攻撃回数
        PlayerPrefs.SetInt("EnemyAttackCount", _attackCount);
        //パリィ成功回数
        PlayerPrefs.SetInt("_parryCount", _parryCount);

        _noteMana.MusicFade();
        FadeManager.Instance.LoadScene("ResultScene", 0.6f);
    }

    /// <summary>
    /// 敵が攻撃開始時に呼び出し、パリィ可能かどうか判定
    /// </summary>
    /// <param name="MAXCount">攻撃の種類</param>
    /// <param name="lpbbeat">1拍の時間</param>
    /// <returns></returns>
    public IEnumerator EnemmyAttack(int MAXCount,float lpbbeat)
    {
        
        bool panchi = false;
        bool beam = false;


        switch (MAXCount)
        {
            case 1:
                Debug.Log("パンチ");
                //アニメーション処理
                panchi = true;
                break;
            case 2:
                Debug.Log("ビーム");
                //アニメーション処理
                beam = true;
                break;
            case 3:
                Debug.Log("3拍攻撃");
                //アニメーション処理
                break;
        }
        Debug.Log("break抜けた");

        for (int i = 0; i < 3; i++)
        {
            Debug.Log(i);
            //パンチ攻撃
            if (panchi)
            {
                if (i == 2)
                {
                    panchi=false;
                    if (_handType)
                    {
                        _enemyHandAnimation.MoveHand(EnemyHandAnimation.HandType.Right, lpbbeat);
                        _handType = false;
                    }
                    else
                    {
                        _enemyHandAnimation.MoveHand(EnemyHandAnimation.HandType.Left, lpbbeat);
                        _handType = true;
                    }
                }
            }

            //ビーム攻撃
            if (beam)
            {
                if (i == 2)
                {
                    Debug.Log("ビーム攻撃入った");
                    //120BPMを0.5倍（二拍分のため）とした倍数を渡す
                    _beamMana.SpeedChange(1);
                    _robotKnockback.Knockback(1).Forget();
                    beam=false;
                }
            }
            yield return new WaitForSeconds(lpbbeat);
        }

        //敵アタックカウント
        if(_gameStart)_attackCount++;
        //ガード判定
        if (!_guardnow)
        {
            //ゲーム中か（倒した後にダメージ受けないために）
            if (_gameStart)
            {
                PlayerDamage();
            }
            yield break;
        }
        //スタミナ判定
        if (!_guardController.UseGuard(_guardCost))
        {
            if (_gameStart)
            {
                PlayerDamage();
            }
            yield break;
        }
        //盾ガードSE挿入
        _girdSource.Play();


        if (_parryReception)
        {
            Debug.Log("パリィ可能！");
            //パリィフラグ
            _playerObj.SetParryHits(true);
            //パリィ可能か
            _parryTimingSprite.gameObject.SetActive(true);
            //ゲームパッド接続状態で可能になったらパッド振動
            if (_myPad != null)
            {
                _padVibration=true;
                _myPad.SetMotorSpeeds(1.0f, 1.0f);
            }
            yield return new WaitForSeconds(lpbbeat);
            //ゲームパッド振動停止
            if(_myPad != null)
            {
                _myPad.SetMotorSpeeds(0.0f, 0.0f);
                _padVibration = false;
            }
            _playerObj.SetParryHits(false);
            Debug.Log("パリイ終了");
            _parryTimingSprite.gameObject.SetActive(false);
        }
    }

    void PlayerDamage()
    {
        //ダメージSE
        _damageSource.Play();

        Debug.Log("ダメージを受けた！");
        _playerHp--;
        //Animation
        _playerObj.TriggerPlayerAnim("Damage");

        //HP画像差し替え
        //myimage = HpSprite[_playerHp].GetComponent<Image>();
        //myimage.sprite = DamageHp;
        _hpMana.SetDamage(_playerHp);
    }

    public void OnTimingSp()
    {
        _parryTimingSprite.SetActive(true);
    }

    public void ExitTimingSp()
    {
        _parryTimingSprite.SetActive(false);
    }

    public int GetPlayerHp()
    {
        return _playerHp;
    }

    public bool GetStart()
    {
        return _gameStart;
    }

    public void SetGameEnd()
    {
        _gameStart = false;
    }

    /// <summary>
    /// パリィ成功時
    /// </summary>
    public void CountParry()
    {
        _parryCount++;
    }

    public void SetParryReception(bool x)
    {
        _parryReception = x;
    }
}
