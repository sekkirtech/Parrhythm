using UnityEngine;


/// <summary>
/// �o�O�Ői�s�s�\���Ƀ^�C�g���ɋ����J��
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
        //�딚�h�~�̂��ߕ�������
        if (Input.GetKey(KeyCode.T))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if(Input.GetKey(KeyCode.C))
                {
                    if(Input.GetKey(KeyCode.H))
                    {
                        //�J��
                        FadeManager.Instance.LoadScene("TitleScene", 0.6f);
                    }
                }
            }
        }
    }
}
