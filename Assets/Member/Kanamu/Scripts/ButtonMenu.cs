using UnityEngine;
using System.Collections;
using UnityEngine.UI; // UIコンポーネントの使用

public class ButtonMenu : MonoBehaviour
{
    Button StageSelect;
    Button License;

    void Start()
    {
        // ボタンコンポーネントの取得
        StageSelect = GameObject.Find("/Canvas/Button").GetComponent<Button>();
        License = GameObject.Find("/Canvas/Button1").GetComponent<Button>();

        // 最初に選択状態にしたいボタンの設定
        StageSelect.Select();
    }
}