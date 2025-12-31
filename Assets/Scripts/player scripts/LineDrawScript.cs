using System.Xml.Serialization;
using UnityEngine;

public class LineDrawScript : MonoBehaviour
{
    private Ascend AscendScript;
    public PlayerDataWithDash Data;

    private GameObject leftLine;
    private GameObject centerLine;
    private GameObject rightLine;

    private LineRenderer leftLineRenderer;
    private LineRenderer centerLineRenderer;
    private LineRenderer rightLineRenderer;

    private TrailRenderer leftTrail;
    private TrailRenderer centerTrail;
    private TrailRenderer rightTrail;

    Vector3 centerPoint;
    Vector3 leftPoint;
    Vector3 rightPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        AscendScript = GetComponent<Ascend>();
        leftLine = transform.Find("LeftLine").gameObject;
        centerLine = transform.Find("CenterLine").gameObject;
        rightLine = transform.Find("RightLine").gameObject;

        leftLineRenderer = leftLine.GetComponent<LineRenderer>();
        centerLineRenderer = centerLine.GetComponent<LineRenderer>();
        rightLineRenderer = rightLine.GetComponent<LineRenderer>();

        leftTrail = leftLine.GetComponent<TrailRenderer>();
        centerTrail = centerLine.GetComponent<TrailRenderer>();
        rightTrail = rightLine.GetComponent<TrailRenderer>();

        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (AscendScript.checkForAscend)
        {
            if (!centerLineRenderer.enabled)
            {
                leftLineRenderer.enabled = true;
                centerLineRenderer.enabled = true;
                rightLineRenderer.enabled = true;
            }
            
            if (AscendScript.centerCheck)
            {
                centerPoint = AscendScript.centerCheck.point;
            }
            else
            {
                centerPoint = transform.position;
                centerPoint.y += Data.ascendRange;
            }

            if (AscendScript.leftCheck)
            {
                leftPoint = AscendScript.leftCheck.point;
            }
            else
            {
                leftPoint = transform.position;
                leftPoint.x -= AscendScript.sideCheckOffset;
                leftPoint.y += Data.ascendRange;
            }

            if (AscendScript.rightCheck)
            {
                rightPoint = AscendScript.rightCheck.point;
            }
            else
            {
                rightPoint = transform.position;
                rightPoint.x += AscendScript.sideCheckOffset;
                rightPoint.y += Data.ascendRange;
            }

            centerLineRenderer.SetPosition(0, transform.position + (Vector3.up * 0.6f));
            centerLineRenderer.SetPosition(1, centerPoint);
            leftLineRenderer.SetPosition(0, transform.position + new Vector3(-AscendScript.sideCheckOffset, 0.6f, 0f));
            leftLineRenderer.SetPosition(1, leftPoint);
            rightLineRenderer.SetPosition(0, transform.position + new Vector3(AscendScript.sideCheckOffset, 0.6f, 0f));
            rightLineRenderer.SetPosition(1, rightPoint);


        }
        else if (centerLineRenderer.enabled)
        {
            centerLineRenderer.enabled = false;
            rightLineRenderer.enabled = false;
            leftLineRenderer.enabled = false;
        }
    }

    public void enableTrails()
    {
        centerTrail.emitting = true;
        leftTrail.emitting = true;
        rightTrail.emitting = true;
    }

    public void disableTrails()
    {
        centerTrail.emitting = false;
        leftTrail.emitting = false;
        rightTrail.emitting = false;
    }
}
