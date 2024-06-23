using System;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static Data LoadData()
    {
        // Data tamp = new Data();
        // System.IO.File.WriteAllText("./Data/test.json", JsonUtility.ToJson(tamp));
        
        string file = "./Data/data.json";

        if (System.IO.File.Exists(file))
        {
            string data = System.IO.File.ReadAllText(file);
            return JsonUtility.FromJson<Data>(data);
        }
        else
        {
            return new Data();
        }
    }
}

[Serializable]
public struct Data
{
    public string ip;
    public int port;
    public string name;
}
