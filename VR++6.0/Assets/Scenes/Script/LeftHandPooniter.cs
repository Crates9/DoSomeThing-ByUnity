
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Highlighters;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LeftHandPooniter : MonoBehaviour
{
    private VRTK_Pointer _Pointer;
    public Color EnterColor, SelectColor;

    

    // Use this for initialization
    void OnEnable()
    {
        _Pointer = GetComponent<VRTK_Pointer>();
        _Pointer.DestinationMarkerEnter += _Pointer_DestinationMarkerEnter;
        _Pointer.DestinationMarkerExit += _Pointer_DestinationMarkerExit;
        _Pointer.DestinationMarkerSet += _Pointer_DestinationMarkerSet;
    }

    private void _Pointer_DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
    {
        //HighLight(e.target, SelectColor);


    }

    private void _Pointer_DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
    {
        HighLight(e.target, Color.clear);

    }

    private void _Pointer_DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
    {
        HighLight(e.target, EnterColor);

    }

    void OnDisable()
    {
        _Pointer.DestinationMarkerEnter -= _Pointer_DestinationMarkerEnter;
        _Pointer.DestinationMarkerExit -= _Pointer_DestinationMarkerExit;
        _Pointer.DestinationMarkerSet -= _Pointer_DestinationMarkerSet;

    }



    private void HighLight(Transform target, Color color)
    {
        VRTK_BaseHighlighter highlighter = (target !=
            null ? target.GetComponent<VRTK_BaseHighlighter>() : null);
        if (highlighter != null)
        {
            highlighter.Initialise();

            if (target.gameObject.tag == "1")
            {
                SceneManager.LoadScene(1);
            }
            if (target.gameObject.tag == "2")
            {
                SceneManager.LoadScene(0);
            }

            if (color != Color.clear)
            {
                highlighter.Highlight(color);
            }
            else
            {
                highlighter.Unhighlight();
            }
        }
    }




    // Update is called once per frame
    void Update()
    {

    }
}