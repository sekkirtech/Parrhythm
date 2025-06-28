using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //プレイヤー格納
    [SerializeField] private PlayerManager _playerObj;
    //敵のHP
    [SerializeField]private int _enemyMaxHP;
    private int _enemyHP = 0;
    //メインゲームマネージャー
    [SerializeField] private MainGameManager _mainGameObj;
    //討伐されてるか
    private bool _enemySlain=false;
    //メンバーが作成したHPバーscript格納
    [SerializeField] private HpBar _hpbar;
    //HP設定のためスコアデータが格納されてるスクリプトへアクセス
    [SerializeField] private EnemyNoteManager _noteMana;


    void Start()
    {
        //初期化
        _enemySlain=true;

        //敵HP調整
        int _stageNum = PlayerPrefs.GetInt("StageNum", 1);
        _enemyMaxHP = _noteMana.GetEnemyHp(_stageNum);

        //HP表示
        _hpbar.Init(_enemyMaxHP);
        _enemyHP = _enemyMaxHP;

        //nullチェック
        if (_playerObj == null)
        {
            Debug.LogError("PlayerManagerがアタッチされてません。");
        }
    }

    void Update()
    {
        //HPが０以下でリザルト
        if (_enemyHP <= 0 && _enemySlain)
        {
            //勝ち
            PlayerPrefs.SetInt("IsWin", 1);
            _mainGameObj.SetGameEnd();
            //Scene遷移
            if (!_mainGameObj._padVibration)
            {
                //複数回読み込まないようフラグ
                _enemySlain = false;
                _mainGameObj.toResult();

            }
        }
    }

    /// <summary>
    /// エネミー側がダメージを受けた時に使用
    /// </summary>
    /// <param name="x">受けたダメージ</param>
    public void EnemyDamage(int x)
    {
        _enemyHP-=x;
        _hpbar.SetHp(x);
    }

    public int[] GetEnemyEndHp()
    {
        return new int [] {_enemyHP,_enemyMaxHP};
    }
}