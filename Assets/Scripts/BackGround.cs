using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackGround : MonoBehaviour
{
    public Transform mainCam;
    public Transform midBackGround;
    public Transform sideBackGround;
    public float length;

    private void Update()
    {
        if(mainCam.position.x > midBackGround.position.x)
        {
            UpdateBackGroundPosition(Vector3.right);
        }else if(mainCam.position.x < midBackGround.position.x)
        {
            UpdateBackGroundPosition(Vector3.left);
        }
    }

    private void UpdateBackGroundPosition(Vector3 dic)
    {
        sideBackGround.position = midBackGround.position + dic* length;
        Transform temp = midBackGround;
        midBackGround = sideBackGround;
        sideBackGround = temp;
    }
}
