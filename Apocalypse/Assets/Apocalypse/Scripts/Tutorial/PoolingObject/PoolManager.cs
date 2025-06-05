using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new();

    /// <summary>
    /// Lấy object từ pool nếu chưa có sẽ tự tạo
    /// </summary>
    public GameObject GetObject(GameObject prefab, Vector3 position = default, Quaternion rotation = default)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
        }

        GameObject obj;

        if (poolDictionary[prefab].Count > 0 && !poolDictionary[prefab].Peek().activeInHierarchy)
        {
            obj = poolDictionary[prefab].Dequeue();
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.transform.SetParent(this.transform);    
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        poolDictionary[prefab].Enqueue(obj);
        return obj;
    }

    /// <summary>
    /// Trả object về pool
    /// </summary>
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
// Gợi ý làm hiệu ứng
// GameObject fx = PoolManager.Instance.GetObject(
// explosionPrefab, transform.position, Quaternion.identity);

// ReturnToPool(fx)