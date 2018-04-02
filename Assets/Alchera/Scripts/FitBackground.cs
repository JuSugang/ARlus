/// ---------------------------------------------------------------------------
//
// Copyright (c) 2018 Alchera, Inc. - All rights reserved.
//
// This example script is under BSD-3-Clause licence.
//
// Author
//       Park DongHa     | dh.park@alcherainc.com
//
// ---------------------------------------------------------------------------
using UnityEngine;

namespace Alchera
{
    public class FitBackground : MonoBehaviour
    {
#if UNITY_STANDALONE || UNITY_STANDALONE_OSX
        const float factor = 0.001627f / 2;
#else
        const float factor = 0.001627f;
#endif

        // Assume this reference is Main Camera
        public Camera view;

        [Range(100, 200)]
        public float distance;

        Quaternion rotation;
        Vector3 scale;

        void Start()
        {
            if (distance > view.farClipPlane)
                throw new UnityException("Distance is over farClipPlane");
   
            // Initial rotation
            rotation = this.transform.rotation;

            // Send the plane to FAR position 
            // But it must be closer than the cliping plane
            this.transform.localPosition = view.transform.localPosition + new Vector3(0, 0, distance - 1);
        }

        void Update()
        {
            var webcam = WebCam.Current;
            if (webcam == null){
                Debug.LogWarning("Webcam not found");
                return;
            }
            
            // Scale it.
            //  the factor can be differ upon position of the plane
            scale.x = webcam.width * factor * distance;
            scale.y = webcam.height * factor * distance;

            this.transform.localScale = scale;

            // the value 1.920982f is from unity3d
            var Max = Mathf.Max(Screen.width, Screen.height) * 1.920982f;
            // Projection matrix
            var mat = view.projectionMatrix;
            mat[0, 0] = Max / Screen.width;
            mat[1, 1] = Max / Screen.height;
            view.projectionMatrix = mat;

            // It's front facing. Apply mirror effect
            if (webcam == Alchera.WebCam.Front)
            {
                var mirror = new Vector3(-scale.x, scale.y, scale.z);
                this.transform.localScale = mirror;
                this.transform.rotation = rotation * Quaternion.AngleAxis(
                    -webcam.videoRotationAngle, Vector3.back);
            }
            else
            {
                this.transform.localScale = scale;
                this.transform.rotation = rotation * Quaternion.AngleAxis(
                    webcam.videoRotationAngle, Vector3.back);
            }
        }

    }

}
