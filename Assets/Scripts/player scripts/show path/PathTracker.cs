using UnityEngine;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor;

public class PathTracker : MonoBehaviour
{
    public Color pathColour;
    public PlayerPathData pathData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       //ClearPathData();
    }

    // Update is called once per frame
    void Update()
    {
        if (pathData != null && Application.isPlaying)
        {
            if (pathData.positions.Count == 0 || Vector3.Distance(pathData.positions[pathData.positions.Count - 1], transform.position) > 0.5) 
            pathData.positions.Add(transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        //if (pathData == null || pathData.positions.Count < 2) return;

        Gizmos.color = pathColour;
        for (int i = 1; i < pathData.positions.Count; i++)
        {

            Gizmos.DrawLine(pathData.positions[i - 1], pathData.positions[i]);

        }

    }

    [ContextMenu("Clear path data")]
    public void ClearPathData()
    {
        if (pathData != null)
        {
            Undo.RecordObject(pathData, "clear path");
            pathData.positions.Clear();
            EditorUtility.SetDirty(pathData);
        }
    }


}
