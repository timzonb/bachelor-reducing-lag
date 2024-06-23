using UnityEngine;

public class Player
{
    public int id;
    public string Username;

    public Vector3 position;
    public Quaternion rotation;
    public Vector3 rightHandPosition;
    public Vector3 leftHandPosition;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        Username = _username;

        position = new Vector3(0, 50, 0);
        rotation = Quaternion.identity;
    }

    public void SetMovement(Vector3 _position, Quaternion _rotation, Vector3 _rightHandPos, Vector3 _leftHandPos)
    {
        position = _position;
        rotation = _rotation;
        rightHandPosition = _rightHandPos;
        leftHandPosition = _leftHandPos;

        ServerSend.PlayerUpdate(this);
        
        MainController.instance.UpdatePlayer(id, _position, _rotation, _rightHandPos, _leftHandPos);
    }

    public void Disconnect()
    {
        ServerSend.DeSpawnPlayer(this);

        Debug.Log($"{Username}'s session has ended.");
    }
}
