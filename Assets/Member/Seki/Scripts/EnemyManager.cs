﻿using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //プレイヤー格納
    [SerializeField] PlayerManager PlayerObj;
    //敵のHP
    [SerializeField]public int EnemyMaxHP = 3;
    public int EnemyHP = 0;
    //メインゲームマネージャー
    [SerializeField] MainGameManager MainGameObj;
    //討伐されてるか
    bool EnemySlain=false;
    //メンバーが作成したHPバーscript格納
    [SerializeField] HpBar Bar;
    //HP設定のためスコアデータが格納されてるスクリプトへアクセス
    [SerializeField] EnemyNoteManager NoteMana;


    void Start()
    {
        //初期化
        EnemySlain=true;

        //敵HP調性用24/10/23
        int StageNum = PlayerPrefs.GetInt("StageNum", 1);
        EnemyMaxHP = NoteMana.scoreData.GetListInScore(StageNum).GetEnemyHP();

        //HP表示
        Bar.Init(EnemyMaxHP);
        EnemyHP = EnemyMaxHP;

        //nullチェック
        if (PlayerObj == null)
        {
            Debug.LogError("PlayerManagerがアタッチされてません。");
        }
    }

    void Update()
    {
        //HPが０以下でリザルト
        if (EnemyHP <= 0&&EnemySlain)
        {
            //勝ち
            PlayerPrefs.SetInt("IsWin", 1);
            MainGameObj.GameStart=false;
            //Scene遷移
            if (!MainGameObj.PadVibration)
            {
                //複数回読み込まないようフラグ
                EnemySlain = false;
                MainGameObj.toResult();

            }
        }
        if (MainGameObj.TestMode)
        {
            EnemyHP=EnemyMaxHP;
        }
    }

    /// <summary>
    /// エネミー側がダメージを受けた時に使用
    /// </summary>
    /// <param name="Damage">受けたダメージ</param>
    public void EnemyDamage(int Damage)
    {
        EnemyHP-=Damage;
        Bar.SetHp(Damage);
    }
}