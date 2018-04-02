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

namespace Alchera
{
    public class FaceMarking : MonoBehaviour, IFaceDemo
    {
        // Texture holder. The texture is used to draw face landmark
        public Material material;

        bool marking;
        Vector2[] landmark;
        
        void Start()
        {
            marking = true;
            landmark = new Vector2[106];

            Debug.LogFormat("{0} Demo initialized", typeof(FaceMarking).Name);
        }

        void OnDestroy()
        {
            Debug.LogFormat("{0} Demo disposed", typeof(FaceMarking).Name);
        }

        void Update()
        {
            if (Input.anyKeyDown)
                marking = !marking;
        }

        IEnumerator 
            IFaceDemo.TakeFaces(IEnumerable<IFaceData> generator, 
                                int degree, bool isFront)
        {
            foreach (IFaceData face in generator)
            {
                face.GetLandmark(landmark);
                face.Track3D();

                var texture = material.mainTexture as Texture2D;
                var color = Color.green;

                // Draw marks
                foreach (var point in landmark)
                {
                    int x = (int)point.x;
                    int y = (int)point.y;

                    texture.SetPixel(x + 1, y + 1, color);
                    texture.SetPixel(x + 0, y + 1, color);
                    texture.SetPixel(x - 1, y + 1, color);

                    texture.SetPixel(x + 1, y + 0, color);
                    texture.SetPixel(x + 0, y + 0, color);
                    texture.SetPixel(x - 1, y + 0, color);

                    texture.SetPixel(x + 1, y - 1, color);
                    texture.SetPixel(x + 0, y - 1, color);
                    texture.SetPixel(x - 1, y - 1, color);
                }
                // Apply the change
                texture.Apply(false);

                // use only 1 face
                break; 
            }

            yield break;
        }

    }

}
