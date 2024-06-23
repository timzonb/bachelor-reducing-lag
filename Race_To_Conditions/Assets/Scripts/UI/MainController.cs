using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [Header("References")]
    public GameObject playerPrefab;
    public Controller controller;
    
    [Header("Extra info")]
    public static MainController instance = null;
    
    public PlayerPositionData?[] players;

    public PlayerPositionData? serverPlayer;
    
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
    }

    private void Start()
    {
        players = new PlayerPositionData?[50];
    }

    public void SpawnServer(string username, Vector3 pos, Quaternion quaternion)
    {
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = pos;
        player.transform.rotation = quaternion;
        player.GetComponentInChildren<TMP_Text>().text = username;

        PlayerPositionData data = new PlayerPositionData
        {
            primary = player,
            rightHand = player.GetNamedChild("RightHand"),
            leftHand = player.GetNamedChild("LeftHand")
        };

        serverPlayer = data;
    }

    public void DeSpawnServer()
    {
        if (serverPlayer != null)
        {
            Destroy(serverPlayer.Value.primary);
            serverPlayer = null;
        }
    }

    public void UpdateServer(Vector3 position, Quaternion rotation, Vector3 rightHandPosition, Vector3 leftHandPosition)
    {
        serverPlayer!.Value.primary.transform.position = position;
        serverPlayer!.Value.primary.transform.rotation = rotation;
        serverPlayer!.Value.rightHand.transform.position = rightHandPosition;
        serverPlayer!.Value.leftHand.transform.position = leftHandPosition;
    }

    public void SpawnPlayer(int id, string userName, Vector3 pos, Quaternion quaternion)
    {
        GameObject player = Instantiate(playerPrefab);
        player.transform.position = pos;
        player.transform.rotation = quaternion;
        player.GetComponentInChildren<TMP_Text>().text = userName;

        PlayerPositionData data = new PlayerPositionData
        {
            primary = player,
            rightHand = player.GetNamedChild("RightHand"),
            leftHand = player.GetNamedChild("LeftHand")
        };

        players[id] = data;
    }
    
    public void UpdatePlayer(int id, Vector3 position, Quaternion rotation, Vector3 rightHandPosition, Vector3 leftHandPosition)
    {
        players[id]!.Value.primary.transform.position = position;
        players[id]!.Value.primary.transform.rotation = rotation;
        players[id]!.Value.rightHand.transform.position = rightHandPosition;
        players[id]!.Value.leftHand.transform.position = leftHandPosition;
    }

    public void DeSpawnPlayer(int id)
    {
        if (players[id] != null)
        {
            Destroy(players[id].Value.primary);
            players[id] = null;
        }
    }

    public void DebugPrint(string message)
    {
        Debug.Log(message);
    }
}

public struct PlayerPositionData
{
    public GameObject primary;
    public GameObject rightHand;
    public GameObject leftHand;
}
