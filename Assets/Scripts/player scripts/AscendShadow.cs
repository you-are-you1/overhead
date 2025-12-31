using UnityEngine;

public class AscendShadow : MonoBehaviour
{
    private Ascend a;
    private SpriteRenderer ilovemonkeys;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        a = transform.parent.GetComponent<Ascend>();
        ilovemonkeys = GetComponent<SpriteRenderer>();
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        

        if (a.centerCheck && a.checkForAscend)
        {

            ilovemonkeys.enabled = true;
            transform.position = a.centerCheck.point;
        }
        else 
        {
            ilovemonkeys.enabled = false;
        } 
    }
}
