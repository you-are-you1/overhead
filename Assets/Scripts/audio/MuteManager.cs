using UnityEngine;

public class MuteManager : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        
        if (StartMenuScript.isMute) AudioListener.volume = 0f;
        else AudioListener.volume = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleAudio(StartMenuScript s)
    {
        AudioListener.volume = StartMenuScript.isMute ? 0f : 1f;
    }

    private void OnEnable()
    {
        StartMenuScript.MuteToggleEvent += ToggleAudio;
    }

    private void OnDisable()
    {
        StartMenuScript.MuteToggleEvent -= ToggleAudio;
    }
}
