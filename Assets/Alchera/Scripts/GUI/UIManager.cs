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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Alchera
{
    public class UIManager : MonoBehaviour
    {
        public Button quit, swap;

        void Start()
        {
            quit.onClick.AddListener(Application.Quit);
            swap.onClick.AddListener(()=>{
                WebCam.Swap();
            });
        }

    }
}