using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class microphoneInput : MonoBehaviour
{
    public float sensitivity = 100;
    public float loudness = 0;
    public Image redbar;
    private float posY = 0;
    AudioSource _audio;
    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.clip = Microphone.Start(null,true,10,44100);
        _audio.loop = true;
        //_audio.mute = true;
        while (!(Microphone.GetPosition(null) > 0)){}
        _audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
        loudness = GetAveragedVolume() * sensitivity;
        print(loudness);
        if (loudness > 4)
        {
            loudness = 4;
        }
        redbar.transform.localScale = new Vector3(loudness, 0.2f, 1f);
        
        if (loudness == 4)
        {
            posY += Time.deltaTime*0.1f;
            this.transform.Translate(new Vector3(0, posY, 0));
        }
    }
    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        _audio.GetOutputData(data, 0);
        foreach(float s in data)
        {
            a += Mathf.Abs(s);

        }
        //print(a);
        return a / 256;
    }
}
