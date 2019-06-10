using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateScript : MonoBehaviour
{
    Vector3 StartPosition;  //左键按下时鼠标的位置
	Vector3 previousPosition;  //上一帧鼠标的位置。
	Vector3 offset;  //在两帧之间鼠标位置的偏移量，也就是这一帧鼠标的位置减去上一帧鼠标的位置。

    float angle=0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))  
		{  
			StartPosition = Input.mousePosition;  //记录鼠标按下的时候的鼠标位置
			previousPosition = Input.mousePosition;  //记录下当前这一帧的鼠标位置
		}  
		if (Input.GetMouseButton(1))  
		{  
			offset = Input.mousePosition - previousPosition; //这一帧鼠标的位置减去上一帧鼠标的位置就是鼠标的偏移量 
			previousPosition = Input.mousePosition; //再次记录当前鼠标的位置，以备下一帧求offset使用。

            angle=offset.x*0.3f;
            
            transform.RotateAround(Vector3.zero,new Vector3(0,1,0),angle);
		}  
    }

}
