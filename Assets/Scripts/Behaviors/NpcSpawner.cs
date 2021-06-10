using System;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors
{
    public class NpcSpawner : MonoBehaviour
    {
        public GameObject npcPrefab;

        public GameObject CreateNpc()
        {
            return Instantiate(npcPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}