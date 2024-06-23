using UnityEngine;

public class ReferenceHolder : MonoBehaviour
{
    [Header("Client references")]
    public GameObject networkingClient;
    public Client client;
    
    [Header("Server references")]
    public GameObject networkingServer;
    public NetworkManager networkManager;
}
