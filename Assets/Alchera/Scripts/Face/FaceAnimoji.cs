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
using UnityEngine;
using System.Collections.Generic;

namespace Alchera
{
    public class FaceAnimoji : MonoBehaviour, IFaceDemo
    {
        Alchera.IFaceData face;
        Alchera.IAnimoji animoji;

        public Material material;

        Vector3 pos3d;
        Quaternion rot3d;
        Quaternion leftIris, rightIris;
        float[] weights;

        void Start()
        {
            weights = new float[14];
            animoji = this.GetComponentInChildren<IAnimoji>();

            if (animoji == null)
                throw new UnityException("No animoji model to use");

            Debug.LogFormat("{0} Demo initialized", typeof(FaceAnimoji).Name);
        }

        void OnDestroy()
        {
            Debug.LogFormat("{0} Demo disposed", typeof(FaceAnimoji).Name);
        }

        void Update()
        {
            if (face == null)
                return;

            // float* filtered = stackalloc float[10];

            animoji.SetLocal(position: pos3d,
                             rotation: rot3d);
            animoji.Animate(weights);            
            animoji.RotateEyes(leftIris, rightIris);
        }

        IEnumerator
            IFaceDemo.TakeFaces(IEnumerable<IFaceData> generator, 
                                int cameraAngle, bool isFront)
        {
            // inverse of camera rotation
            // Remove camera rotation from detector's output
            var invCam = Quaternion.AngleAxis(cameraAngle, Vector3.back);

            face = null;
            foreach (var f in generator)
            {
                face = f;
                face.Track3D();

                break;  // use only 1 face
            }

            if (face == null)
                yield break;
            
            // 검출된 Face의 '위치'를 적용하는 방법
            //  - 위치 초기값 + (위치 보정 * 카메라 회전 제거 * 위치 추정값)
            pos3d = invCam * face.Position;

            // 검출된 Face의 '회전'을 적용하는 방법
            //  - 회전 초기값 * 카메라 회전 제거 * 회전 추정값
            rot3d = invCam * face.Rotation;

            // 각각의 초기값은 Animoji 구현체에서 적용한다.

            face.GetAnimation(weights);
            face.GetIrisRotation(out leftIris, out rightIris);

            // For front camera, apply mirror effect
            // Simply, it is YZ plane mirroring
            if (isFront)
            {
                pos3d.x = -pos3d.x;

                rot3d.y = -rot3d.y;
                rot3d.z = -rot3d.z;
            
            //    leftIris.y = -leftIris.y;
            //    leftIris.z = -leftIris.z;
            
            //    rightIris.y = -rightIris.y;
            //    rightIris.z = -rightIris.z;
            }

            var tex2d = material.mainTexture as Texture2D;

            foreach(var point in face.Landmark)
            {
                tex2d.SetPixel((int)point.x, (int)point.y, Color.green);
            }
            tex2d.Apply(false);

        }
    }

}
