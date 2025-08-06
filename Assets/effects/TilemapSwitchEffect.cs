using System.Collections;
using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class TilemapSwitchEffect : MonoBehaviour
{
    public LineRenderer linePrefab;

    private CompositeCollider2D tilemapCollider;

    public float expansionDistance;
    public float duration;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilemapCollider = GetComponent<CompositeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void showEffect(DottedTilemapScript d)
    {
        StartCoroutine(FindPathPoints());
        
    }

    private IEnumerator FindPathPoints()
    {
        yield return null; //wait one frame
        Debug.Log(tilemapCollider.pathCount);

        for (int i = 0; i < tilemapCollider.pathCount; i++)
        {

            Vector2[] pathPoints = new Vector2[tilemapCollider.GetPathPointCount(i)];
            tilemapCollider.GetPath(i, pathPoints);

            LineRenderer line = Instantiate(linePrefab);
            line.positionCount = pathPoints.Length + 1;

            for (int j = 0; j < pathPoints.Length; j++)
            {

                line.SetPosition(j, pathPoints[j] + new Vector2(0.5f, 0.5f));
            }

            line.SetPosition(pathPoints.Length, pathPoints[0] + new Vector2(0.5f, 0.5f));

            Vector2 centre = FindCentre(pathPoints);

            for (int k = 0; k < line.positionCount; k++)
            {
                int prev = (k - 1 + line.positionCount) % line.positionCount;
                int next = (k + 1) % line.positionCount;

                Vector2 tangent = (line.GetPosition(next) - line.GetPosition(prev)).normalized;
                Vector2 normal = new Vector2(-tangent.y, tangent.x);

                int index = k;
                Vector2 current = line.GetPosition(index);
                Vector2 target = current + normal * -expansionDistance;

                DOTween.To(() => current, x => line.SetPosition(index, x), target, duration);
            }
        }

    }

    private void OnEnable()
    {
        DottedTilemapScript.OnSwitchTilemapEvent += showEffect;
    }

    private void OnDisable()
    {
        DottedTilemapScript.OnSwitchTilemapEvent -= showEffect;
    }

    private Vector2 FindCentre(Vector2[] list)
    {
        float xSum = 0, ySum = 0;

        for (int i = 0; i < list.Length; i++)
        {
            xSum += list[i].x;
            ySum += list[i].y;
        }

        xSum /= list.Length;
        ySum /= list.Length;

        return new Vector2(xSum, ySum);
    }
}
