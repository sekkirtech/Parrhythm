using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    //α版用
    private TextMeshProUGUI HPtext;

    void Start()
    {
        //初期化
        EnemySlain=false;
        //HP表示（長いので要改善）
        GameObject child = MainGameObj.SpriteList[5];
        child = child.transform.GetChild(0).gameObject;
        HPtext = child.GetComponent<TextMeshProUGUI>();

        //nullチェック
        if (PlayerObj == null)
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

        //HP更新
        if (HPtext != null)
        {
            HPtext.text = "EnemyHP:" + EnemyHP;
        }
        else
        {
            Debug.Log("プレイヤーテキストエラー");
        }
    }

    public void EnemyDamage()
    {
        //アニメーション挿入
        EnemyHP--;
    }
}
