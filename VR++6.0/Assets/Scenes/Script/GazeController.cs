using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeController : MonoBehaviour
{

    //准星容器  
    public Canvas reticleCanvas;
    //准星  
    public Image reticleImage;
    //击中的当前目标  
    public GameObject target;
    //初始位置  
    private Vector3 originPos;
    //初始缩放  
    private Vector3 originScale;
    //倒计时时间  
    private float countDownTime = 3;
    //当前时间  
    private float currentTime = 0;

    // Use this for initialization  
    void Start()
    {
        reticleImage.fillAmount = 0;
        //记录初始位置  
        originPos = reticleCanvas.transform.localPosition;
        //记录初始缩放  
        originScale = reticleCanvas.transform.localScale;
    }

    // Update is called once per frame  
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        //如果碰撞到了物体  
        if (Physics.Raycast(ray, out hit, 50))
        {
            //将碰撞的位置赋给准星  
            reticleCanvas.transform.position = hit.point;
            //根据距离进行缩放，补偿在3d世界中近大远小的情况  
            reticleCanvas.transform.localScale = originScale*hit.distance*2;
            //让准星与碰撞的物体垂直通过让准星与击中点法线方向一致  
            reticleCanvas.transform.forward = hit.normal;
            //视线初次进入  
            if (hit.transform.gameObject != target)
            {
                //如果上次的目标物体不为空，进行移出的操作  
                if (target != null)
                {
                    VRGazeItem oldItem = target.GetComponent<VRGazeItem>();
                    
                    if (oldItem)
                    {
                        oldItem.GazeOut();
                    }
                }

                //将击中的目标赋给当前的目标物体  
                target = hit.transform.gameObject;
                //获取物体上的凝视组件  
                VRGazeItem newItem = target.GetComponent<VRGazeItem>();
                //如果有凝视组件  
                if (newItem)
                {
                    //凝视  
                    newItem.GazeIn();
                }
            }
            else//视线在此停留  
            {
                currentTime += Time.deltaTime;
                //设定时间未结束  
                if (countDownTime - currentTime > 0)
                {
                    //设置进度条  
                    reticleImage.fillAmount = currentTime / countDownTime;
                }
                else//达到设定条件  
                {
                    currentTime = 0;

                    //如果  
                    VRGazeItem gazeFireItem = target.GetComponent<VRGazeItem>();
                    if (gazeFireItem)
                    {
                        gazeFireItem.GazeFire(hit);

                    }
                }
            }
        }
        //没有碰撞到物体  
        else
        {
            reticleCanvas.transform.localPosition = originPos;
            //缩放复位  
            reticleCanvas.transform.localScale = originScale;
            //准星方向复位  
            reticleCanvas.transform.forward = Camera.main.transform.forward;
            reticleImage.fillAmount = 0;
            currentTime = 0;
        }
    }
}
