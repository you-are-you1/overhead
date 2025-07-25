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

    Vector3 endPoint;
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

        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (AscendScript.checkForAscend)
        {
            if (!centerLine.activeSelf)
            {
                leftLine.SetActive(true);
                centerLine.SetActive(true);
                rightLine.SetActive(true);
            }
            
            if (AscendScript.centerCheck)
            {
                endPoint = AscendScript.centerCheck.point;
            }
            else
            {
                endPoint = transform.position;
                endPoint.y += Data.ascendRange;
            }

            centerLineRenderer.SetPosition(0, transform.position + (Vector3.up * 0.5f));
            centerLineRenderer.SetPosition(1, endPoint);
            leftLineRenderer.SetPosition(0, new Vector3(transform.position.x - 0.4f, transform.position.y + 0.5f, transform.position.z));
            leftLineRenderer.SetPosition(1, new Vector3(endPoint.x - 0.4f, endPoint.y, endPoint.z));
            rightLineRenderer.SetPosition(0, new Vector3(transform.position.x + 0.4f, transform.position.y + 0.5f, transform.position.z));
            rightLineRenderer.SetPosition(1, new Vector3(endPoint.x + 0.4f, endPoint.y, endPoint.z));


        }
        else if (centerLine.activeSelf)
        {
            centerLine.SetActive(false);
            rightLine.SetActive(false);
            leftLine.SetActive(false);
        }
    }
}
