using UnityEngine;
using System.Collections;
using BansheeGz.BGSpline.Components;

public class playercontrol : MonoBehaviour
{
    public GameObject trackLine;
    public float speed;
    private float angleSpeed= 5;
    // Use this for initialization
    void Start()
    {
        
        BGCcCursorChangeLinear linear = trackLine.GetComponent<BGCcCursorChangeLinear>();
        BGCcCursor cursor = trackLine.GetComponent<BGCcCursor>();
        linear.Speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = angleSpeed * Time.deltaTime;

        if (Input.GetKey("a"))
        {
            this.transform.Rotate(new Vector3(0, 0, -angle), Space.Self);
        }
        if (Input.GetKey("d"))
        {
            this.transform.Rotate(new Vector3(0, 0, angle), Space.Self);
        }
                        
    }
}
