using UnityEngine;

public class RandomizePitch : MonoBehaviour
{
    public float minPitch, maxPitch;

    private AudioSource source;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.pitch = Random.Range(minPitch, maxPitch);
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
