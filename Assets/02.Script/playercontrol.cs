using UnityEngine;
using System.Collections;
using BansheeGz.BGSpline.Components;

public class playercontrol : MonoBehaviour
{
    public GameObject line;
    public GameObject playerAnchor;
    private float angleSpeed= 5;
    // Use this for initialization
    void Start()
    {
        BGCcCursorChangeLinear a = line.GetComponent<BGCcCursorChangeLinear>();
        a.Speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = angleSpeed * Time.deltaTime;

        if (Input.GetKey("a"))
        {
            playerAnchor.transform.Rotate(new Vector3(0, 0, -angle), Space.Self);
        }
        if (Input.GetKey("d"))
        {
            playerAnchor.transform.Rotate(new Vector3(0, 0, angle), Space.Self);
        }
                        
    }
}
