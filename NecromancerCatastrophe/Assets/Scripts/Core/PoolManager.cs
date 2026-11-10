using System.Collections.Generic;
using UnityEngine;

namespace Necromancer.Core
{
    /// <summary>
    /// Объектный пул для миньонов, врагов, эффектов и душ.
    /// Уменьшает GC и Instantiate/Destroy на мобильных устройствах.
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

        void Awake()
        {
            Instance = this;

            foreach (var pool in pools)
            {
                var queue = new Queue<GameObject>();
                for (int i = 0; i < pool.size; i++)
                {
                    var obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    obj.transform.SetParent(transform);
                    queue.Enqueue(obj);
                }
                poolDictionary.Add(pool.tag, queue);
            }
        }

        public GameObject Spawn(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"[PoolManager] Пул с тегом {tag} не найден");
                return null;
            }

            var obj = poolDictionary[tag].Dequeue();
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            poolDictionary[tag].Enqueue(obj); // Вернуть в конец (циклический)
            return obj;
        }

        public void Despawn(string tag, GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(transform);
        }
    }
}
