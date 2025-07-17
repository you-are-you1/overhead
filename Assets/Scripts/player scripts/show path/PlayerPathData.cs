using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerPathData", menuName = "Scriptable Objects/PlayerPathData")]
public class PlayerPathData : ScriptableObject
{
    
    public List<List<Vector3>> positionLists = new List<List<Vector3>>();
}
