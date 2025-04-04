using UnityEngine;


/// <summary>
/// バグで進行不能時にタイトルに強制遷移
/// </summary>
public class DebugCommand : MonoBehaviour
{
    private static DebugCommand instance;

    void Start()
    {
        if (DebugCommand.instance == null)
        {
            DebugCommand.instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }


    void Update()
    {
        //誤爆防止のため複数文字
        if (Input.GetKey(KeyCode.T))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if(Input.GetKey(KeyCode.C))
                {
                    if(Input.GetKey(KeyCode.H))
                    {
                        //遷移
                        FadeManager.Instance.LoadScene("TitleScene", 0.6f);
                    }
                }
            }
        }
    }
}
