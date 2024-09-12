using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class MainGameManager : MonoBehaviour
{
    //プレイヤーのHP
    public int PlayerHp = 3;
    //プレイヤー格納
    [SerializeField] PlayerManager PlayerObj;
    //敵格納
    [SerializeField] EnemyManager EnemyObj;
    //経過時間測定
    public float BattleTime = 0.0f;
    //ゲームが開始してるか
    public bool GameStart=false;
    //α視覚用obj
    [SerializeField] public GameObject[] SpriteList;
    //敵が攻撃した回数（パリィ率表記用）
    public int AttackCount = 0;
    //パリィ成功回数
    public int ParryCount = 0;
    //ガード中フラグ
    public bool Girdnow = false;
    //スタミナガード可不可フラグ
    public bool SutaminaGird = true;
    //HP画像格納
    [SerializeField] GameObject[] HpSprite;
    //HPダメージ画像格納
    [SerializeField] Sprite DamageHp;
    private Image myimage;
    //パリィ受付フラグ
    public bool ParryReception = false;
    //パリィ成功時用連打対策
    public bool ParryHits = false;
    //パリィ可能フラグ
    public bool ParryAttack = false;
    //コントローラー格納
    Gamepad MyPad;
    [SerializeField,Header("スタミナ")] GuardController guardController;
    [SerializeField, Header("ガードコスト")] float GuardCost = 25.0f;



    //拍用AudioSource
    [SerializeField] AudioSource BeatAudioSource;
    [SerializeField] AudioClip Beat;
    [SerializeField] AudioClip BeatFin;


    //パンチアニメーション
    [SerializeField] EnemyHandAnimation enemyHandAnimation;
    //パンチアニメーション左右制御
    private bool handType=false;
    //ビームエフェクト制御
    [SerializeField] ParticleSpeed BeamMana;

    bool panchi = false;
    bool beam = false;

    //Beatフラグ
    public bool BeatFlag=true;
    
    [SerializeField, Header("NoteManager")] EnemyNoteManager NoteMana;


    //SE用諸々
    AudioSource DamageSource;
    AudioSource GirdSource;
    [SerializeField, Header("斬撃SE")] public AudioClip SlashSE;
    [SerializeField, Header("ダメージSE")] AudioClip DamageSE;
    [SerializeField, Header("盾ガードSE")] AudioClip GirdSE;


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
/// あとテスト用拍再生
/// </summary>
/// <param name="MAXCount">Beatの数</param>
/// <returns></returns>
    public IEnumerator EnemmyAttack(int MAXCount,float lpbbeat)
    {
        panchi=false;
        beam=false;
        BeatFlag = false;
        switch (MAXCount)
        {
            case 1:
                Debug.Log("パンチ攻撃　1");
                //アニメーション処理
                panchi = true;
                break;
            case 2:
                Debug.Log("ビーム攻撃　2");
                //アニメーション処理
                beam = true;
                break;
            case 3:
                Debug.Log("パンチ！　3");
                //アニメーション処理
                break;
        }
        Debug.Log("break抜けた");

        BeatAudioSource.clip = Beat;
        Debug.Log("拍セット");
        for (int i = 0; i < 3; i++)
        {
            //BeatAudioSource.Play();
            Debug.Log("拍Play");
            Debug.Log(i);
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
            if (beam)
            {
                if (i == (1))
                {
                    BeamMana.ChangeSpeed(1.0f);
                }
            }
            yield return new WaitForSeconds(lpbbeat);
        }

/*        BeatAudioSource.clip = BeatFin;
        BeatAudioSource.Play();*/

        AttackCount++;
        //ガード判定
        if (!Girdnow)
        {
            PlayerDamage();
            BeatFlag = true;
            //SpriteList[0].SetActive(false);
            yield break;
        }
        //スタミナ判定
        if (!guardController.UseGuard(GuardCost))
        {
            PlayerDamage();
            BeatFlag=true;
            //SpriteList[0].SetActive(false);
            yield break;
        }
        //盾ガードSE挿入
        if(GirdSource==null) GirdSource = this.gameObject.AddComponent<AudioSource>();
        GirdSource.volume = 0.5f;
        GirdSource.clip = GirdSE;
        GirdSource.loop = false;
        GirdSource.Play();


        if (ParryReception)
        {
            Debug.Log("パリィ可能！");
            //連打防止用フラグ
            ParryHits = true;
            //パリィ可能か
            ParryAttack = true;
            SpriteList[2].gameObject.SetActive(true);
            //ゲームパッド接続状態で可能になったらパッド振動
            if (MyPad != null)
            {
                MyPad.SetMotorSpeeds(1.0f, 1.0f);
            }
            yield return new WaitForSeconds(lpbbeat);
            //ゲームパッド振動初期化
            if(MyPad != null)
            {
                MyPad.SetMotorSpeeds(0.0f, 0.0f);
            }
            ParryAttack = false;
            Debug.Log("パリイ終了");
            SpriteList[2].gameObject.SetActive(false);
        }
        //SpriteList[0].SetActive(false);
        BeatFlag = true;
    }

    void PlayerDamage()
    {
        //ダメージSE
        if(DamageSource==null) DamageSource= this.gameObject.AddComponent<AudioSource>();
        DamageSource.clip = DamageSE;
        DamageSource.loop = false;
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
