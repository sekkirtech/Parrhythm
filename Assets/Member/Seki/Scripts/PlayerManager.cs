using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;


public class PlayerManager : MonoBehaviour
{
    //敵オブジェクト
    [SerializeField] EnemyManager EnemyObj;
    //情報格納用マネージャー
    [SerializeField] MainGameManager MainGameObj;
    //連打防止フラグ
    bool playerlose=false;
    //ガード時間計測用
    float GirdTime = 0.0f;
    //コントローラー用bool
    private bool GirdButton = false;
    private bool ParryAttackButton = false;
    //Animator
    [SerializeField] public Animator PlayerAnim;
    //AnimationFlag
    public bool CancedGuardAnim=false;

    //SE用Source
    AudioSource SlashSource;

    private CompositeDisposable disposables_=new CompositeDisposable();

    [SerializeField, Header("パリィエフェクト")] ParticleSystem ParryEffect;

    [SerializeField, Header("スラッシュエフェクト")] ParticleSystem SlashEffect;

    [SerializeField] int DelayFlame = 20;


    void Start()
    {
        //初期化
        GirdTime = 0.0f;
        MainGameObj.ParryTimingSprite.gameObject.SetActive(false);
        playerlose = false;
        ParryEffect.gameObject.SetActive(false);
        SlashEffect.gameObject.SetActive(false);

        //nullチェック
        if (EnemyObj == null)
        {
            Debug.Log("Enemyがないのでアタッチします");
            GameObject enemyseki = GameObject.Find("Player");
            EnemyObj = enemyseki.GetComponent<EnemyManager>();
        }
        //入力処理登録
        ControllerManager.Instance.L2ButtonObservable.Subscribe(x => GirdButton = true).AddTo(disposables_);
        ControllerManager.Instance.R2ButtonObservable.Subscribe(x => GirdButton = true).AddTo(disposables_);
        ControllerManager.Instance.L2ButtonUpObservable.Subscribe(x => GirdButton = false).AddTo(disposables_);
        ControllerManager.Instance.R2ButtonUpObservable.Subscribe(x => GirdButton = false).AddTo(disposables_);
        ControllerManager.Instance.WestButtonObservable.Subscribe(x => ParryAttackButton = true).AddTo(disposables_);
        ControllerManager.Instance.WestButtonUpObservable.Subscribe(x=>ParryAttackButton = false).AddTo(disposables_);
    }


    void Update()
    {
        //ガード中
        if (Input.GetKey(KeyCode.Space) || GirdButton)
        {
            Debug.Log("ガード");

            //ガード中ではないとき
            if (!MainGameObj.Guardnow)
            {
                //キャンセルアニメーション加速
                PlayerAnim.SetFloat("GuardCancelSpeed", 5f);

                //アニメーションスピード初期化
                PlayerAnim.SetFloat("GuardActiveSpeed", 1f);
                PlayerAnim.SetFloat("GuardIdleSpeed", 1f);

                //再生アニメーション整理
                PlayerAnim.SetTrigger("GuardActive");
                PlayerAnim.SetTrigger("GuardIdle");
                PlayerAnim.SetBool("GuardCancel", false);
            }
            CancedGuardAnim = true;
            MainGameObj.Guardnow = true;

            //タイム計測
            GirdTime += Time.deltaTime;

            //ガード開始してから0.5秒以内でパリィ可、超えたら不可に
            if (GirdTime > 0.5)
            {
                MainGameObj.ParryReception = false;
            }
            else
            {
                MainGameObj.ParryReception = true;
            }
        }
        else
        {
            //ガード解消時に諸々初期化等
            if(MainGameObj.Guardnow&& !PlayerAnim.GetBool("Counter"))
            {
                MainGameObj.Guardnow=false;
                CancedGuardAnim = false;

                //アニメーションスピード加速
                PlayerAnim.SetFloat("GuardActiveSpeed", 5f);
                PlayerAnim.SetFloat("GuardIdleSpeed", 5f);

                //アニメーションスピード初期化
                PlayerAnim.SetFloat("GuardCancelSpeed", 1f);

                //アニメーション再生
                PlayerAnim.SetBool("GuardIdle", false);
                PlayerAnim.SetBool("Counted", false);
                PlayerAnim.SetBool("GuardCancel", true);
            }
            //押してなければ初期化
            GirdTime = 0.0f;
        }

        //パリィ可能時間内にP(□)でパリィ成功
        if (Input.GetKeyDown(KeyCode.Return) || ParryAttackButton)
        {
            Debug.Log("ParryAttack" + MainGameObj.ParryAttack);
            Debug.Log("ParryHits" + MainGameObj.ParryHits);
            if (MainGameObj.ParryAttack && MainGameObj.ParryHits)
            {
                ParryEffect.Stop();


                MainGameObj.ParryHits = false;
                MainGameObj.ParryTimingSprite.gameObject.SetActive(true);
                //アニメーションスピード加速
                PlayerAnim.SetFloat("GuardIdleSpeed", 5f);
                PlayerAnim.SetFloat("GuardActiveSpeed", 5f);

                //Animation再生
                PlayerAnim.SetBool("GuardCancel", false);
                PlayerAnim.SetTrigger("GuardIdle");
                PlayerAnim.SetTrigger("Counter");
                PlayerAnim.SetBool("Counted", true);
                EnemyObj.EnemyDamage(1);

                //パリィカウント
                MainGameObj.ParryCount++;

                Debug.Log("パリィ成功");

                //斬撃SE挿入
                if (SlashSource == null) SlashSource = this.AddComponent<AudioSource>();
                SlashSource.volume = 0.5f;
                SlashSource.clip = MainGameObj.SlashSE;
                SlashSource.loop = false;
                SlashSource.Play();

                //エフェクト挿入
                ParryEffect.gameObject.SetActive(true);
                ParryEffect.Play();
                StartCoroutine(SlashCot(DelayFlame));

            }
        }


        //HPが0でリザルトへ
        if (MainGameObj.PlayerHp <= 0 && !playerlose)
        {
            playerlose = true;
            MainGameObj.GameStart=false;
            PlayerPrefs.SetInt("IsWin", 0);
            if (!MainGameObj.PadVibration) MainGameObj.toResult();
        }


        if (PlayerAnim.GetBool("Counter"))
        {
            Debug.Log("カウンター中");
            PlayerAnim.SetFloat("PlayerIdleSpeed", 3f);
        }
        else
        {
            PlayerAnim.SetFloat("PlayerIdleSpeed", 1f);
        }
    }

    //Subscribe削除
    private void OnDestroy()
    {
        disposables_.Dispose();
    }

    /// <summary>
    /// アニメーションとエフェクトのタイミングを合わせるために再生するフレームを遅らせる
    /// </summary>
    /// <param name="flame">遅らせるフレーム数</param>
    /// <returns></returns>
    IEnumerator SlashCot(int flame)
    {
        for (var i = 0; i < flame; i++)
        {
            yield return null;
        }
        SlashEffect.gameObject.SetActive(true);
        SlashEffect.Play();
        PlayerAnim.SetFloat("PlayerIdleSpeed", 1f);
    }
}
