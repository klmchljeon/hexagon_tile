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
        //�����̼� �ִϸ��̼� ������� ����, ����� �ٷ� ȸ��
        transform.eulerAngles = new Vector3(0,0,(transform.eulerAngles.z + 180)%360);
    }
}
