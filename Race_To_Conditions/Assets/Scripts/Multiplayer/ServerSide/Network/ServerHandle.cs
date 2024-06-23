using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _Username = _packet.ReadString();

        Debug.Log($"{_Username} connected successfully and is now player {_fromClient}");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            //Maybe Disconnect player
        }
        else
        {
            Server.clients[_fromClient].InitializePlayer(_fromClient, _Username);
        }

        // TODO: send player into game
        GameObject.Find("Main Camera").GetComponent<PlayerHandler>().ServerStart(_fromClient, GameObject.Find("DontDestroy").GetComponent<DontDestroyOnLoad>().username);
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        Vector3 playerPosition = _packet.ReadVector3();
        Quaternion playerRotation = _packet.ReadQuaternion();
        Vector3 rightHandPos = _packet.ReadVector3();
        Vector3 leftHandPos = _packet.ReadVector3();

        Server.clients[_fromClient].player.SetMovement(playerPosition, playerRotation, rightHandPos, leftHandPos);
    }

    public static void PhysicsObjectUpdate(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        Vector3 position = _packet.ReadVector3();
        long sentTime = _packet.ReadLong();
        
        MainController.instance.controller.UpdatePhysicsObject(id, position, sentTime);
        
        ServerSend.PhysicsObjectUpdateFromClient(_fromClient, id, position, sentTime);
    }

    public static void ExperimentObjectUpdate(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        Vector3 position = _packet.ReadVector3();
        long sentTime = _packet.ReadLong();
        
        ExperimentController.instance.serverController.UpdatePhysicsObject(id, position, sentTime);
    }

    public static void PhysicsObjectDeselect(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        
        MainController.instance.controller.DeselectPhysicsObject(id);
        
        ServerSend.PhysicsObjectDeselectFromClient(_fromClient, id);
    }

    public static void ExperimentObjectDeselect(int _fromClient, Packet _packet)
    {
        int id = _packet.ReadInt();
        
        ExperimentController.instance.serverController.DeselectPhysicsObject(id);
    }
}
