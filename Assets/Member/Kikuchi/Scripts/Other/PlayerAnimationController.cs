using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̃A�j���[�V�����𐧌䂷��N���X�ł��B
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;

    /// <summary>
    /// �A�j���[�V�����̎�ނ��`����񋓌^�ł��B
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
    /// �������������s���܂��B
    /// </summary>
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// �w�肵���A�j���[�V�����̃g���K�[��ݒ肵�܂��B
    /// </summary>
    /// <param name="animationType">�ݒ肷��A�j���[�V�����̎�ށB</param>
    public void SetTrigger(AnimationType animationType)
    {
        // GuardCancel �܂��� Counter , GuardDamage �̏ꍇ�AGuardIdle ��Ԃł̂݃g���K�[��ݒ肵�܂��B
        if ((animationType == AnimationType.GuardCancel || animationType == AnimationType.Counter || animationType == AnimationType.GuardDamage) &&
            _animator.GetCurrentAnimatorStateInfo(0).IsName("GuardIdle"))
        {
            _animator.SetTrigger(animationType.ToString());
            return;
        }

        // ���̑��̃A�j���[�V�����̃g���K�[��ݒ肵�܂��B
        _animator.SetTrigger(animationType.ToString());
    }
}
