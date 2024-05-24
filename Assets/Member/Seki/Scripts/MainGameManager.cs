using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainGameManager : MonoBehaviour
{
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


    void Start()
    {
        BattleTime = 0.0f;
        AttackCount = 0;
        ParryCount = 0;
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
        PlayerPrefs.SetInt("StageNum", EnemyHP);
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
}
