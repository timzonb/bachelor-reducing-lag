using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public int maxPlayers = 50;
    public string ip = "192.168.90.104";
    public int port = 1777;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already existed, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Constants.TICKS_PER_SEC;

        Server.Start(maxPlayers, ip, port);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }
}
