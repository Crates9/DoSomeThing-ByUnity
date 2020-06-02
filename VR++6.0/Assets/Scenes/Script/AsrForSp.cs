using UnityEngine;
using UnityEngine.UI;
using Wit.BaiduAip.Speech;
using UnityEngine.Video;

public class AsrForSp : MonoBehaviour
{
    public string APIKey = "";
    public string SecretKey = "";
    public Button StartButton;
    public Button StopButton;
    public Text DescriptionText;
    public AudioSource ap;
    private AudioClip _clipRecord;
    private Asr _asr;
    public GameObject lightmain;
    public GameObject light1;
    public GameObject light2;
    public GameObject light3;
    public GameObject light4;
    public GameObject light5;
    public GameObject light6;
    public GameObject light7;
    public GameObject light8;
    public ParticleSystem smoke1;
    public ParticleSystem smoke2; public ParticleSystem smoke3; public ParticleSystem smoke4;
    public ParticleSystem smoke5; public ParticleSystem smoke6; public ParticleSystem smoke7; public ParticleSystem smoke8;
    public ParticleSystem cock1;
    public ParticleSystem cock2; public ParticleSystem cock3; public ParticleSystem cock4;
    public ParticleSystem cock5; public ParticleSystem cock6; public ParticleSystem cock7; public ParticleSystem cock8;
    public GameObject pjt;
    public GameObject patient;

    public GameObject door1; public GameObject door2; public GameObject door3; public GameObject door4; public GameObject door5;
    public GameObject door6; public GameObject door7; public GameObject door8;


    //  Microphone is not supported in Webgl
#if !UNITY_WEBGL

    static int ToDigit(char cn)
    {//Convert Chinese Characters to digit figures
        int number;
        switch (cn)
        {
            case '壹':
            case '一':
            case '腰':
            case '约':
                number = 1;
                break;
            case '两':
            case '贰':
            case '俩':
            case '二':
                number = 2;
                break;


            case '叁':
            case '三':
                number = 3;
                break;

            case '肆':
            case '四':
                number = 4;
                break;

            case '伍':
            case '五':
                number = 5;
                break;

            case '陆':

            case '六':

                number = 6;

                break;

            case '柒':

            case '七':

                number = 7;

                break;

            case '捌':

            case '八':

                number = 8;

                break;

            case '玖':

            case '九':

                number = 9;

                break;

            case '拾':

            case '十':

                number = 10;
                break;

            case '零':

            default:

                number = 0;

                break;

        }

        return number;

    }
    int dogs;
    void Start()
    {
        _asr = new Asr(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());

        StartButton.gameObject.SetActive(true);
        StopButton.gameObject.SetActive(false);
        DescriptionText.text = "";

        StartButton.onClick.AddListener(OnClickStartButton);
        StopButton.onClick.AddListener(OnClickStopButton);
        

    }


    private void OnClickStartButton()
    {   // if U puts start button down
        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(true);
        DescriptionText.text = "Listening...";

        _clipRecord = Microphone.Start(null, false, 30, 16000);
    }
    private string message;
    private string message2;

    private void OnClickStopButton()
    {
        dogs = pjt.GetComponent<AsrForPjt>().dog;
        Debug.Log("sssss" + "," + dogs);
        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        DescriptionText.text = "Recognizing...";
        Microphone.End(null);


        var data = Asr.ConvertAudioClipToPCM16(_clipRecord);
        StartCoroutine(_asr.Recognize(data, s =>
        {

            message = s.result != null && s.result.Length > 0 ? s.result[0] : "未识别到声音";
            char[] messageChar = message.ToCharArray(); //Convenient access by index
            if ((message.IndexOf("病房")) == -1) Debug.Log("repeat");
            else {      //Start Algorithm


            int temporary = 1;
            int chamber = ToDigit(messageChar[(message.IndexOf("病房") - temporary)]);      
            for (temporary = 1; temporary < message.IndexOf("病房");)
                { //if there's another character before"病房" like "第六个病房" etc. still Still searching forward
                if (chamber == 0)
                {
                    chamber = ToDigit(message[(message.IndexOf("病房") - (++temporary))]);
                }
                else if (chamber != 0)
                {// we got it!
                    break;
                }
            }

            message2 = s.result != null && s.result.Length > 0 ? s.result[0] : "未识别到声音";
            //0none，1开灯2关灯3消毒4开门
            int operate = 0;
            string[] operates = { "", "开灯", "关灯", "消毒", "开门" };
            for (int i = 1; i <= operates.Length;)
            {
                int t = message2.IndexOf(operates[i]);
                if (t == (-1))
                {
                    i++;
                }
                else
                {
                    operate = i;
                    break;
                }
            }//用indexof寻找关键字，没找到返回-1 找到了赋给operate
             // chamber 1，2，3，4，5，6，7，8
             // operate 1，2，3，4



            if (chamber == 1 && operate == 1)
            {
                light1.SetActive(true);
            }
            else if (chamber == 1 && operate == 2)
            {
                light1.SetActive(false);
            }
            else if (chamber == 1 && operate == 3)
            {
                Debug.Log(dogs);
                if (dogs == 1)
                {
                    ap.Play();
                }
                else
                {
                    smoke1.Play();
                    cock1.Stop();
                }
            }
            else if (chamber == 1 && operate == 4)
            {
                door1.transform.position = new Vector3(door1.transform.position.x,
                    door1.transform.position.y, door1.transform.position.z+2);
                
            }

            if (chamber == 2 && operate == 1)
            {
                light2.SetActive(true);
            }
            else if (chamber == 2 && operate == 2)
            {
                light2.SetActive(false);
            }
            else if (chamber == 2 && operate == 3)
            {
                Debug.Log(dogs);
                if (dogs == 2)
                {
                    ap.Play();
                }
                else
                {
                    smoke2.Play();
                    cock2.Stop();
                }
            }
            else if (chamber == 2 && operate == 4)
            {
                door2.transform.position = new Vector3(door2.transform.position.x,
                    door2.transform.position.y, door2.transform.position.z + 2);

            }

            if (chamber == 3 && operate == 1)
            {
                light3.SetActive(true);
            }
            else if (chamber == 3 && operate == 2)
            {
                light3.SetActive(false);
            }
            else if (chamber == 3 && operate == 3)
            {
                Debug.Log(dogs);
                if (dogs == 3)
                {

                    ap.Play();
                }
                else
                {
                    smoke3.Play();
                    cock3.Stop();
                }
            }
            else if (chamber == 3 && operate == 4)
            {
                door3.transform.position = new Vector3(door3.transform.position.x,
                    door3.transform.position.y, door3.transform.position.z + 2);

            }
            if (chamber == 4 && operate == 1)
            {
                light4.SetActive(true);
            }
            else if (chamber == 4 && operate == 2)
            {
                light4.SetActive(false);
            }
            else if (chamber == 4 && operate == 3)
            {
                Debug.Log(dogs);
                if (dogs == 4)
                {
                    ap.Play();
                }
                else
                {
                    smoke4.Play();
                    cock4.Stop();
                }
            }
            else if (chamber == 4 && operate == 4)
            {
                door4.transform.position = new Vector3(door4.transform.position.x,
                    door4.transform.position.y, door4.transform.position.z + 2);

            }
            if (chamber == 5 && operate == 1)
            {
                light5.SetActive(true);
            }
            else if (chamber == 5 && operate == 2)
            {
                light5.SetActive(false);
            }
            else if (chamber == 5 && operate == 3)
            {
                Debug.Log(dogs);
                if (dogs == 5)
                {
                    ap.Play();
                }
                else
                {
                    smoke5.Play();
                    cock5.Stop();
                }
            }
            else if (chamber == 5 && operate == 4)
            {
                door5.transform.position = new Vector3(door5.transform.position.x,
                    door5.transform.position.y, door5.transform.position.z + 2);

            }
            if (chamber == 6 && operate == 1)
            {
                light6.SetActive(true);
            }
            else if (chamber == 6 && operate == 2)
            {
                light6.SetActive(false);
            }
            else if (chamber == 6 && operate == 3)
            {
                Debug.Log(dogs);
                if (dogs == 6)
                {
                    ap.Play();
                }
                else
                {
                    smoke6.Play();
                    cock6.Stop();
                }
            }
            else if (chamber == 6 && operate == 4)
            {
                door6.transform.position = new Vector3(door6.transform.position.x,
                    door6.transform.position.y, door6.transform.position.z + 2);

            }
            if (chamber == 7 && operate == 1)
            {
                light7.SetActive(true);
            }
            else if (chamber == 7 && operate == 2)
            {
                light7.SetActive(false);
            }
            else if (chamber == 7 && operate == 3)
            {
                Debug.Log(dogs);
                if (dogs == 7)
                {
                    ap.Play();
                }
                else
                {
                    smoke7.Play();
                    cock7.Stop();
                }
            }
            else if (chamber == 7 && operate == 4)
            {
                door7.transform.position = new Vector3(door7.transform.position.x,
                    door7.transform.position.y, door7.transform.position.z + 2);

            }
            if (chamber == 8 && operate == 1)
            {
                light8.SetActive(true);
            }
            else if (chamber == 8 && operate == 2)
            {
                light8.SetActive(false);
            }
            else if (chamber == 8 && operate == 3)
            {
                Debug.Log(dogs);
                if (dogs == 8)
                {
                    ap.Play();
                }
                else
                {
                    smoke8.Play();
                    cock8.Stop();
                }
            }
            else if (chamber == 8 && operate == 4)
            {
                door8.transform.position = new Vector3(door8.transform.position.x,
                    door8.transform.position.y, door1.transform.position.z + 2);

            }
            if (chamber == 9 && operate == 1)
            {
                lightmain.SetActive(true);
            }
            else if (chamber == 9 && operate == 2)
            {
                lightmain.SetActive(false);
            }
            else if (chamber == 9 && operate == 3)
            {

                ap.Play();
            }
            DescriptionText.text = chamber.ToString() + "," + operate;

            }
            DescriptionText.text = "";
            StartButton.gameObject.SetActive(true);
        }));
    }


#endif
}
//一病房开灯，一区关灯，一区消毒，一区通风






