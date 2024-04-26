using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //プレイヤー格納
    [SerializeField] PlayerManager playerId;
    //敵のHP
    [SerializeField]int EnemyHP = 3;
    //拍
    int Beat = 0;
    //敵が攻撃した回数（パリィ率表記用）
    int AttackCount = 0;
    //BGM用Source
    [SerializeField] AudioClip[] BGMClip;
    //

    void Start()
    {
        if(playerId == null)
        {
            Debug.Log("Playerのスクリプトがないからアタッチ");
        }
    }

    void Update()
    {
        
    }
}
