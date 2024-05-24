using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //敵オブジェクト
    [SerializeField] EnemyManager EnemyObj;
    //連打防止フラグ
    bool playerlose=false;
    //ガード時間計測用
    float GirdTime = 0.0f;
    //α版用
    private float HanteiTime = 0.0f;
    //コントローラー用bool
    private bool GirdButton = false;
    private bool ParryAttackButton = false;


    private CompositeDisposable disposables_=new CompositeDisposable();


    void Start()
    {
        //初期化
        GirdTime = 0.0f;
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
            if (Input.GetKey(KeyCode.Space)||GirdButton)
            {
                Debug.Log("ガード");
                MainGameManager.Instance.SpriteList[1].gameObject.SetActive(true);
                MainGameManager.Instance.Girdnow = true;
                //タイム計測
                GirdTime += Time.deltaTime;
                //0.25秒以内でパリィ可、超えたら不可に
                if (GirdTime > 0.5)
                {
                    MainGameManager.Instance.ParryReception = false;
                }
                else
                {
                    MainGameManager.Instance.ParryReception = true;
                }
            }
            else
            {
                MainGameManager.Instance.Girdnow = false;
                MainGameManager.Instance.SpriteList[1].gameObject.SetActive(false);
                //押してなければ初期化
                GirdTime = 0.0f;
            }

            //パリィ可能時間内にP(□)でパリィ成功
            if (Input.GetKeyDown(KeyCode.P)||ParryAttackButton && MainGameManager.Instance.ParryAttack && MainGameManager.Instance.ParryHits)
            {
                MainGameManager.Instance.ParryHits = false;
                MainGameManager.Instance.SpriteList[3].gameObject.SetActive(true);
                HanteiTime = 0.0f;
                EnemyObj.EnemyDamage();
                MainGameManager.Instance.ParryCount++;
                Debug.Log("パリィ成功");
            }

            //パリィ成功画面の削除
            if (MainGameManager.Instance.SpriteList[3].gameObject.activeSelf)
            {
                HanteiTime += Time.deltaTime;
                if (HanteiTime > 1.0f) MainGameManager.Instance.SpriteList[3].gameObject.SetActive(false);
            }


            //HPが0でリザルトへ
            if (MainGameManager.Instance.PlayerHp <= 0&&!playerlose)
            {
            playerlose = true;
            PlayerPrefs.SetInt("IsWin", 0);
                MainGameManager.Instance.toResult(MainGameManager.Instance.EnemyHP,MainGameManager.Instance.EnemyMaxHP);
            }
        }

    //Subscribe削除
    private void OnDestroy()
    {
        disposables_.Dispose();
    }
}
