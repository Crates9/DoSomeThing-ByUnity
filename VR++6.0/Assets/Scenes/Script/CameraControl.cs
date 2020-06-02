using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    private float maxYRotation = 120;
    private float minYRotation = 0;

    private float maxXRotation = 60;
    private float minXRotation = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float xPosPrecent = Input.mousePosition.x / Screen.width;
        float yPosPrecent = Input.mousePosition.y / Screen.height;

        float xAngle = -Mathf.Clamp(
            yPosPrecent * maxXRotation,
            minXRotation,
            maxXRotation) - 140;
        float yAngle = Mathf.Clamp(
            xPosPrecent * maxYRotation,
            minYRotation,
            maxYRotation) - 20;

        transform.eulerAngles = new Vector3(xAngle, yAngle, 180);
    }
}
