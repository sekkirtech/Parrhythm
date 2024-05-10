using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //プレイヤー格納
    [SerializeField] PlayerManager PlayerObj;
    //敵のHP
    [SerializeField]int EnemyHP = 3;
    //プレファブ
    [SerializeField] GameObject[] AttackObj;
    //情報格納用マネージャー
    [SerializeField] MainGameManager MainGameObj;
    //敵が攻撃した回数（パリィ率表記用）
    int AttackCount = 0;
    //BGM用Source
    //[SerializeField] AudioClip[] BGMClip;
    //討伐されてるか
    bool EnemySlain=false;

    void Start()
    {
        //初期化
        EnemySlain=false;

        //nullチェック
        if(PlayerObj == null)
        {
            Debug.Log("Playerのスクリプトがないからアタッチします");
            GameObject playerseki = GameObject.Find("Player");
            PlayerObj = playerseki.GetComponent<PlayerManager>();
        }
        for (int i = 0; i < 3; i++)
        {
            //プレハブ名が未確定のためエラーで表記
            //if (AttackObj[i] == null) Debug.LogError("攻撃オブジェクトの" + i + "番がありません");
        }
    }

    void Update()
    {
        //HPが０以下でリザルト（マイナス行くかもなので）
        if (EnemyHP > 0)
        {
            EnemySlain = true;
            MainGameObj.PlayerWin();
        }
    }
}
