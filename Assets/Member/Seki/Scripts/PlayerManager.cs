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
    bool Playerlose=false;
    //ガード時間計測用
    float GuardTime = 0.0f;
    //コントローラー用bool
    public bool GuardButton = false;
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

    //斬撃エフェクトを何コマ遅らせて再生するか
    [SerializeField] int DelayFlame = 20;


    void Start()
    {
        //初期化
        GuardTime = 0.0f;
        MainGameObj.ParryTimingSprite.gameObject.SetActive(false);
        Playerlose = false;
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
        ControllerManager.Instance.L2ButtonObservable.Subscribe(x => GuardButton = true).AddTo(disposables_);
        ControllerManager.Instance.R2ButtonObservable.Subscribe(x => GuardButton = true).AddTo(disposables_);
        ControllerManager.Instance.L2ButtonUpObservable.Subscribe(x => GuardButton = false).AddTo(disposables_);
        ControllerManager.Instance.R2ButtonUpObservable.Subscribe(x => GuardButton = false).AddTo(disposables_);
        ControllerManager.Instance.WestButtonObservable.Subscribe(x => ParryAttackButton = true).AddTo(disposables_);
        ControllerManager.Instance.WestButtonUpObservable.Subscribe(x=>ParryAttackButton = false).AddTo(disposables_);
    }


    void Update()
    {
        //ガード中
        if (Input.GetKey(KeyCode.Space) || GuardButton)
        {
            //Debug.Log("ガード");

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
            //MainGamaManagerに状態を渡す
            MainGameObj.Guardnow = true;

            //タイム計測
            GuardTime += Time.deltaTime;

            //ガード開始してから0.5秒以内に攻撃が来たらパリィ可、超えたら不可に
            if (GuardTime > 0.5)
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
            //押してなければパリィ判定用カウント初期化
            GuardTime = 0.0f;
        }

        //パリィ可能時間内にP(□)でパリィ成功
        if (Input.GetKeyDown(KeyCode.Return) || ParryAttackButton)
        {
            //Debug.Log("ParryAttack" + MainGameObj.ParryAttack);
            //Debug.Log("ParryHits" + MainGameObj.ParryHits);

            //MainGameManagerの判定状況取得
            if (MainGameObj.ParryAttack && MainGameObj.ParryHits)
            {
                //エフェクト停止
                ParryEffect.Stop();
                SlashEffect.Stop();

                //判定を下げる
                MainGameObj.ParryHits = false;
                //今だ！画像非表示
                MainGameObj.ParryTimingSprite.gameObject.SetActive(true);
                //アニメーションスピード加速
                PlayerAnim.SetFloat("GuardIdleSpeed", 5f);
                PlayerAnim.SetFloat("GuardActiveSpeed", 5f);

                //Animation再生
                PlayerAnim.SetBool("GuardCancel", false);
                PlayerAnim.SetTrigger("GuardIdle");
                PlayerAnim.SetTrigger("Counter");
                PlayerAnim.SetBool("Counted", true);

                //敵にダメージ
                EnemyObj.EnemyDamage(1);

                //パリィカウント
                MainGameObj.ParryCount++;

                //Debug.Log("パリィ成功");

                //斬撃SE挿入
                if (SlashSource == null) SlashSource = this.AddComponent<AudioSource>();
                //音量
                SlashSource.volume = 0.5f;
                //音源セット
                SlashSource.clip = MainGameObj.SlashSE;
                //ループ設定
                SlashSource.loop = false;
                //再生
                SlashSource.Play();

                //エフェクト挿入（シーン遷移時に再生されないようfalseにしてある）
                ParryEffect.gameObject.SetActive(true);
                ParryEffect.Play();
                //斬撃エフェクト再生
                StartCoroutine(SlashCot(DelayFlame));

            }
        }


        //HPが0でリザルトへ
        if (MainGameObj.PlayerHp <= 0 && !Playerlose)
        {
            //負けフラグ
            Playerlose = true;
            //ゲーム停止
            MainGameObj.GameStart=false;
            //リザルトへ渡す情報
            PlayerPrefs.SetInt("IsWin", 0);
            //リザルト遷移
            if (!MainGameObj.PadVibration) MainGameObj.toResult();
        }
    }

    //入力処理削除
    private void OnDestroy()
    {
        disposables_.Dispose();
    }

    /// <summary>
    /// 斬撃におけるアニメーションとエフェクトのタイミングを合わせるために再生するフレームを遅らせる
    /// フレームレートは60であらかじめ固定済
    /// </summary>
    /// <param name="flame">遅らせるフレーム数</param>
    /// <returns></returns>
    IEnumerator SlashCot(int flame)
    {
        //指定コマ数待つ
        for (var i = 0; i < flame; i++)
        {
            yield return null;
        }
        //エフェクト再生（遷移時の誤爆防止でfalseにしてある）
        SlashEffect.gameObject.SetActive(true);
        SlashEffect.Play();
    }
}
