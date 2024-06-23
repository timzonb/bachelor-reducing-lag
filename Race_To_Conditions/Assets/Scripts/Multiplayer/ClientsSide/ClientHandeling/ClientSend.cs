using System;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(GameObject.Find("DontDestroy").GetComponent<DontDestroyOnLoad>().username);
            
            SendTCPData(_packet);
        }
    }

    public static void PlayerUpdate(Vector3 _position, Quaternion _rotation, Vector3 _rightHandPos, Vector3 _leftHandPos)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_position);
            _packet.Write(_rotation);
            _packet.Write(_rightHandPos);
            _packet.Write(_leftHandPos);

            SendUDPData(_packet);
        }
    }

    public static void PhysicsObjectUpdate(int objectId, Vector3 position)
    {
        using (Packet _packet = new Packet((int)ClientPackets.physicsObjectUpdate))
        {
            _packet.Write(objectId);
            _packet.Write(position);
            _packet.Write(DateTime.Now.Ticks);
            
            SendTCPData(_packet);
        }
    }

    public static void ExperimentObjectUpdate(int objectId, Vector3 position)
    {
        using (Packet _packet = new Packet((int)ClientPackets.experimentObjectUpdate))
        {
            _packet.Write(objectId);
            _packet.Write(position);
            _packet.Write(DateTime.Now.Ticks);
            
            SendTCPData(_packet);
        }
    }

    public static void PhysicsObjectDeselected(int objectId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.physicsObjectDeselect))
        {
            _packet.Write(objectId);
            
            SendTCPData(_packet);
        }
    }

    public static void ExperimentObjectDeselect(int objectId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.experimentObjectDeselect))
        {
            _packet.Write(objectId);
            
            SendTCPData(_packet);
        }
    }
    #endregion
}
