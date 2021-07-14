using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    [Header("Variables")] 
    public GameObject[] targets;
    private Save CreateSaveGameObject()
    {
        var save = new Save();
        foreach (var targetGameObject in targets)
        {
            var position = targetGameObject.transform.position;
            var rotation = targetGameObject.transform.rotation;
            
            Vector3Serializable serializableVector3 = new Vector3Serializable
            {
                x = position.x, y = position.y, z = position.z
            };
            QuaternionSerializable serializableQuaternion = new QuaternionSerializable
            {
                x = rotation.x, y = rotation.y, z = rotation.z, w = rotation.w
            };
            save.targetPositions.Add(serializableVector3);
            save.targetRotation.Add(serializableQuaternion);
        }
        return save;
    }
    public void SaveGameFile()
    {
        var save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
        
        Debug.Log("Game Saved");
    }
    public void LoadGameFile()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            
            for (var i = 0; i < save.targetPositions.Count; i++)
            {
                Vector3 vector3 = new Vector3()
                {
                    x = save.targetPositions[i].x, y = save.targetPositions[i].y, z = save.targetPositions[i].z
                };
                Quaternion quaternion = new Quaternion()
                {
                    x = save.targetRotation[i].x, y = save.targetRotation[i].y, z = save.targetRotation[i].z, w = save.targetRotation[i].w
                };
                targets[i].transform.position = vector3;
                targets[i].transform.rotation = quaternion;
            }

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
}
