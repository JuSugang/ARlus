using UnityEngine;
using System.Collections;
using BansheeGz.BGSpline.Components;

public class playercontrol : MonoBehaviour
{
    public GameObject line;
    // Use this for initialization
    void Start()
    {


        BGCcCursorChangeLinear a = line.GetComponent<BGCcCursorChangeLinear>();
        a.Speed = 100;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
