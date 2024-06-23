using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject leftHand;

    public bool isServer;

    public void ServerStart(int id, string username)
    {
        ServerSend.SelfSpawn(id, username, transform.position, transform.rotation);
    }
    
    private void FixedUpdate()
    {
        if (isServer)
        {
            ServerSend.SelfUpdate(transform.position, transform.rotation, rightHand.transform.position, leftHand.transform.position);
        }
        else
        {
            ClientSend.PlayerUpdate(transform.position, transform.rotation, rightHand.transform.position, leftHand.transform.position);
        }
    }
}
