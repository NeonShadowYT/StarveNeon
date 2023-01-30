using UnityEngine;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

public static class BinarySavingSystem // static - нам нужна всего одна копия этого класса //https://www.youtube.com/channel/UC2pH2qNfh2sEAeYEGs1k_Lg
{


    public static void SavePlayer(Indicators indicators, CustomCharacterController player, InventoryManager inventoryManager, MoneySlot texnoSlot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.b";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(indicators,player,inventoryManager,texnoSlot);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.b";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }


    public static void SaveScene(Transform parentObject)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/scene.b";
        FileStream stream = new FileStream(path, FileMode.Create);
        SceneData data = new SceneData(parentObject);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SceneData LoadScene()
    {
        string path = Application.persistentDataPath + "/scene.b";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SceneData data = formatter.Deserialize(stream) as SceneData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }


    public static void SavePass(PassManager passManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/pass.b";
        FileStream stream = new FileStream(path, FileMode.Create);
        PassData data = new PassData(passManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PassData LoadPass()
    {
        string path = Application.persistentDataPath + "/pass.b";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PassData data = formatter.Deserialize(stream) as PassData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveQuast(QuastManager quastManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/quast.b";
        FileStream stream = new FileStream(path, FileMode.Create);
        QuastData data = new QuastData(quastManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static QuastData LoadQuast()
    {
        string path = Application.persistentDataPath + "/quast.b";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            QuastData data = formatter.Deserialize(stream) as QuastData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
