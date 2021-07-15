using System.Collections.Generic;
using Behaviors;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Core : MonoBehaviour
{
    [SerializeField] private Button spawnNpcButton;

    [SerializeField] private GameObject player;

    public GameObject Player => player;

    private NpcSpawner _spawner;

    private int _nextId;

    private readonly Dictionary<int, GameObject> _objs
        = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        RegisterObject(player);

        _spawner = gameObject.GetComponent<NpcSpawner>();
        spawnNpcButton.onClick.AddListener(OnSpawnNpcButtonClick);

        player
            .GetComponent<IdentityWorldPositionUpdateProvider>()
            .WorldPositionObservable
            .Subscribe(vector3 =>
            {
                // TODO: broadcast to a server and echo to other clients
                print("Player position change: " + vector3);
            });
    }

    private void OnSpawnNpcButtonClick()
    {
        SpawnNpc();
    }

    private void RegisterObject(GameObject o)
    {
        var objectId = GetNextObjectId();
        o.GetComponent<IdentityBehavior>().Id = objectId;
        _objs.Add(objectId, o);
    }

    private void SpawnNpc()
    {
        var spawned = _spawner.CreateNpc();
        RegisterObject(spawned);
    }

    public GameObject GetObjectById(int id)
    {
        _objs.TryGetValue(id, out var obj);
        return obj;
    }

    private int GetNextObjectId()
    {
        return ++_nextId;
    }
}