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
    //プレイヤーのHP
    int PlayerHp = 3;
    //敵オブジェクト
    [SerializeField] EnemyManager EnemyObj;
    //情報格納用マネージャー
    [SerializeField] MainGameManager MainGameObj;
    //ガード中フラグ
    bool Girdnow = false;
    //連打防止フラグ
    bool playerlose=false;
    //パリィ受付フラグ
    bool ParryReception = false;
    //パリィ可能フラグ
    bool ParryAttack = false;
    //ガード時間計測用
    float GirdTime = 0.0f;
    //パリィ成功時用連打対策
    bool ParryHits = false;
    //α版用
    private TextMeshProUGUI HPtext;
    private float HanteiTime = 0.0f;
    //コントローラー用bool
    private bool GirdButton = false;
    private bool ParryAttackButton = false;
    //HP画像格納
    [SerializeField] GameObject[] HpSprite;
    //HPダメージ画像格納
    [SerializeField] Sprite DamageHp;
    private Image myimage;
    //コントローラー格納
    Gamepad MyPad;

    private CompositeDisposable disposables_=new CompositeDisposable();


    void Start()
    {
        //初期化
        Girdnow = false;
        ParryReception = false;
        GirdTime = 0.0f;
        MainGameObj.SpriteList[1].gameObject.SetActive(false);
        MainGameObj.SpriteList[2].gameObject.SetActive(false);
        MainGameObj.SpriteList[3].gameObject.SetActive(false);
        playerlose = false;
        MyPad = Gamepad.current;
        //HP表示（長いので要改善）
        GameObject child = MainGameObj.SpriteList[4];
        child = child.transform.GetChild(0).gameObject;
        HPtext = child.GetComponent<TextMeshProUGUI>();

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
                MainGameObj.SpriteList[1].gameObject.SetActive(true);
                Girdnow = true;
                //タイム計測
                GirdTime += Time.deltaTime;
                //0.25秒以内でパリィ可、超えたら不可に
                if (GirdTime > 0.5)
                {
                    ParryReception = false;
                }
                else
                {
                    ParryReception = true;
                }
            }
            else
            {
                Girdnow = false;
                MainGameObj.SpriteList[1].gameObject.SetActive(false);
                //押してなければ初期化
                GirdTime = 0.0f;
            }

            //パリィ可能時間内にP(□)でパリィ成功
            if (Input.GetKeyDown(KeyCode.P)||ParryAttackButton && ParryAttack && ParryHits)
            {
                ParryHits = false;
                MainGameObj.SpriteList[3].gameObject.SetActive(true);
                HanteiTime = 0.0f;
                EnemyObj.EnemyDamage();
                MainGameObj.ParryCount++;
                Debug.Log("パリィ成功");
            }

            //パリィ成功画面の削除
            if (MainGameObj.SpriteList[3].gameObject.activeSelf)
            {
                HanteiTime += Time.deltaTime;
                if (HanteiTime > 1.0f) MainGameObj.SpriteList[3].gameObject.SetActive(false);
            }


            //HPが0でリザルトへ
            if (PlayerHp <= 0&&!playerlose)
            {
            playerlose = true;
            PlayerPrefs.SetInt("IsWin", 0);
                MainGameObj.toResult(EnemyObj.EnemyHP,EnemyObj.EnemyMaxHP);
            }
        }

    //Subscribe削除
    private void OnDestroy()
    {
        disposables_.Dispose();
    }



    /// <summary>
    /// 敵の攻撃が当たるときに敵オブジェクトから呼び出し
    /// ガードをしてなかったらダメージ
    /// パリィ受付時間内なら0.25秒だけパリィ可能にする
    /// </summary>
    public IEnumerator EnemmyAttack()
        {
            MainGameObj.AttackCount++;
            if (!Girdnow)
            {
                Debug.Log("ダメージを受けた！");
                PlayerHp--;
            //HP画像差し替え
            myimage = HpSprite[PlayerHp].GetComponent<Image>();
            myimage.sprite=DamageHp;
                yield break;
            }
            if (ParryReception)
            {
                Debug.Log("パリィ可能！");
                //連打防止用フラグ
                ParryHits = true;
                //パリィ可能か
                ParryAttack = true;
                MainGameObj.SpriteList[2].gameObject.SetActive(true);
                //可能になったらパッド振動
                MyPad.SetMotorSpeeds(1.0f, 1.0f);
            yield return new WaitForSeconds(0.15f);
            MyPad.SetMotorSpeeds(0.0f, 0.0f);
            yield return new WaitForSeconds(0.35f);
                ParryAttack = false;
                Debug.Log("パリイ終了");
                MainGameObj.SpriteList[2].gameObject.SetActive(false);
            }
        }
}
