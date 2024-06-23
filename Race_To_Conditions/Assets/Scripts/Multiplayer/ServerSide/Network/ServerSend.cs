using System;
using UnityEngine;

public class ServerSend
{
    #region Send TCP Data
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }

    private static void SendTCPDataToAllButOne(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }
    #endregion

    #region Send UDP Data
    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }

    private static void SendUDPDataToAllButOne(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }
    #endregion



    #region Packets
    public static void Welcome(int _toClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_toClient);
            
            _packet.Write(MainController.instance.controller.allObjects.Length);
            foreach (PhysicsObject physicsObject in MainController.instance.controller.allObjects)
            {
                _packet.Write(physicsObject.id);
                _packet.Write(physicsObject.State.Pos);
                _packet.Write(physicsObject.State.Vel);
            }

            SendTCPData(_toClient, _packet);
        }
    }

    public static void SelfSpawn(int id, string userName, Vector3 _position, Quaternion _rotation)
    {
        using (Packet _packet = new Packet((int)ServerPackets.serverSelfSpawn))
        {
            _packet.Write(userName);
            _packet.Write(_position);
            _packet.Write(_rotation);

            SendTCPData(id, _packet);
        }
    }

    public static void SelfUpdate(Vector3 _position, Quaternion _rotation, Vector3 _rightHandPosition, Vector3 _leftHandPosition)
    {
        using (Packet _packet = new Packet((int)ServerPackets.serverSelfUpdate))
        {
            _packet.Write(_position);
            _packet.Write(_rotation);
            _packet.Write(_rightHandPosition);
            _packet.Write(_leftHandPosition);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerUpdate(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerUpdate))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.position);
            _packet.Write(_player.rotation);

            SendUDPDataToAllButOne(_player.id, _packet);
        }
    }

    public static void SpawnPlayer(int _exceptClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.Username);
            _packet.Write(_player.position);
            _packet.Write(_player.rotation);

            SendTCPDataToAllButOne(_exceptClient, _packet);
        }
    }

    public static void DeSpawnPlayer(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.deSpawnPlayer))
        {
            _packet.Write(_player.id);

            SendTCPDataToAllButOne(_player.id, _packet);
        }
    }

    public static void PhysicsObjectUpdateFromServer(int id, Vector3 position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.physicsObjectUpdate))
        {
            _packet.Write(id);
            _packet.Write(position);
            _packet.Write(DateTime.Now.Ticks);

            SendTCPDataToAll(_packet);
        }
    }
    
    public static void ExperimentObjectUpdateFromServer(int id, Vector3 position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.experimentObjectUpdate))
        {
            _packet.Write(id);
            _packet.Write(position);
            _packet.Write(DateTime.Now.Ticks);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PhysicsObjectUpdateFromClient(int fromClient, int id, Vector3 position, long sentTime)
    {
        using (Packet _packet = new Packet((int)ServerPackets.physicsObjectUpdate))
        {
            _packet.Write(id);
            _packet.Write(position);
            _packet.Write(sentTime);

            SendTCPDataToAllButOne(fromClient, _packet);
        }
    }

    public static void PhysicsObjectDeselectFromServer(int objectId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.physicsObjectDeselect))
        {
            _packet.Write(objectId);
            
            SendTCPDataToAll(_packet);
        }
    }

    public static void PhysicsObjectDeselectFromClient(int fromClient, int objectId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.physicsObjectDeselect))
        {
            _packet.Write(objectId);
            
            SendTCPDataToAllButOne(fromClient, _packet);
        }
    }

    public static void ExperimentObjectDeselect(int objectId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.experimentObjectDeselect))
        {
            _packet.Write(objectId);
            
            SendTCPDataToAll(_packet);
        }
    }
    #endregion
}
