using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UniRx;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyObj;

    [SerializeField] private MainGameManager _mainGameObj;
    /// <summary>
    /// 敗北フラグ
    /// </summary>
    private bool _playerlose=false;

    private float _guardTime = 0.0f;

    private bool _guardButton = false;

    private bool _parryAttackButton = false;

    [SerializeField] private Animator _playerAnim;

    private bool _cancedGuardAnim=false;

    private bool _parryHits = false;

    private AudioSource _audioSource;

    [SerializeField] private AudioClip _slashClip;

    private CompositeDisposable _disposables=new CompositeDisposable();

    [SerializeField] private ParticleSystem _parryEffect;

    [SerializeField] private ParticleSystem _slashEffect;

    /// <summary>
    /// 斬撃エフェクトを何フレーム遅らせるか調整用
    /// </summary>
    private int _delayFlame = 20;


    void Start()
    {
        //初期化
        _guardTime = 0.0f;
        _playerlose = false;
        _parryEffect.gameObject.SetActive(false);
        _slashEffect.gameObject.SetActive(false);

        //nullチェック
        if (_enemyObj == null)
        {
            //Debug.Log("Enemyがないのでアタッチします");
            GameObject _enemy = GameObject.Find("Player");
            _enemyObj = _enemy.GetComponent<EnemyManager>();
        }
        //入力処理登録
        ControllerManager.Instance.L2ButtonObservable.Subscribe(x => _guardButton = true).AddTo(_disposables);
        ControllerManager.Instance.R2ButtonObservable.Subscribe(x => _guardButton = true).AddTo(_disposables);
        ControllerManager.Instance.L2ButtonUpObservable.Subscribe(x => _guardButton = false).AddTo(_disposables);
        ControllerManager.Instance.R2ButtonUpObservable.Subscribe(x => _guardButton = false).AddTo(_disposables);
        ControllerManager.Instance.WestButtonObservable.Subscribe(x => _parryAttackButton = true).AddTo(_disposables);
        ControllerManager.Instance.WestButtonUpObservable.Subscribe(x=>_parryAttackButton = false).AddTo(_disposables);
    }


    void Update()
    {
        //ガード中
        if (Input.GetKey(KeyCode.Space) || _guardButton)
        {
            //Debug.Log("ガード");

            //ガード中ではないとき
            if (!_mainGameObj._guardnow)
            {
                //キャンセルアニメーション加速
                _playerAnim.SetFloat("GuardCancelSpeed", 5f);

                //アニメーションスピード初期化
                _playerAnim.SetFloat("GuardActiveSpeed", 1f);
                _playerAnim.SetFloat("GuardIdleSpeed", 1f);

                //再生アニメーション整理
                _playerAnim.SetTrigger("GuardActive");
                _playerAnim.SetTrigger("GuardIdle");
                _playerAnim.SetBool("GuardCancel", false);
            }
            _cancedGuardAnim = true;
            //MainGamaManagerに状態を渡す
            _mainGameObj._guardnow = true;

            //タイム計測
            _guardTime += Time.deltaTime;

            //ガード開始してから0.5秒以内に攻撃が来たらパリィ可、超えたら不可に
            if (_guardTime > 0.5)
            {
                _mainGameObj.SetParryReception(false);
            }
            else
            {
                _mainGameObj.SetParryReception(true);
            }
        }
        else
        {
            //ガード解消時に諸々初期化等
            if(_mainGameObj._guardnow&& !_playerAnim.GetBool("Counter"))
            {
                _mainGameObj._guardnow=false;
                _cancedGuardAnim = false;

                //アニメーションスピード加速
                _playerAnim.SetFloat("GuardActiveSpeed", 5f);
                _playerAnim.SetFloat("GuardIdleSpeed", 5f);

                //アニメーションスピード初期化
                _playerAnim.SetFloat("GuardCancelSpeed", 1f);

                //アニメーション再生
                _playerAnim.SetBool("GuardIdle", false);
                _playerAnim.SetBool("Counted", false);
                _playerAnim.SetBool("GuardCancel", true);
            }
            //押してなければパリィ判定用カウント初期化
            _guardTime = 0.0f;
        }

        //パリィ可能時間内にP(□)でパリィ成功
        if (Input.GetKeyDown(KeyCode.Return) || _parryAttackButton)
        {
            //Debug.Log("ParryAttack" + _mainGameObj.ParryAttack);
            //Debug.Log("ParryHits" + _mainGameObj.ParryHits);

            //MainGameManagerの判定状況取得
            if (_parryHits)
            {
                //エフェクト停止
                _parryEffect.Stop();
                _slashEffect.Stop();

                //判定を下げる
                _parryHits = false;
                //今だ！画像非表示
                _mainGameObj.ExitTimingSp();
                //アニメーションスピード加速
                _playerAnim.SetFloat("GuardIdleSpeed", 5f);
                _playerAnim.SetFloat("GuardActiveSpeed", 5f);

                //Animation再生
                _playerAnim.SetBool("GuardCancel", false);
                _playerAnim.SetTrigger("GuardIdle");
                _playerAnim.SetTrigger("Counter");
                _playerAnim.SetBool("Counted", true);

                //敵にダメージ
                _enemyObj.EnemyDamage(1);

                //パリィカウント
                _mainGameObj.CountParry();

                //Debug.Log("パリィ成功");

                //斬撃SE挿入
                if (_audioSource == null) _audioSource = this.AddComponent<AudioSource>();
                //音量
                _audioSource.volume = 0.5f;
                //音源セット
                _audioSource.clip = _slashClip;
                //ループ設定
                _audioSource.loop = false;
                //再生
                _audioSource.Play();

                //エフェクト挿入（シーン遷移時に再生されないようfalseにしてある）
                _parryEffect.gameObject.SetActive(true);
                _parryEffect.Play();
                //斬撃エフェクト再生
                StartCoroutine(SlashCot(_delayFlame));

            }
        }


        //HPが0でリザルトへ
        if (_mainGameObj.GetPlayerHp() <= 0 && !_playerlose)
        {
            //負けフラグ
            _playerlose = true;
            //ゲーム停止
            _mainGameObj.SetGameEnd();
            //リザルトへ渡す情報
            PlayerPrefs.SetInt("IsWin", 0);
            //リザルト遷移
            if (!_mainGameObj._padVibration) _mainGameObj.toResult();
        }
    }

    //入力処理削除
    private void OnDestroy()
    {
        _disposables.Dispose();
    }

    /// <summary>
    /// 斬撃におけるアニメーションとエフェクトのタイミングを合わせるために再生するフレームを遅らせて再生する
    /// </summary>
    /// <param x="x">遅らせるフレーム数</param>
    /// <returns></returns>
    IEnumerator SlashCot(int x)
    {
        //指定フレーム数待つ
        for (var i = 0; i < x; i++)
        {
            yield return null;
        }
        //エフェクト再生（遷移時の誤爆防止でfalseにしてある）
        _slashEffect.gameObject.SetActive(true);
        _slashEffect.Play();
    }

    /// <summary>
    /// MainGameManagerからアニメーションを再生する際に使用
    /// </summary>
    /// <param x="x"></param>
    public void TriggerPlayerAnim(string x)
    {
        //Debug.Log(x + "再生");
        _playerAnim.SetTrigger(x);
    }

    public void SetParryHits(bool x)
    {
        _parryHits = x;
    }
}
