using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Save
{

    //SCRIPT QUE CONTROLA TODOS OS SAVES E LOADS DO JOGO

    public static void SavePlayer(PlayerData player)
    {
        BinaryFormatter formater = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerdata.prth";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = player;

        formater.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/playerdata.prth";
        if (File.Exists(path))
        {
            BinaryFormatter formater = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formater.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("ERROR: Arquivo Não Existente");
            return null;
        }
    }

}
