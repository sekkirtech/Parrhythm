using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //プレイヤーのHP
    int PlayerHp = 3;
    //敵オブジェクト
    [SerializeField] EnemyManager EnemyObj;
    //情報格納用マネージャー
    [SerializeField] MainGameManager MainGameObj;
    //ガード中フラグ
    bool Girdnow=false;
    //パリィ受付フラグ
    bool ParryReception = false;
    //パリィ可能フラグ
    bool ParryAttack=false;
    //長押し時用フラグ
    bool LongPushnow=false;
    //ガード時間計測用
    float GirdTime = 0.0f;
    //パリィ成功時用連打対策
    bool ParryHits = false;


    
    void Start()
    {
        //初期化
        Girdnow = false;
        ParryReception = false;
        LongPushnow = false;
        GirdTime = 0.0f;

        //nullチェック
        if (EnemyObj == null)
        {
            Debug.Log("Enemyがないのでアタッチします");
            GameObject enemyseki = GameObject.Find("Player");
            EnemyObj = enemyseki.GetComponent<EnemyManager>();
        }
    }

    void Update()
    {
        //ガード中
        if(Input.GetKey(KeyCode.Space))
        {
            Girdnow=true;
            //タイム計測
            GirdTime += Time.deltaTime;
            //0.25秒以内でパリィ可、超えたら不可に
            if (GirdTime > 0.25)
            {
                ParryReception = false;
            }
            else
            {
                ParryReception=true;
            }
        }
        else
        {
            Girdnow = false;
            //押してなければ初期化
            GirdTime=0.0f;
        }

        //パリィ可能時間内にP(□)でパリィ成功
        if(Input.GetKeyDown(KeyCode.P)&&ParryAttack&&ParryHits)
        {
            ParryHits = false;
            Debug.Log("パリィ成功");
        }
        //HPが0でリザルトへ
        if (PlayerHp == 0)
        {
            MainGameObj.EnemyWin();
        }

    }
    /// <summary>
    /// 敵の攻撃が当たるときに敵オブジェクトから呼び出し
    /// ガードをしてなかったらダメージ
    /// パリィ受付時間内なら0.25秒だけパリィ可能にする
    /// </summary>
     public IEnumerator EnemmyAttack()
    {
        if (!Girdnow)
        {
            PlayerHp--;
            yield break;
        }
        if (ParryReception)
        {
            ParryHits=true;
            ParryAttack = true;
            yield return new WaitForSeconds(0.25f);
            ParryAttack=false;
        }
    }
}
