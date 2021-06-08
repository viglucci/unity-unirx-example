using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Path", order = 1)]
public class Path : ScriptableObject
{
    public List<Vector2> checkPoints;
}