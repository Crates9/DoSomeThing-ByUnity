using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class VRGazeItem : MonoBehaviour
{

    //高亮材质  
    public Material highlightMat;
    //正常材质  
    public Material normalMat;


    void Start()
    {

    }


    void Update()
    {

    }

    //视线移入处理函数  
    public void GazeIn()
    {
        if (gameObject.tag == "GazeUI")
        {

        }
        else if (gameObject.tag == "GazeObj")
        {
            //给cube添加高亮时变成蓝色  
            gameObject.GetComponent<Renderer>().material = highlightMat;
        }

    }

    //视线移出处理函数  
    public void GazeOut()
    {
        if (gameObject.tag == "GazeUI")
        {

        }
        else if (gameObject.tag == "GazeObj")
        {
            //cube变成正常的颜色  
            gameObject.GetComponent<Renderer>().material = normalMat;
        }
    }

    //视线激活处理函数  
    public void GazeFire(RaycastHit hit)
    {
        if (gameObject.tag == "GazeUI")
        {
            SceneManager.LoadScene(1);
        }
        else if (gameObject.tag == "GazeObj")
        {
            //给物体cube作用一个力  
            gameObject.GetComponent<Rigidbody>().AddForceAtPosition(hit.point.normalized * 100, hit.point);
        }

    }
}