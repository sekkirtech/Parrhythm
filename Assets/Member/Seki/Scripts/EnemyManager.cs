using UnityEngine;

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
            Debug.LogError("PlayerManagerがアタッチされてません。");
        }
    }

    void Update()
    {
        //HPが０以下でリザルト
        if (EnemyHP <= 0&&EnemySlain)
        {
            //複数回読み込まないようフラグ
            EnemySlain = false;
            //勝ち
            PlayerPrefs.SetInt("IsWin", 1);
            //Scene遷移
            MainGameObj.toResult();
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