using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Dictionary<string, Queue<PooledObject>> poolDictionary;
    public List<Pool> pools;

    #region Singleton
    private ObjectPool()
    {

    }
    public static ObjectPool Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<PooledObject>>();
        foreach (Pool pool in pools)
        {
            Queue<PooledObject> objectPool = new Queue<PooledObject>();
            for (int i = 0; i < pool.size; i++)
            {
                PooledObject obj = Instantiate(pool.prefab);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    public IEnumerator WaitToDespawn(string tag, PooledObject gameObjectToDespawn, float timeToDespawn)
    {
        yield return new WaitForSeconds(timeToDespawn);
        DespawnObject(tag, gameObjectToDespawn);
        yield break;
    }
    public void DespawnObject(string tag, PooledObject goToQueue, float timeToDespawn = 0f)
    {
        if (timeToDespawn > 0)
        {
            StartCoroutine(WaitToDespawn(tag, goToQueue, timeToDespawn));
            return;
        }
        goToQueue.OnObjectDespawn();
        Enqueue(goToQueue, tag);
    }
    public PooledObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;
        }
        if (poolDictionary[tag].Count <= 0)
        {
            return null;
        }
        PooledObject objectToSpawn = poolDictionary[tag].Dequeue();


        objectToSpawn.transform.SetPositionAndRotation(position, rotation);
        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.OnObjectSpawn();
        return objectToSpawn;
    }

    public void Enqueue(PooledObject obj, string tag)
    {
        obj.gameObject.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }
}