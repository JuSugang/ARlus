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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Alchera
{
    public interface IAnimoji
    {
        void SetLocal(Vector3 position, Quaternion rotation);

        void Animate(float[] param);
        void RotateEyes(Quaternion left, Quaternion right);

    }
}
