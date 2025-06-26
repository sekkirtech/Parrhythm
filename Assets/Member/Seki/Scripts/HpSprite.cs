using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpSprite : MonoBehaviour
{
    [SerializeField] private Image _sprite;

    [SerializeField] private Sprite _damageSprite;

    private void Start()
    {
        if (_sprite == null)
        {
            _sprite = GetComponent<Image>();
        }
    }

    public Image GetImageCom()
    {
        return _sprite;
    }

    public void SetDamage()
    {
        _sprite.sprite = _damageSprite;
    }
}
