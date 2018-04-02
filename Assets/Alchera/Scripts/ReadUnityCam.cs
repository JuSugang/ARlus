// ---------------------------------------------------------------------------
//
// Copyright (c) 2018 Alchera, Inc. - All rights reserved.
//
// This example script is under BSD-3-Clause licence.
//
// Author
//      JungWoo Chang   | jw.chang@alcherainc.com
//      Park DongHa     | dh.park@alcherainc.com
//
// ---------------------------------------------------------------------------
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;

namespace Alchera
{
    /// <summary>
    /// Helper class for <see cref="UnityEngine.WebCamTexture"/>'s ease of use.
    /// </summary>
    public static class WebCam
    {
#if UNITY_STANDALONE
        const int MaxWidth = 1280;
        const int MaxHeight = 720;
#else
        const int MaxWidth = 640;
        const int MaxHeight = 480;
#endif
        public static WebCamDevice[] DeviceList;
        public static WebCamTexture Front { get; private set; }
        public static WebCamTexture Rear { get; private set; }
        public static WebCamTexture Current { get; private set; }

        public static void Init()
        {
            // Already initialized
            if (DeviceList != null)
                return;

            DeviceList = WebCamTexture.devices;
            Debug.LogFormat("Camera Count: {0}", DeviceList.Length);
            foreach (var device in DeviceList)
            {
                if (device.isFrontFacing)
                {
                    Front = new WebCamTexture(device.name);
                    Front.requestedWidth = MaxWidth;
                    Front.requestedHeight = MaxHeight;
                    //Front.requestedFPS = 30;
                    Debug.LogFormat("Front: {0}", device.name);
                }
                else
                {
                    Rear = new WebCamTexture(device.name);
                    Rear.requestedWidth = MaxWidth;
                    Rear.requestedHeight = MaxHeight;
                    //Rear.requestedFPS = 30;
                    Debug.LogFormat("Rear: {0}", device.name);
                }
            }

            // Default camera is Rear
            if (Rear == null)
            {
                Current = Front;
                Debug.Log("Using Front Webcam...");
            }
            else
            {
                Current = Rear;
                Debug.Log("Using Rear Webcam...");
            }
        }

        public static bool Swap()
        {
            // Only 1 webcam. Can do nothing
            if (DeviceList.Length < 2)
                return false;

            bool playing = Current.isPlaying;
            // Stop
            if (playing)
                Current.Stop();

            // Swap
            if (Current == Front)
                Current = Rear;
            else
                Current = Front;

            // Play again ?
            if (playing)
                Current.Play();

            return true; // Camera changed!
        }
    }


    public interface IFrameHandler
    {
        void OnFrame(FrameData frame, Int32 degree, Boolean isFront);
    }

    public class ReadUnityCam : MonoBehaviour
    {
        public Material material;
        Texture2D texture;

        WebCamTexture webcam;
        Alchera.FrameData frame;
        Color32[] pixels;

        IFrameHandler handler;

        void Start()
        {
            frame = default(FrameData);

            handler = this.GetComponent<IFrameHandler>();
            if (handler == null)
                throw new UnityException("Frame handler must exist");

            Alchera.WebCam.Init();
            webcam = Alchera.WebCam.Current;

            // Camera output texture. A material will hold it
            texture = new Texture2D(webcam.width, webcam.height,
                                    TextureFormat.ARGB32, false);

            texture.Resize(webcam.requestedWidth, webcam.requestedHeight);
            material.mainTexture = texture;

            // Sizing issue can occur accroding to webcam's input size
            // We assume thate input will be **just like** requested size
            pixels = new Color32[webcam.requestedWidth * webcam.requestedHeight];

            webcam.Play();  // Start the webcam
        }

        void OnDestroy()
        {
            if (webcam != null)
                webcam.Stop();
        }

        void Update()
        {
            webcam = Alchera.WebCam.Current;
            if (webcam.width < 640)
            {
                Debug.LogWarning("Frame is too small. Skip.");
                return;
            }

            Profiler.BeginSample("Processing");

            // Sizing issue can occur accroding to webcam's input size
            webcam.GetPixels32(pixels);
           
            texture.SetPixels32(pixels);
            texture.Apply(false);

            // Process the frame to internal format
            frame.Read(texture.width, texture.height, pixels);

            Profiler.EndSample();

            Profiler.BeginSample("FrameHandling");
            // frame + degree + facing
            handler.OnFrame(frame,
                            webcam.videoRotationAngle,    // H/W camera rotation  
                            webcam == Alchera.WebCam.Front);
            Profiler.EndSample();
        }
        
    }

}
