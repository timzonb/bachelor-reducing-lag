using System;
using System.Threading;
using UnityEngine;

public class ExperimentController : MonoBehaviour
{
    [Header("Physics References")]
    public PhysicsObject sphereOne;
    public PhysicsObject sphereTwo;

    [Header("Client Network References")]
    public GameObject clientGameObject;
    public Client client;
    public Controller clientController;

    [Header("Server Network References")]
    public GameObject serverGameObject;
    public NetworkManager networkManager;
    public Controller serverController;
    
    private float time = 0.0f;
    private bool firstTime = true;

    public static ExperimentController instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        sphereOne.State.Pos = new Vector3(0, 10, 0);
        sphereTwo.State.Pos = new Vector3(0, 10, 0);

        networkManager.ip = "127.0.0.1";
        networkManager.port = 1777;
        serverGameObject.SetActive(true);
        
        Thread.Sleep(500);
        
        clientGameObject.SetActive(true);
        client.StartupConnection("127.0.0.1", 1777);
        client.ConnectToServer();

        Debug.Log(0.76f + 0.005f);
    }

    private void FixedUpdate()
    {
        if (firstTime && time > 15.0f)
        {
            UpdateClientSide(new Vector3(0, 15, 0));
            // UpdateServerSide(new Vector3(0, 12, -4));
            firstTime = false;
        }
        
        time += Time.fixedDeltaTime;
    }

    private void UpdateServerSide(Vector3 position)
    {
        serverController.UpdatePhysicsObject(0, position, DateTime.Now.Ticks);
        ServerSend.ExperimentObjectUpdateFromServer(0, position);
        ServerSend.ExperimentObjectDeselect(0);
    }
    
    private void UpdateClientSide(Vector3 position)
    {
        clientController.UpdatePhysicsObject(0, position, DateTime.Now.Ticks);
        ClientSend.ExperimentObjectUpdate(0, position);
        ClientSend.ExperimentObjectDeselect(0);
    }
}
