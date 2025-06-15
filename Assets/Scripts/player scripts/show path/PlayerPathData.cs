using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerPathData", menuName = "Scriptable Objects/PlayerPathData")]
public class PlayerPathData : ScriptableObject
{
    public List<Vector3> positions = new List<Vector3>();
}
