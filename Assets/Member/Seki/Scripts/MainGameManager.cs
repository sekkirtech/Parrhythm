using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class MainGameManager : MonoBehaviour
{
    public int PlayerHp = 3;//プレイヤーのHP
    
    [SerializeField] PlayerManager PlayerObj;//プレイヤー格納
    
    [SerializeField] EnemyManager EnemyObj;//敵格納
    
    public float BattleTime = 0.0f;//経過時間測定
    
    public bool GameStart=false;//ゲームが開始してるか
    
    [SerializeField] public GameObject ParryTimingSprite ;//いまだ！画像
    
    public int AttackCount = 0;//敵が攻撃した回数（パリィ率表記用）
    
    public int ParryCount = 0;//パリィ成功回数
    
    public bool Guardnow = false;//ガード中フラグ
    
    [SerializeField] GameObject[] HpSprite;//プレイヤーHP画像格納
   
    [SerializeField] Sprite DamageHp; //HPダメージ画像格納
    private Image myimage;
    
    public bool ParryReception = false;//パリィ受付フラグ
    
    public bool ParryHits = false;//パリィ成功時用連打対策
    
    public bool ParryAttack = false;//パリィ可能フラグ

    
    Gamepad MyPad;//コントローラー格納

    [SerializeField,Header("スタミナ")] GuardController guardController;

    [SerializeField, Header("ガードコスト")] float GuardCost = 25.0f;

    
    [SerializeField] EnemyHandAnimation enemyHandAnimation;//パンチアニメーション
    
    private bool handType=false;//パンチアニメーション左右制御用
    
    [SerializeField] ParticleSpeed BeamMana;//ビームエフェクト制御

    bool panchi = false;
    bool beam = false;

    
    public bool BeatFlag=true;//Beatフラグ
    
    [SerializeField, Header("NoteManager")] EnemyNoteManager NoteMana;


    //SE用諸々
    AudioSource DamageSource;
    AudioSource GirdSource;
    [SerializeField, Header("斬撃SE")] public AudioClip SlashSE;
    [SerializeField, Header("ダメージSE")] AudioClip DamageSE;
    [SerializeField, Header("盾ガードSE")] AudioClip GirdSE;

    private float BeamSpeed;//ビーム攻撃発生時に渡す引数格納枠


    void Start()
    {
        //FPSを60に固定
        Application.targetFrameRate = 60;

        //初期化
        BattleTime = 0.0f;
        AttackCount = 0;
        ParryCount = 0;
        ParryReception = false;
        MyPad = Gamepad.current;

        //盾ガードSEセットアップ
        if (GirdSource == null) GirdSource = this.gameObject.AddComponent<AudioSource>();
        GirdSource.volume = 0.5f;
        GirdSource.clip = GirdSE;
        GirdSource.loop = false;

        //ダメージSEセットアップ
        if (DamageSource == null) DamageSource = this.gameObject.AddComponent<AudioSource>();
        DamageSource.clip = DamageSE;
        DamageSource.loop = false;

        //120BPMを1倍とした倍数を格納（ビーム攻撃にて使用）
        BeamSpeed = (float)NoteMana.BPM / (float)120;
    }

    void Update()
    {
        //譜面上タイム取得
        if (GameStart)
        {
            BattleTime=NoteMana.NotenowTime;
        }
       

        //ゲームが開始していないか
        if (GameStart == false)
        {
            //譜面の読み込みが完了しているか
            if (NoteMana.EnemyNoteManagerFix == true)
            {
                GameStart=true;
                NoteMana.EnemyAttackStart();
            }
        }
    }

    //勝敗が決したときに呼び出す
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

        NoteMana.MusicFade();
        FadeManager.Instance.LoadScene("ResultScene", 1.0f);
    }

    /// <summary>
    /// 敵が攻撃開始時に呼び出し、パリィ可能かどうか判定
    /// </summary>
    /// <param name="MAXCount">Beatの数</param>
    /// <param name="lpbbeat">1拍の時間</param>
    /// <returns></returns>
    public IEnumerator EnemmyAttack(int MAXCount,float lpbbeat)
    {
        panchi=false;
        beam=false;
        BeatFlag = false;
        switch (MAXCount)
        {
            case 1:
                Debug.Log("1拍攻撃");
                //アニメーション処理
                panchi = true;
                break;
            case 2:
                Debug.Log("2拍攻撃");
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
                if (i == (2))
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
            if (beam)
            {
                if (i == (1))
                {
                    //120BPMを1倍とした倍数を渡す
                    BeamMana.ChangeSpeed(BeamSpeed);
                }
            }
            yield return new WaitForSeconds(lpbbeat);
        }

        AttackCount++;
        //ガード判定
        if (!Guardnow)
        {
            PlayerDamage();
            BeatFlag = true;
            yield break;
        }
        //スタミナ判定
        if (!guardController.UseGuard(GuardCost))
        {
            PlayerDamage();
            BeatFlag=true;
            yield break;
        }
        //盾ガードSE挿入
        GirdSource.Play();


        if (ParryReception)
        {
            Debug.Log("パリィ可能！");
            //連打防止用フラグ
            ParryHits = true;
            //パリィ可能か
            ParryAttack = true;
            ParryTimingSprite.gameObject.SetActive(true);
            //ゲームパッド接続状態で可能になったらパッド振動
            if (MyPad != null)
            {
                MyPad.SetMotorSpeeds(1.0f, 1.0f);
            }
            yield return new WaitForSeconds(lpbbeat);
            //ゲームパッド振動停止
            if(MyPad != null)
            {
                MyPad.SetMotorSpeeds(0.0f, 0.0f);
            }
            ParryAttack = false;
            Debug.Log("パリイ終了");
            ParryTimingSprite.gameObject.SetActive(false);
        }
        BeatFlag = true;
    }

    void PlayerDamage()
    {
        //ダメージSE
        DamageSource.Play();

        Debug.Log("ダメージを受けた！");
        PlayerHp--;
        //Animation
        PlayerObj.PlayerAnim.SetTrigger("Damage");

        //HP画像差し替え
        myimage = HpSprite[PlayerHp].GetComponent<Image>();
        myimage.sprite = DamageHp;
    }
}
