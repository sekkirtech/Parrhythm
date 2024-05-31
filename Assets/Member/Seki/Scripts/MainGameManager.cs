using System.Collections;
using System.Collections.Generic;
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
    float BattleTime = 0.0f;
    //ゲームが開始してるか
    bool GameStart=false;
    //α視覚用obj
    [SerializeField] public GameObject[] SpriteList;
    //敵が攻撃した回数（パリィ率表記用）
    public int AttackCount = 0;
    //パリィ成功回数
    public int ParryCount = 0;
    //ガード中フラグ
    public bool Girdnow = false;
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
    [SerializeField] AudioClip Damage;
    private AudioSource AudioSource;


    void Start()
    {
        BattleTime = 0.0f;
        AttackCount = 0;
        ParryCount = 0;
        GameStart = true;
        ParryReception = false;
        MyPad = Gamepad.current;
        AudioSource=gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        //タイム計測、
        if (GameStart) BattleTime += Time.deltaTime;
    }

    //勝敗が決したときに呼び出す
    public void toResult(int EnemyHP,int EnemyMaxHP)
    {
        GameStart = false;
        //敵残HP
        PlayerPrefs.SetInt("CurrentHP", EnemyHP);
        //敵最大HP
        PlayerPrefs.SetInt("MaxHP", EnemyMaxHP);
        //戦闘時間
        PlayerPrefs.SetFloat("Time", BattleTime);
        //敵攻撃回数
        PlayerPrefs.SetInt("EnemyAttackCount", AttackCount);
        //パリィ成功回数
        PlayerPrefs.SetInt("ParryCount", ParryCount);


        FadeManager.Instance.LoadScene("ResultScene", 1.0f);
    }

    /// <summary>
    /// 敵の攻撃が当たるときに敵オブジェクトから呼び出し
    /// ガードをしてなかったらダメージ
    /// パリィ受付時間内なら0.25秒だけパリィ可能にする
    /// </summary>
    public IEnumerator EnemmyAttack()
    {
        AttackCount++;
        if (!Girdnow)
        {
            Debug.Log("ダメージを受けた！");
            AudioSource.clip = Damage;
            AudioSource.Play();
            PlayerHp--;
            //HP画像差し替え
            myimage = HpSprite[PlayerHp].GetComponent<Image>();
            myimage.sprite = DamageHp;
            yield break;
        }
        if (ParryReception)
        {
            Debug.Log("パリィ可能！");
            //連打防止用フラグ
            ParryHits = true;
            //パリィ可能か
            ParryAttack = true;
            SpriteList[2].gameObject.SetActive(true);
            //可能になったらパッド振動
            MyPad.SetMotorSpeeds(1.0f, 1.0f);
            yield return new WaitForSeconds(0.15f);
            MyPad.SetMotorSpeeds(0.0f, 0.0f);
            yield return new WaitForSeconds(0.35f);
            ParryAttack = false;
            Debug.Log("パリイ終了");
            SpriteList[2].gameObject.SetActive(false);
        }
    }
}
