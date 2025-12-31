using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class AscendBlockEffect : MonoBehaviour
{
    public float destroyTime;
    private bool hasActivated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasActivated)
        {
            hasActivated = true;
            StartCoroutine(destroyObject());
        }
    }

    private IEnumerator destroyObject()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);

    }
}
