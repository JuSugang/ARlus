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
using UnityEngine;

namespace Alchera
{
    public class PandaModel : MonoBehaviour, IAnimoji
    {
        public GameObject eyeLeft, eyeRight;

        Quaternion iR;          // Model's initial rotation
        Quaternion lR, rR;      // left/right initial rotation
        Matrix4x4 correction;   // Correction for face 3D position
                                // It's used to adjust each model's difference

        SkinnedMeshRenderer smr;// Renderer for blendshape
        Kalman filter;
        public float kalmanRatio;

        void Start()
        {
            // Since output rotation is flipped(rotated 180), we have to rotate the model.
            // Calculate the model Rotate in advance to save computation
            iR = Quaternion.AngleAxis(180, Vector3.forward) * this.transform.localRotation;

            this.transform.localPosition = new Vector3(0, 0, -30);
            // Position adjustment for each platform
#if UNITY_STANDALONE
            correction = Matrix4x4.Scale(new Vector3(6.5f, 6.2f, 9.3f));
            // this.transform.localScale = Vector3.one * 30;
            this.transform.localScale = this.transform.localScale * 30;
#elif UNITY_IPHONE
            correction  = Matrix4x4.Scale(new Vector3(15.2f, 15.2f, 9.1f));
            this.transform.localScale = this.transform.localScale * 30;
#elif UNITY_ANDROID
            correction  = Matrix4x4.Scale(new Vector3(8.1f, 8.1f, 9.1f));
            this.transform.localScale = this.transform.localScale * 46;
#else
            correction = Matrix4x4.Scale(new Vector3(6.5f, 6.2f, 9.3f));
            this.transform.localScale = this.transform.localScale * 32;
#endif
            // Acquire renderer for blendshape
            smr = this.GetComponentInChildren<SkinnedMeshRenderer>();
            if (smr == null)
                throw new UnityException("SkinnedMeshRenderer for this model is missing");

            filter = new Kalman(kalmanRatio);
        }

        // Parameter index might be different for each model.
        // So be cautious when implementing new AnimojiModel
        void IAnimoji.Animate(float[] animation)
        {
            // IFaceData holds 14 parameters
            smr.SetBlendShapeWeight(0, animation[0]);  // Brow_Outer_Up_Left   (왼쪽 눈썹의 바깥쪽 부분 올림)
            smr.SetBlendShapeWeight(1, animation[1]);  // Brow_Outer_Up_Right  (오른쪽 눈썹의 바깥쪽 부분 올림)
            smr.SetBlendShapeWeight(2, animation[2]);  // Brow_Down_Left       (왼쪽 눈썹의 바깥쪽 부분 내림)
            smr.SetBlendShapeWeight(3, animation[3]);  // Brow_Down_Right      (오른쪽 눈썹의 바깥쪽 부분 내림)            
            smr.SetBlendShapeWeight(4, animation[4]);  // Eye_Wide_Left        (왼쪽 눈 크게 뜸)
            smr.SetBlendShapeWeight(5, animation[5]);  // Eye_Wide_Right       (오른쪽 눈 크게 뜸)
            smr.SetBlendShapeWeight(6, animation[6]);  // Eye_Blink_Left       (왼쪽 눈 감음)
            smr.SetBlendShapeWeight(7, animation[7]);  // Eye_Blink_Right      (오른쪽 눈 감음)
            smr.SetBlendShapeWeight(8, animation[8]);  // Mouth_Smile_Left     (입 왼쪽 끝 위로 올림)
            smr.SetBlendShapeWeight(9, animation[9]);  // Mouth_Smile_Right    (입 오른쪽 끝 위로 올림)
            smr.SetBlendShapeWeight(10, animation[10]); // Mouth_Stretch_Left  (입 왼쪽 끝 왼쪽으로 움직임)
            smr.SetBlendShapeWeight(11, animation[11]); // Mouth_Stretch_Right (입 오른쪽 끝 오른쪽으로 움직임)            
            smr.SetBlendShapeWeight(12, animation[12]); // Mouth_Funnel        (입을 연 모양으로 입술 오므림)            
            smr.SetBlendShapeWeight(13, animation[13]); // Jaw_Open            (아랫 턱을 연 모양)
        }

        void IAnimoji.RotateEyes(Quaternion leftR, Quaternion rightR)
        {
            // EWMA for Smooth rotation
            lR = Quaternion.Slerp(lR, leftR, 0.5f);
            rR = Quaternion.Slerp(rR, rightR, 0.5f);

            eyeLeft.transform.localRotation = leftR;
            eyeRight.transform.localRotation = rightR;
        }

        void IAnimoji.SetLocal(Vector3 position, Quaternion rotation)
        {
            // Appliy Kalman Filter
            float[] z = { position.x, position.y, position.z, rotation.x, rotation.y, rotation.z, rotation.w };
            float[] nz = filter.Update(z, Time.deltaTime);

            // kalman-filtered position
            Vector3 np = new Vector3(nz[0], nz[1], nz[2]);
            // kalman-filtered rotation
            Quaternion nr = new Quaternion(nz[3], nz[4], nz[5], nz[6]);

            this.transform.localPosition = (correction * np);
            this.transform.localRotation = iR * nr;
        }

    }
}

