using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //プレイヤー格納
    [SerializeField] PlayerManager PlayerObj;
    //敵のHP
    [SerializeField]public int EnemyMaxHP = 3;
    public int EnemyHP = 0;
    //プレファブ
    [SerializeField] GameObject[] AttackObj;
    //情報格納用マネージャー
    [SerializeField] MainGameManager MainGameObj;
    //BGM用Source
    //[SerializeField] AudioClip[] BGMClip;
    //討伐されてるか
    bool EnemySlain=false;
    //α版用
    private TextMeshProUGUI HPtext;
    //HPバーscript格納
    [SerializeField] HpBar Bar;

    void Start()
    {
        //初期化
        EnemySlain=true;
        //HP表示
        Bar.Init(EnemyMaxHP);
        EnemyHP = EnemyMaxHP;

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
        if (EnemyHP <= 0&&EnemySlain)
        {
            EnemySlain = false;
            PlayerPrefs.SetInt("IsWin", 1);
            MainGameObj.toResult(EnemyHP,EnemyMaxHP);
        }
    }

    public void EnemyDamage()
    {
        //アニメーション挿入
        EnemyHP--;
        Bar.SetHp(1);
    }
}
