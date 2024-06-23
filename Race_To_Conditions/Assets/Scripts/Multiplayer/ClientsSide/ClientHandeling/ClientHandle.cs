using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        int _myId = _packet.ReadInt();
        Client.instance.myId = _myId;

        ClientSend.WelcomeReceived();
        Client.instance.udp.Connenct(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        
        int objectCount = _packet.ReadInt();
        for(int i = 0; i < objectCount; i++)
        {
            int physicsObjectId = _packet.ReadInt();
            Vector3 postion = _packet.ReadVector3();
            Vector3 velocity = _packet.ReadVector3();
            
            MainController.instance.controller.UpdatePhysicsObject(physicsObjectId, postion, velocity); //Could be more perfomant, but this is easier
        }

        Debug.Log("Connected to the server as client!");
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        MainController.instance.SpawnPlayer(_id, username, _position, _rotation);
    }

    public static void DeSpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();

        MainController.instance.DeSpawnPlayer(_id);
    }

    public static void PlayerUpdate(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        Vector3 _rightHandPosition = _packet.ReadVector3();
        Vector3 _leftHandPostition = _packet.ReadVector3();

        MainController.instance.UpdatePlayer(_id, _position, _rotation, _rightHandPosition, _leftHandPostition);
    }

    public static void SpawnServer(Packet _packet)
    {
        string username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        MainController.instance.SpawnServer(username, _position, _rotation);
    }

    public static void ServerUpdate(Packet _packet)
    {
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        Vector3 _rightHandPosition = _packet.ReadVector3();
        Vector3 _leftHandPosition = _packet.ReadVector3();

        MainController.instance.UpdateServer(_position, _rotation, _rightHandPosition, _leftHandPosition);
    }

    public static void PhysicsObjectUpdate(Packet _packet)
    {
        int id = _packet.ReadInt();
        Vector3 position = _packet.ReadVector3();
        long sentTime = _packet.ReadLong();
        
        MainController.instance.controller.UpdatePhysicsObject(id, position, sentTime);
    }

    public static void ExperimentObjectUpdate(Packet _packet)
    {
        int id = _packet.ReadInt();
        Vector3 position = _packet.ReadVector3();
        long sentTime = _packet.ReadLong();
        
        ExperimentController.instance.clientController.UpdatePhysicsObject(id, position, sentTime);
    }

    public static void PhysicsObjectDeselect(Packet _packet)
    {
        int id = _packet.ReadInt();
        
        MainController.instance.controller.DeselectPhysicsObject(id);
    }

    public static void ExperimentObjectDeselect(Packet _packet)
    {
        int id = _packet.ReadInt();
        
        ExperimentController.instance.clientController.DeselectPhysicsObject(id);
    }
}
