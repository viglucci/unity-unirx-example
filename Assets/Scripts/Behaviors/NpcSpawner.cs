using System;
using UnityEngine;

namespace Behaviors
{
    public class NpcSpawner : MonoBehaviour
    {
        public GameObject npcPrefab;

        public GameObject CreateNpc()
        {
            var defaultPosition = new Vector3(0, 0, 0);
            return Instantiate(npcPrefab, defaultPosition, Quaternion.identity);
        }
    }
}