using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    //Beatフラグ
    public bool BeatFlag=true;

    void Start()
    {
        BattleTime = 0.0f;
        AttackCount = 0;
        ParryCount = 0;
        GameStart = true;
        ParryReception = false;
        MyPad = Gamepad.current;
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
/// 敵が攻撃開始時に呼び出し、パリィ可能かどうか判定
/// あとテスト用拍再生
/// </summary>
/// <param name="MAXCount">Beatの数</param>
/// <returns></returns>
    public IEnumerator EnemmyAttack(int MAXCount)
    {
        BeatAudioSource.clip = Beat;
        for (int i = 0; i < MAXCount; i++)
        {
            BeatAudioSource.Play();
            Debug.Log(i);
            if (i == (MAXCount-1))
            {
                SpriteList[0].SetActive(true);
            }
            yield return new WaitForSeconds(0.5f);
        }

        BeatAudioSource.clip = BeatFin;
        BeatAudioSource.Play();

        AttackCount++;
        if (!Girdnow)
        {
            PlayerDamage();
        }
        if (!guardController.UseGuard(GuardCost))
        {
            PlayerDamage();
            BeatFlag=true;
            SpriteList[0].SetActive(false);
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
        SpriteList[0].SetActive(false);
        BeatFlag = true;
    }

    void PlayerDamage()
    {
        Debug.Log("ダメージを受けた！");
        PlayerHp--;
        //HP画像差し替え
        myimage = HpSprite[PlayerHp].GetComponent<Image>();
        myimage.sprite = DamageHp;
    }
}
