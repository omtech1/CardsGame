using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class S
{
    public static int score = 0, fon; 
    public static bool pause = false, win;
     
    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.saving";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData();
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadData()
    {
        string path = Application.persistentDataPath + "/data.saving";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData a = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            fon = a.fon;
        }
        else
        {
            return;
        }
    }
}
