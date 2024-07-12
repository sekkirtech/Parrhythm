using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.InputSystem;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


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
    //α版用
    private float HanteiTime = 0.0f;
    //コントローラー用bool
    private bool GirdButton = false;
    private bool ParryAttackButton = false;
    //Animator
    [SerializeField] public Animator PlayerAnim;
    //AnimationFlag
    public bool CancedGuardAnim=false;



    private CompositeDisposable disposables_=new CompositeDisposable();


    void Start()
    {
        //初期化
        GirdTime = 0.0f;
        MainGameObj.SpriteList[0].gameObject.SetActive(false);
        MainGameObj.SpriteList[1].gameObject.SetActive(false);
        MainGameObj.SpriteList[2].gameObject.SetActive(false);
        MainGameObj.SpriteList[3].gameObject.SetActive(false);
        playerlose = false;

        //nullチェック
        if (EnemyObj == null)
        {
            Debug.Log("Enemyがないのでアタッチします");
            GameObject enemyseki = GameObject.Find("Player");
            EnemyObj = enemyseki.GetComponent<EnemyManager>();
        }
        //コントローラー処理登録
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
            if (!MainGameObj.Girdnow)
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
            //MainGameObj.SpriteList[1].gameObject.SetActive(true);
            MainGameObj.Girdnow = true;
            //タイム計測
            GirdTime += Time.deltaTime;
            //0.25秒以内でパリィ可、超えたら不可に
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
            if(MainGameObj.Girdnow)
            {
                MainGameObj.Girdnow=false;
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
                //MainGameObj.SpriteList[1].gameObject.SetActive(false);
            }
            //押してなければ初期化
            GirdTime = 0.0f;
        }

        //パリィ可能時間内にP(□)でパリィ成功
        if (Input.GetKeyDown(KeyCode.P) || ParryAttackButton)
        {
            Debug.Log("ParryAttack" + MainGameObj.ParryAttack);
            Debug.Log("ParryHits" + MainGameObj.ParryHits);
            if (MainGameObj.ParryAttack && MainGameObj.ParryHits)
            {
                MainGameObj.ParryHits = false;
                MainGameObj.SpriteList[3].gameObject.SetActive(true);
                HanteiTime = 0.0f;
                //アニメーションスピード加速
                PlayerAnim.SetFloat("GuardIdleSpeed", 5f);
                PlayerAnim.SetFloat("GuardActiveSpeed", 5f);

                //Animation再生
                PlayerAnim.SetBool("GuardCancel", false);
                PlayerAnim.SetTrigger("GuardIdle");
                PlayerAnim.SetTrigger("Counter");
                PlayerAnim.SetBool("Counted", true);
                EnemyObj.EnemyDamage();
                MainGameObj.ParryCount++;
                Debug.Log("パリィ成功");
            }
        }

        //パリィ成功画面の削除
        if (MainGameObj.SpriteList[3].gameObject.activeSelf)
        {
            HanteiTime += Time.deltaTime;
            if (HanteiTime > 1.0f) MainGameObj.SpriteList[3].gameObject.SetActive(false);
        }


        //HPが0でリザルトへ
        if (MainGameObj.PlayerHp <= 0 && !playerlose)
        {
            playerlose = true;
            PlayerPrefs.SetInt("IsWin", 0);
            MainGameObj.toResult(EnemyObj.EnemyHP, EnemyObj.EnemyMaxHP);
        }
    }

    //Subscribe削除
    private void OnDestroy()
    {
        disposables_.Dispose();
    }
}
