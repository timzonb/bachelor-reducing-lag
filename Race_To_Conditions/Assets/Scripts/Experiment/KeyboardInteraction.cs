using UnityEngine;

public class KeyboardInteraction : MonoBehaviour
{
    public DontDestroyOnLoad dontDestroyOnLoad;
    
    private void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            dontDestroyOnLoad.beServer = true;
            dontDestroyOnLoad.LoadScene("BasicScene");
        }
        else if (Input.GetKeyDown("c"))
        {
            dontDestroyOnLoad.beServer = false;
            dontDestroyOnLoad.LoadScene("BasicScene");
        }
    }
}
