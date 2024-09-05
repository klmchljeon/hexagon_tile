using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Tile>().isRotateChanged += RotateAnimate;
    }
    
    void RotateAnimate(bool isRotate)
    {
        //로테이션 애니메이션 집어넣을 예정, 현재는 바로 회전
        transform.eulerAngles = new Vector3(0,0,(transform.eulerAngles.z + 180)%360);
    }
}
