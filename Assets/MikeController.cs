using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class MikeController : MonoBehaviour {
    public float sensitivity = 100;
    public float loudness = 0;
    private AudioSource audio;
    private string input;
    private float mTimer;
    // Use this for initialization
    void Start () {
        mTimer = 0f;

        //GetMicCaps();

        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = Microphone.Start(null, true, 100, 44100);
        audio.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) {
            Debug.Log("no mic input");
        }
        Debug.Log("start playing... position is " + Microphone.GetPosition(null));
        audio.Play();
    }
	
	// Update is called once per frame
	void Update () {
        //loudness = GetAveragedVolume() * sensitivity;

        //if (!Microphone.IsRecording(input))
        //{
        //    StartMicrophone();
        //}

        mTimer += Time.deltaTime;

        //if (mTimer >= mRefTime)
        //{
        //    StopMicrophone();
        //    StartMicrophone();
        //    mTimer = 0;
        //}
        

    }
    //void GetMicCaps()
    //{
    //    Microphone.GetDeviceCaps(input, out minFreq, out maxFreq);//Gets the frequency of the device

    //    if (maxFreq - minFreq < 1)
    //    {
    //        minFreq = 0;
    //        maxFreq = 44100;
    //    }
    //}

    //public void StartMicrophone()
    //{
    //    audio.clip = Microphone.Start(input, true, 10, maxFreq);//Starts recording
    //    while (!(Microphone.GetPosition(input) > 0)) { } // Wait until the recording has started
    //    audio.Play(); // Play the audio source!
    //}

    //public void StopMicrophone()
    //{
    //    audio.Stop();//Stops the audio
    //    Microphone.End(input);//Stops the recording of the device
    //}

    //float GetAveragedVolume()
    //{
    //    float[] data = new float[amountSamples];
    //    float a = 0;
    //    audio.GetOutputData(data, 0);
    //    foreach (float s in data)
    //    {
    //        a += Mathf.Abs(s);
    //    }
    //    return a / amountSamples;
    //}
}
