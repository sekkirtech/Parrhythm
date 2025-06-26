using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpManager : MonoBehaviour
{
    [SerializeField] private HpSprite[] _spriteMane;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetDamage(int x)
    {
        _spriteMane[x].SetDamage();
    }
}
