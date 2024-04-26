using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //プレイヤー格納
    [SerializeField] PlayerManager PlayerId;
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
        //nullチェック
        if(PlayerId == null)
        {
            Debug.Log("Playerのスクリプトがないからアタッチ");
            GameObject playerseki = GameObject.Find("Enemy");
            PlayerId = playerseki.GetComponent<PlayerManager>();
        }
        for (int i = 0; i < AttackObj.Length; i++)
        {
            if (AttackObj[i] == null) Debug.LogError(i + "番が存在しません");
        }
    }

    void Update()
    {
        
    }
}
