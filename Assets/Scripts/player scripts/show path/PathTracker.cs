using UnityEngine;
using System.Collections.Generic;

using UnityEditor;

public class PathTracker : MonoBehaviour
{
    public Color pathColour;
    public PlayerPathData pathData;

    private List<Vector3> currentPositionList = new List<Vector3>();
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    
      
       
    }

    private void OnDisable()
    {
        pathData.positionLists.Add(currentPositionList);
        while (pathData.positionLists.Count > 2)
        {
            pathData.positionLists.RemoveAt(0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (pathData != null && Application.isPlaying)
        {
            if (currentPositionList.Count == 0 || Vector3.Distance(currentPositionList[currentPositionList.Count - 1], transform.position) > 0.5) 
            currentPositionList.Add(transform.position);
            
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //if (pathData == null || pathData.positions.Count < 2) return;

        Gizmos.color = pathColour;
        foreach (List<Vector3> l in pathData.positionLists)
        {
            for (int i = 1;  i < l.Count; i++)
            {
                Gizmos.DrawLine(l[i - 1], l[i]);
            }
        }

    }

    [ContextMenu("Clear path data")]
    public void ClearPathData()
    {
        if (pathData != null)
        {
            Undo.RecordObject(pathData, "clear path");
            pathData.positionLists.Clear();
            EditorUtility.SetDirty(pathData);
        }
    }

#endif
}
