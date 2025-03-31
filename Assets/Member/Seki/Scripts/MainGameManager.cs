using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class MainGameManager : MonoBehaviour
{
    public int PlayerHp = 3;//プレイヤーのHP
    
    [SerializeField] PlayerManager PlayerObj;//プレイヤー
    
    [SerializeField] EnemyManager EnemyObj;//敵
    
    private float BattleTime = 0.0f;//経過時間測定
    
    public bool GameStart=false;//ゲームが開始してるか
    
    [SerializeField] public GameObject ParryTimingSprite ;//いまだ！画像
    
    private int AttackCount = 0;//敵が攻撃した回数（パリィ率表記用）
    
    public int ParryCount = 0;//パリィ成功回数
    
    public bool Guardnow = false;//ガード中フラグ
    
    [SerializeField] private Image[] HpSprite;//プレイヤーHP画像格納
   
    [SerializeField] private Sprite DamageHp; //HPダメージ画像格納
                                      
    public bool ParryReception = false;//パリィ受付フラグ
    
    public bool ParryHits = false;//パリィ成功時用連打対策
    
    public bool ParryAttack = false;//パリィ可能フラグ

    
    private Gamepad MyPad;//コントローラー格納

    public bool PadVibration=false; //コントローラーがバイブレーション中か

    [SerializeField,Header("スタミナ")] private GuardController guardController;

    [SerializeField, Header("ガードコスト")] private int GuardCost = 25;

    
    [SerializeField] private EnemyHandAnimation enemyHandAnimation;//パンチアニメーションスクリプト
    
    private bool handType=false;//パンチアニメーション左右制御用
    
    [SerializeField] private BeamParticleSpeed BeamMana;//ビームエフェクト制御

    public bool BeatFlag=true;//Beatフラグ
    
    [SerializeField, Header("NoteManager")] private EnemyNoteManager NoteMana;


    //SE用諸々
    private AudioSource DamageSource;
    private AudioSource GirdSource;
    [SerializeField, Header("斬撃SE")] public AudioClip SlashSE;
    [SerializeField, Header("ダメージSE")] private AudioClip DamageSE;
    [SerializeField, Header("盾ガードSE")] private AudioClip GirdSE;

    [SerializeField] private RobotKnockback robotKnockback;　//ロボノックバックアニメーション

    [SerializeField] public bool TestMode=false;


    void Start()
    {
        //FPSを60に固定
        Application.targetFrameRate = 60;

        //初期化
        BattleTime = 0.0f;
        AttackCount = 0;
        ParryCount = 0;
        ParryReception = false;
        //Pad情報格納
        MyPad = Gamepad.current;

        //盾ガードSEセットアップ
        //nullチェック
        if (GirdSource == null) GirdSource = this.gameObject.AddComponent<AudioSource>();
        //音量
        GirdSource.volume = 0.5f;
        //音源
        GirdSource.clip = GirdSE;
        //ループさせない
        GirdSource.loop = false;

        //ダメージSEセットアップ
        //nullチェック
        if (DamageSource == null) DamageSource = this.gameObject.AddComponent<AudioSource>();
        //音源
        DamageSource.clip = DamageSE;
        //ループさせない
        DamageSource.loop = false;
    }

    void Update()
    {
        //譜面上タイム取得
        if (GameStart)
        {
            BattleTime=NoteMana.NotenowTime;
        }
       

        //ゲームが開始していない時の処理
        if (GameStart == false)
        {
            //譜面の読み込みが完了しているか
            if (NoteMana.EnemyNoteManagerStandby == true)
            {
                //スタートさせる
                GameStart=true;
                NoteMana.EnemyAttackStart();
            }
        }

        //デバッグ用
        //if (TestMode) PlayerHp = 3;
    }

    /// <summary>
    /// リザルトへの遷移
    /// </summary>
    public void toResult()
    {
        GameStart = false;
        //敵残HP
        PlayerPrefs.SetInt("CurrentHP", EnemyObj.EnemyHP);
        //敵最大HP
        PlayerPrefs.SetInt("MaxHP", EnemyObj.EnemyMaxHP);
        //戦闘時間
        PlayerPrefs.SetFloat("Time", BattleTime);
        //敵攻撃回数
        PlayerPrefs.SetInt("EnemyAttackCount", AttackCount);
        //パリィ成功回数
        PlayerPrefs.SetInt("ParryCount", ParryCount);

        //音源FadeOut
        NoteMana.MusicFade();
        //シーン遷移
        FadeManager.Instance.LoadScene("ResultScene", 0.6f);
    }

    /// <summary>
    /// 敵が攻撃開始時に呼び出し、パリィ可能かどうか判定
    /// </summary>
    /// <param name="EnemyAttackType">攻撃の種類</param>
    /// <param name="lpbbeat">1拍の時間</param>
    /// <returns></returns>
    public IEnumerator EnemmyAttack(int EnemyAttackType,float lpbbeat)
    {
        BeatFlag = false;

        for (int i = 0; i < 3; i++)
        {
            Debug.Log(i);
            //パンチ攻撃
            if (EnemyAttackType==1)
            {
                if (i == 2)
                {
                    if (handType)
                    {
                        enemyHandAnimation.MoveHand(EnemyHandAnimation.HandType.Right, lpbbeat);
                        handType = false;
                    }
                    else
                    {
                        enemyHandAnimation.MoveHand(EnemyHandAnimation.HandType.Left, lpbbeat);
                        handType = true;
                    }
                }
            }

            //ビーム攻撃
            if (EnemyAttackType == 2)
            {
                if (i == 2)
                {
                    Debug.Log("ビーム攻撃入った");
                    BeamMana.SpeedChange(1);
                    robotKnockback.Knockback(1).Forget();
                }
            }
            //一伯空ける
            yield return new WaitForSeconds(lpbbeat);
        }

        //敵アタックカウント
        if(GameStart)AttackCount++;
        //ガード判定(ガードしてなかったら終了)
        if (!Guardnow)
        {
            //ゲーム中か（倒した後にダメージ受けないために）
            if (GameStart)
            {
                //プレイヤーダメージ
                PlayerDamage();
            }
            BeatFlag = true;
            yield break;
        }
        //スタミナ判定(不可なら終了)
        if (!guardController.UseGuard(GuardCost))
        {
            if (GameStart)
            {
                PlayerDamage();
            }
            BeatFlag=true;
            yield break;
        }
        //盾ガードSE挿入
        GirdSource.Play();

        //PlayerManager側でパリィ可能とフラグを立てている場合
        if (ParryReception)
        {
            //Debug.Log("パリィ可能！");
            //連打防止用フラグ
            ParryHits = true;
            //パリィ可能か
            ParryAttack = true;
            //いまだ！画像表示
            ParryTimingSprite.gameObject.SetActive(true);
            //ゲームパッド接続状態で可能になったらパッド振動
            if (MyPad != null)
            {
                PadVibration=true;
                MyPad.SetMotorSpeeds(1.0f, 1.0f);
            }
            //一伯分振動
            yield return new WaitForSeconds(lpbbeat);
            //ゲームパッド振動停止
            if(MyPad != null)
            {
                MyPad.SetMotorSpeeds(0.0f, 0.0f);
                PadVibration = false;
            }
            //アタック終了
            ParryAttack = false;
            //Debug.Log("パリイ終了");
            //いまだ！画像非表示
            ParryTimingSprite.gameObject.SetActive(false);
        }
        BeatFlag = true;
    }

    /// <summary>
    /// プレイヤーにダメージを与える
    /// </summary>
    void PlayerDamage()
    {
        //ダメージSE再生
        DamageSource.Play();

        //Debug.Log("ダメージを受けた！");
        //HP減少
        PlayerHp--;
        //プレイヤーダメージAnimation
        PlayerObj.PlayerAnim.SetTrigger("Damage");

        //HP画像をブランクに差し替え
        if (!TestMode)
        {
            //HP減少
            PlayerHp--;
            //HP画像をブランクに差し替え
            HpSprite[PlayerHp].sprite = DamageHp;
        }
    }
}
