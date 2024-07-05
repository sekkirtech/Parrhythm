using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのアニメーションを制御するクラスです。
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;

    /// <summary>
    /// アニメーションの種類を定義する列挙型です。
    /// </summary>
    public enum AnimationType
    {
        Damage,
        GuardActive,
        GuardCancel,
        GuardDamage,
        Counter,
    }

    /// <summary>
    /// 初期化処理を行います。
    /// </summary>
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 指定したアニメーションのトリガーを設定します。
    /// </summary>
    /// <param name="animationType">設定するアニメーションの種類。</param>
    public void SetTrigger(AnimationType animationType)
    {
        // GuardCancel または Counter , GuardDamage の場合、GuardIdle 状態でのみトリガーを設定します。
        if ((animationType == AnimationType.GuardCancel || animationType == AnimationType.Counter || animationType == AnimationType.GuardDamage) &&
            _animator.GetCurrentAnimatorStateInfo(0).IsName("GuardIdle"))
        {
            _animator.SetTrigger(animationType.ToString());
            return;
        }

        // その他のアニメーションのトリガーを設定します。
        _animator.SetTrigger(animationType.ToString());
    }
}
