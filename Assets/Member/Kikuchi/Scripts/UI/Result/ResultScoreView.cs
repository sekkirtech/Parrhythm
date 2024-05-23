using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultSceneView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    public void SetText(TextMeshProUGUI tmPro, int score)
    {
        tmPro.text = score.ToString();
    }
}
