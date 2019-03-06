using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDropper : MonoBehaviour {

    [System.Serializable]
    public class myPool
    {
        public string poolTag;
        public GameObject bomb;
        public int total_Pool_Amt;
    }

    public static BombDropper Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<myPool> objectPools;
    public Dictionary<string, Queue<GameObject>> bombPoolDictionary;
    //this code was created using the help of brackeys on youtube

	// Use this for initialization
	void Start () {
        bombPoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (myPool pool in objectPools)
        {
            Queue<GameObject> bombPool = new Queue<GameObject>();
            for (int i = 0; i < pool.total_Pool_Amt; i++)
            {
               GameObject obj = Instantiate(pool.bomb);
               obj.SetActive(false);
               bombPool.Enqueue(obj);
            }

            bombPoolDictionary.Add(pool.poolTag, bombPool);
        }
	}
	
	public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    { 
        if (!bombPoolDictionary.ContainsKey(tag))
        {
            return null;
        }
        GameObject obj_To_Spawn = bombPoolDictionary[tag].Dequeue();

        obj_To_Spawn.SetActive(true);
        obj_To_Spawn.transform.position = position;
        obj_To_Spawn.transform.rotation = rotation;

        bombPoolDictionary[tag].Enqueue(obj_To_Spawn);

        return obj_To_Spawn;
    }
}
