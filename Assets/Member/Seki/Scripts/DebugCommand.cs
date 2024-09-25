using UnityEngine;

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


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if(Input.GetKey(KeyCode.C))
                {
                    if(Input.GetKey(KeyCode.H))
                    {
                        FadeManager.Instance.LoadScene("TitleScene", 0.6f);
                    }
                }
            }
        }
    }
}
