using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardController : MonoBehaviour
{
    [SerializeField]
    private int maxStamina = 100;

    private float currentStamina;

    [SerializeField]
    private Slider _staminaSlider;

    private void Start()
    {
        currentStamina = maxStamina;
        _staminaSlider.maxValue = maxStamina;
        _staminaSlider.value = currentStamina;
    }

    private void Update()
    {
        if(currentStamina != maxStamina)
        {
            currentStamina += Time.deltaTime * 10;
            if(currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            _staminaSlider.value = currentStamina;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            UseGuard(25);
        }
    }

    public bool UseGuard(float cost)
    {
        if (currentStamina < cost) return false;
        currentStamina -= cost;
        UpdateSlider();
        return true;
    }

    private void UpdateSlider()
    {
        _staminaSlider.DOValue(currentStamina, 0.5f);
    }
}
