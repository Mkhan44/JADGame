//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

//Using Brackeys tutorial from YouTube.

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

        //test
        Cloud_Saving.instance.cloudTotalCoinsTest = collectmanager.totalCoins;
        Cloud_Saving.instance.SaveCloudData(Cloud_Saving.instance.cloudTotalCoinsTest);

        //test

        if (Cloud_Saving.instance.myCloudData != null)
        {
            Cloud_Saving.instance.myCloudData.totalCoins = collectmanager.totalCoins;

            Cloud_Saving.instance.SaveCloudData(Cloud_Saving.instance.cloudDataKey, Cloud_Saving.instance.myCloudData);

        }
        /*
        if (CloudOnce_Services.instance != null)
        {
            Debug.Log("Total coins in the cloud save was: " + CloudVariables.TotalCoins.ToString());
            CloudVariables.TotalCoins = collectmanager.totalCoins;
            Debug.Log("Total coins in the cloud save is now: " + CloudVariables.TotalCoins.ToString());

            CloudOnce_Services.instance.saveCloud();
        }
        */
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

    public static void DeleteCollectables()
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "CollectableSave.Rass");


        File.Delete(path);
    }

}
