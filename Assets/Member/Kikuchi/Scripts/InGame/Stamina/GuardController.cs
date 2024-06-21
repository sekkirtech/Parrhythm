using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �K�[�h�̊Ǘ����s���N���X
/// </summary>
public class GuardController : MonoBehaviour
{
    [SerializeField]
    private int maxStamina = 100; // �ő�X�^�~�i

    private float currentStamina; // ���݂̃X�^�~�i

    [SerializeField]
    private Slider staminaSlider; // �X�^�~�i�\���p�X���C�_�[

    /// <summary>
    /// �����ݒ���s���܂�
    /// </summary>
    private void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    /// <summary>
    /// ���t���[���̍X�V���s���܂�
    /// </summary>
    private void Update()
    {
        RegenerateStamina();
    }

    /// <summary>
    /// �X�^�~�i���񕜂��܂�
    /// </summary>
    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * 10;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            staminaSlider.value = currentStamina;
        }
    }

    /// <summary>
    /// �K�[�h���g�p���A�X�^�~�i������܂�
    /// </summary>
    /// <param name="cost">�����X�^�~�i</param>
    /// <returns>�K�[�h���g�p�ł������ǂ���</returns>
    public bool UseGuard(float cost)
    {
        if (currentStamina < cost) return false;
        currentStamina -= cost;
        UpdateSlider();
        return true;
    }

    /// <summary>
    /// �X�^�~�i�X���C�_�[���X�V���܂�
    /// </summary>
    private void UpdateSlider()
    {
        staminaSlider.DOValue(currentStamina, 0.5f);
    }
}
