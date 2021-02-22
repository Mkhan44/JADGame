using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        //[Tooltip("MAKE SURE THIS TAG == THE NAME OF THE GAMEOBJECT WE ARE USING!")]
       // public string tag;
        public GameObject prefab;
        [Tooltip("How many of these do we want in the pool? If this is something that spawns quickly, this number should be higher.")]
        public int size;
    }

    [System.Serializable]
    public class ListWrapper
    {
        [Tooltip("What era is this?")]
        public string listName;

        public List<Pool> wrappedList;
    }

    public static Object_Pooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Tooltip("This number reflects the amount of pools. This needs to be manually entered for now.")]
    public int numPools;

    public List<Pool> pools;

    public List<ListWrapper> thePools;

    //The current list of prefabs that we're using. We will figure this out based on the time period we're in.
    List<GameObject> currentList = new List<GameObject>();


    /*
    //We want something like this...Need to figure out how to do this more efficiently.
    public List<Pool> prehistoricPools;
    public List<Pool> FeudalJapanPools;
    public List<Pool> WildWestPools;
    public List<Pool> MedPools;
    public List<Pool> FuturePools;
    */


    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        for(int i = 0; i < thePools.Count; i++)
        {
            foreach (Pool pool in thePools[i].wrappedList)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                //Fill out the entire pool by instantiating based on size.
                for (int j = 0; j < pool.size; j++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);

                }

                //Add the pool of objects to the dictionary.
                poolDictionary.Add(pool.prefab.name, objectPool);
               // Debug.Log("Added: " + pool.prefab.name + " To the dictionary!");
            }
        }

        //Debug.Log("The current time period is: " + Level_Manager.Instance.getTimePeriod());

        SetTimePeriodList(Level_Manager.Instance.getTimePeriod());

    }

    //If an item/enemy gets destroyed we instead add it back into the pool.
    //MAKE SURE THAT ENEMY NAMES MATCH THE TAG WE USE FOR POOLING THEM!!!!
    public void AddToPool(GameObject ObjToAdd)
    {
        ObjToAdd.SetActive(false);
        string nameToAdd = ObjToAdd.name.Replace("(Clone)", "");
        poolDictionary[nameToAdd].Enqueue(ObjToAdd);


    }

    //Wave spawner will pass in the position and rotation of these objects.
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag: " + tag + " doesn't exist!");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooled_Object pooledObj = objectToSpawn.GetComponent<IPooled_Object>();

        if(pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void SetTimePeriodList(Level_Manager.timePeriod TheTimePeriod)
    {
        //Clear the list so that nothing is conflicting when we re-fill it with new obstacles.
        currentList.Clear();
        currentList = new List<GameObject>();

        Debug.Log(TheTimePeriod);
        switch (TheTimePeriod)
        {
            case Level_Manager.timePeriod.Prehistoric:
                {
                    for(int i = 0; i < thePools.Count; i++)
                    {
                        if(thePools[i].listName == "PrehistoricPools")
                        {
                            //Essentially what we do here is: Add the items fo 'currentList' 
                            //Once we do that, we break out of this for loop that is checking for the names of the lists of pools.
                            //Finally, the new list is assigned and we can return that list.
                            for(int j = 0; j < thePools[i].wrappedList.Count; j++)
                            {
                                currentList.Add(thePools[i].wrappedList[j].prefab);
                            }
                            break;
                        }
                        else if (i == thePools.Count)
                        {
                            Debug.LogWarning("We didn't find the specified set of pools for: " + TheTimePeriod);
                            break;
                        }
                    }
                    break;
                }
            case Level_Manager.timePeriod.FeudalJapan:
                {
                    for (int i = 0; i < thePools.Count; i++)
                    {
                        if (thePools[i].listName == "FeudalJapanPools")
                        {
                            //Essentially what we do here is: Add the items fo 'currentList' 
                            //Once we do that, we break out of this for loop that is checking for the names of the lists of pools.
                            //Finally, the new list is assigned and we can return that list.
                            for (int j = 0; j < thePools[i].wrappedList.Count; j++)
                            {
                                currentList.Add(thePools[i].wrappedList[j].prefab);
                            }
                            break;
                        }
                        else if (i == thePools.Count)
                        {
                            Debug.LogWarning("We didn't find the specified set of pools for: " + TheTimePeriod);
                            break;
                        }
                    }
                    break;
                }
            case Level_Manager.timePeriod.WildWest:
                {
                    for (int i = 0; i < thePools.Count; i++)
                    {
                        if (thePools[i].listName == "WildWestPools")
                        {
                            //Essentially what we do here is: Add the items fo 'currentList' 
                            //Once we do that, we break out of this for loop that is checking for the names of the lists of pools.
                            //Finally, the new list is assigned and we can return that list.
                            for (int j = 0; j < thePools[i].wrappedList.Count; j++)
                            {
                                currentList.Add(thePools[i].wrappedList[j].prefab);
                            }
                            break;
                        }
                        else if(i == thePools.Count)
                        {
                            Debug.LogWarning("We didn't find the specified set of pools for: " + TheTimePeriod);
                            break;
                        }
                    }
                    break;
                }
            case Level_Manager.timePeriod.Medieval:
                {
                    for (int i = 0; i < thePools.Count; i++)
                    {
                        if (thePools[i].listName == "MedievalPools")
                        {
                            //Essentially what we do here is: Add the items fo 'currentList' 
                            //Once we do that, we break out of this for loop that is checking for the names of the lists of pools.
                            //Finally, the new list is assigned and we can return that list.
                            for (int j = 0; j < thePools[i].wrappedList.Count; j++)
                            {
                                currentList.Add(thePools[i].wrappedList[j].prefab);
                            }
                            break;
                        }
                        else if (i == thePools.Count)
                        {
                            Debug.LogWarning("We didn't find the specified set of pools for: " + TheTimePeriod);
                            break;
                        }
                    }
                    break;
                }
            case Level_Manager.timePeriod.Future:
                {
                    for (int i = 0; i < thePools.Count; i++)
                    {
                        if (thePools[i].listName == "FuturePools")
                        {
                            //Essentially what we do here is: Add the items fo 'currentList' 
                            //Once we do that, we break out of this for loop that is checking for the names of the lists of pools.
                            //Finally, the new list is assigned and we can return that list.
                            for (int j = 0; j < thePools[i].wrappedList.Count; j++)
                            {
                                currentList.Add(thePools[i].wrappedList[j].prefab);
                            }
                            break;
                        }
                        else if (i == thePools.Count)
                        {
                            Debug.LogWarning("We didn't find the specified set of pools for: " + TheTimePeriod);
                            break;
                        }
                    }
                    break;
                }

            //OTHER ERAS TO BE ADDED.
            default:
                {
                    Debug.LogWarning("We got to the default case!!!!");
                    for (int i = 0; i < thePools.Count; i++)
                    {
                        if (thePools[i].listName == "PrehistoricPools")
                        {
                            //Essentially what we do here is: Add the items fo 'currentList' 
                            //Once we do that, we break out of this for loop that is checking for the names of the lists of pools.
                            //Finally, the new list is assigned and we can return that list.
                            for (int j = 0; j < thePools[i].wrappedList.Count; j++)
                            {
                                currentList.Add(thePools[i].wrappedList[j].prefab);
                            }
                            break;
                        }
                        else if (i == thePools.Count)
                        {
                            Debug.LogWarning("We didn't find the specified set of pools in default");
                            break;
                        }
                    }
                    break;
                }
               
        }
        Debug.Log("The current list has been set.");
    }

    //This function will be used via WaveSpawner to grab the values of Currentlist and make that the current set of prefabs that we are using to spawn in during gameplay.
    public GameObject GetCurrentlistIndex(int index)
    {
        if(index < currentList.Count)
        {
            return currentList[index].gameObject;
        }
        else
        {
            Debug.LogWarning("Error, index not found in currentList!");
            return null;
        }
    }

    public int GetCurrentListCount()
    {
        return currentList.Count;
    }

}
