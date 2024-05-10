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

    void Start()
    {
        BattleTime = 0.0f;
    }

    void Update()
    {
        //タイム計測、
        if (GameStart) BattleTime += Time.deltaTime;
    }

    //プレイヤーのHPが0になったら呼び出す
    public void EnemyWin()
    {
        GameStart = false;
        /*
         ステージセレクト用に残した破壊不能objに
        敵HPの残り％
        戦闘時間を格納
        リザルトエネミーバージョンへ遷移
         */
    }

    //敵のHPが０になったときに呼び出す
    public void PlayerWin()
    {
        GameStart = false;
        /*
         ステージセレクト用に残した破壊不能objに
        戦闘時間パリィの成功率を格納
        リザルトプレイヤーバージョンへ遷移
         */
    }



}
