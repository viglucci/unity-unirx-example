using System;
using System.Collections.Generic;
using Behaviors;
using UnityEngine;

public class Core : MonoBehaviour
{
    private long _lastSpawnedAt;
    
    private NpcSpawner _spawner;
    
    private int _nextId;

    private readonly Dictionary<int, GameObject> _objs
        = new Dictionary<int, GameObject>();
    
    // Start is called before the first frame update
    void Awake()
    {
        _spawner = gameObject.GetComponent<NpcSpawner>();
    }
    
    void Update()
    {
        if (ShouldSpawn())
        {
            var spawned = _spawner.CreateNpc();
            var objectId = ++_nextId;
            _objs.Add(objectId, spawned);
            _lastSpawnedAt = Now() + 5000;
        }
    }
    
    private bool ShouldSpawn()
    {
        return Now() >= _lastSpawnedAt;
    }

    private long Now()
    {
        return new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
    }
}
