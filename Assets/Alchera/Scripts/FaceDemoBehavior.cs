// ---------------------------------------------------------------------------
//
// Copyright (c) 2018 Alchera, Inc. - All rights reserved.
//
// This example script is under BSD-3-Clause licence.
//
// Author
//       Park DongHa     | dh.park@alcherainc.com
//
// ---------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Alchera
{
    public interface IFaceDemo
    {
        IEnumerator TakeFaces(IEnumerable<Alchera.IFaceData> generator, int degree, bool isFront);
    }

    public class FaceDemoBehavior : MonoBehaviour, IFrameHandler
    {
        public Text VersionText;

        // Demo interface
        Alchera.IFaceDemo demo;

        Alchera.IFaceDetector detector;
        IEnumerable<Alchera.IFaceData> generator;

        bool frontFacing;
        int rotationDegree;

        void Start()
        {
            // Basically, IFaceDemo is a kind of face listener 
            demo = this.GetComponent<IFaceDemo>();

            generator = null; // Sequence of detected face
            detector = Alchera.Module.Face(Application.platform); // Load detector for each platform

            if (detector == null)
            {
                Debug.LogError("Failed to create face detector");
                Application.Quit();
            }

            // Display current plugin version
            VersionText.text = detector.Description;
        }

        // Release all resources of IFaceDetector
        // !!! This is mandatory !!!
        void OnDestroy()
        {
            detector.Dispose(); 
        }

        // Forward detected face to IFaceDemo
        void Update()
        {
            // Ensure non-null
            if(generator != null)
                StartCoroutine(demo.TakeFaces(generator, rotationDegree, frontFacing));
        }

        // Receive a frame and other info from ReadUnityCam
        void IFrameHandler.OnFrame(FrameData frame, int degree, bool isFront)
        {
            //VersionText.text = 
            //string.Format("Degree: {0} Orientation: {1}, {2}", degree, Input.deviceOrientation,
                            //(int)Input.deviceOrientation);
            
            // Reuse C# memory to boost performance
            this.generator = detector.Faces(ref frame, (uint)degree, generator);
            this.frontFacing = isFront;
            this.rotationDegree = degree;
        }
    }

}
