using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ガードの管理を行うクラス
/// </summary>
public class GuardController : MonoBehaviour
{
    [SerializeField]
    private int maxStamina = 100; // 最大スタミナ

    private float currentStamina; // 現在のスタミナ

    [SerializeField]
    private Slider staminaSlider; // スタミナ表示用スライダー

    /// <summary>
    /// 初期設定を行います
    /// </summary>
    private void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    /// <summary>
    /// 毎フレームの更新を行います
    /// </summary>
    private void Update()
    {
        RegenerateStamina();
    }

    /// <summary>
    /// スタミナを回復します
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
    /// ガードを使用し、スタミナを消費します
    /// </summary>
    /// <param name="cost">消費するスタミナ</param>
    /// <returns>ガードが使用できたかどうか</returns>
    public bool UseGuard(float cost)
    {
        if (currentStamina < cost) return false;
        currentStamina -= cost;
        UpdateSlider();
        return true;
    }

    /// <summary>
    /// スタミナスライダーを更新します
    /// </summary>
    private void UpdateSlider()
    {
        staminaSlider.DOValue(currentStamina, 0.5f);
    }
}
