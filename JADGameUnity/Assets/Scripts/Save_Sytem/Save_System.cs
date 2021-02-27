//Save system.

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Save_System 
{
   
    public static void SaveCollectables(Collect_Manager collectmanager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "CollectableSave.Rass");
        FileStream stream = new FileStream(path, FileMode.Create);

        CollectableData collectableData = new CollectableData(collectmanager);

        formatter.Serialize(stream, collectableData);
        stream.Close();
    }

    public static CollectableData LoadCollectables()
    {
        string path = Path.Combine(Application.persistentDataPath, "CollectableSave.Rass");


        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
              
               CollectableData collectableData = formatter.Deserialize(stream) as CollectableData;

                //Using statement auto-closes.
                return collectableData;

            }
            
        }
        else
        {
            Debug.LogWarning("Save file not found in: " + path);
            return null;
        }
        
       
        
    }

}
