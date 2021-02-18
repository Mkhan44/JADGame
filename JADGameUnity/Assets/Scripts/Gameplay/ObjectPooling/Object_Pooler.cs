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
        public int size;
    }

    public static Object_Pooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Tooltip("This number reflects the amount of pools. This needs to be manually entered for now.")]
    public int numPools;

    public List<Pool> pools;

    //We want something like this...Need to figure out how to do this more efficiently.
    public List<Pool> prehistoricPools;
    public List<Pool> FeudalJapanPools;
    public List<Pool> WildWestPools;
    public List<Pool> MedPools;
    public List<Pool> FuturePools;



    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            //Fill out the entire pool by instantiating based on size.
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);

            }

            //Add the pool of objects to the dictionary.
            poolDictionary.Add(pool.prefab.name, objectPool);
        }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
