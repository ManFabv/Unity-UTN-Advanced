using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject Prefab;
    [SerializeField] private int PreloadObjectAmmount = 20;
    [SerializeField] private string poolType = "Default";
#pragma warning restore 0649

    public string PoolType => poolType;
    
    private Queue<GameObject> pool = new Queue<GameObject>();
    private Transform cachedTransform;

    private void Awake()
    {
        cachedTransform = this.GetComponent<Transform>();
        for (int i = 0; i < PreloadObjectAmmount; i++)
        {
            PutObjectOnPool(cachedTransform);
        }
    }
    
    private void PutObjectOnPool(Transform shootSpawnPoint)
    {
        GameObject go = Instantiate(Prefab);
        SetObjectTransform(shootSpawnPoint, go);
        ReturnObjectToPool(go);
    }

    private void SetObjectTransform(Transform shootSpawnPoint, GameObject go)
    {
        go.transform.position = shootSpawnPoint.position;
        go.transform.rotation = shootSpawnPoint.rotation;
    }

    public GameObject GetObjectFromPool(Transform shootSpawnPoint)
    {
        if (pool.Count == 0)
        {
            PutObjectOnPool(shootSpawnPoint);
        }
        GameObject go = pool.Dequeue();
        SetObjectTransform(shootSpawnPoint, go);
        go.SetActive(true);
        return go;
    }

    public void ReturnObjectToPool(GameObject go)
    {
        go.SetActive(false);
        pool.Enqueue(go);
    }
}