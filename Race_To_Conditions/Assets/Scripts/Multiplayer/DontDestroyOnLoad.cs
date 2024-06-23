using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    public bool beServer = false;

    public int port;
    public string ip;

    public string username;

    private void Awake()
    {
        Debug.Log("Awake");

        SceneManager.sceneLoaded += OnSceneLoaded;

        //Can cause issues if we reload the scene, but currently not possible
        DontDestroyOnLoad(gameObject);

        Data data = SaveSystem.LoadData();
        ip = data.ip;
        port = data.port;
        username = data.name;
    }

    public void SetBeServer(bool value)
    {
        beServer = value;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        Debug.Log("Scene loaded with name: " + currentSceneName);

        if (currentSceneName == "BasicScene")
        {
            GameObject common = GameObject.Find("NetworkingCommon");
            ReferenceHolder references = common.GetComponent<ReferenceHolder>();

            if (beServer)
            {
                Debug.Log("Becoming Server");
                references.networkManager.ip = ip;
                references.networkManager.port = port;
                references.networkingServer.SetActive(true);
            }
            else
            {
                Debug.Log("Becoming Client");
                references.networkingClient.SetActive(true);
                references.client.StartupConnection(ip, port);
                references.client.ConnectToServer();
            }

            PlayerHandler playerHandler = GameObject.Find("Main Camera").GetComponent<PlayerHandler>();
            playerHandler.isServer = beServer;
            playerHandler.enabled = true;

            MainController.instance.controller.isServer = beServer;
        }
    }
}
