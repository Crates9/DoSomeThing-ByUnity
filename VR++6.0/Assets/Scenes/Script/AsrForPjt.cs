using UnityEngine;
using UnityEngine.UI;
using Wit.BaiduAip.Speech;
using UnityEngine.Video;

public class AsrForPjt : MonoBehaviour
{
    public string APIKey = "";
    public string SecretKey = "";
    public Button StartButton;
    public Button StopButton;
    public Text DescriptionText;
    public AudioSource ap;
    private AudioClip _clipRecord;
    private Asr _asr ;
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
    public Text patText;
    public GameObject patient;

    //  Microphone is not supported in Webgl
#if !UNITY_WEBGL
    static int ToDigit(char cn)

    {

        int number;

        switch (cn)

        {

            case '壹':
            case '一':
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
            case '其':
            case '七':
            case '起':

                number = 7;

                break;

            case '捌':
            case '八':
            case '发':

                number = 8;

                break;

            case '玖':
            case '就':
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
    public int dog;
    void Start()
    {
        _asr = new Asr(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());

        StartButton.gameObject.SetActive(true);
        StopButton.gameObject.SetActive(false);
        DescriptionText.text = "";

        StartButton.onClick.AddListener(OnClickStartButton);
        StopButton.onClick.AddListener(OnClickStopButton);

        light1.SetActive(false); light2.SetActive(false); light3.SetActive(false); light4.SetActive(false);
        light5.SetActive(false); light6.SetActive(false); light7.SetActive(false); light8.SetActive(false);
        lightmain.SetActive(false);
        // dog = ;//生成敌人
         dog = Random.Range(1, 8);//生成敌人
        patient.transform.localScale = new Vector3(50, 50, 60);
       // dog = 1;
        if      (dog == 1) Instantiate(patient, new Vector3(10.902f, 0.5f, -3.24f), Quaternion.Euler(-180,-90,0));
        else if (dog == 2) Instantiate(patient, new Vector3(10.902f, 0.5f, 9.25f), Quaternion.Euler(-180,-90,0));
        else if (dog == 3) Instantiate(patient, new Vector3(10.902f, 0.5f, 21.52f), Quaternion.Euler(-180,-90,0));
        else if (dog == 4) Instantiate(patient, new Vector3(10.902f, 0.5f, 34.17f), Quaternion.Euler(-180,-90,0));
        else if (dog == 5) Instantiate(patient, new Vector3(-5.31f, 0.5f, 36), Quaternion.Euler(-180,-90,0));
        else if (dog == 6) Instantiate(patient, new Vector3(-5.31f,0.5f, 23.47f), Quaternion.Euler(-180,-90,0));
        else if (dog == 7) Instantiate(patient, new Vector3(-5.31f,0.5f, 14.14f), Quaternion.Euler(-180,-90,0));
        else if (dog == 8) Instantiate(patient, new Vector3(-5.31f,0.5f, 1.92f), Quaternion.Euler(-180,-90,0));
        Debug.Log(dog);                                        
        patText.text = "Your patient is in ward No." + dog;
    }


    private void OnClickStartButton()
    {
        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(true);
        DescriptionText.text = "Listening...";

        _clipRecord = Microphone.Start(null, false, 30, 16000);
    }
    private string message;
    private string message2;

    private void OnClickStopButton()
    {

        StartButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(false);
        DescriptionText.text = "Recognizing...";
        Microphone.End(null);

        var data = Asr.ConvertAudioClipToPCM16(_clipRecord);
        StartCoroutine(_asr.Recognize(data, s =>
        {
            message = s.result != null && s.result.Length > 0 ? s.result[0] : "未识别到声音";
            char[] messageChar = message.ToCharArray();
            int temporary = 1;
            if ((message.IndexOf("病房") == -1))Debug.Log("repeat"); 
            else { 
            int chamber = ToDigit(messageChar[(message.IndexOf("病房") - temporary)]);
            for (temporary = 1; temporary < message.IndexOf("病房");)
            { //如病房前面还有字 第六个病房 etc. 接着往前找
                    if (chamber == 0)
                    {
                        chamber = ToDigit(message[(message.IndexOf("病房") - (++temporary))]);
                    }
                    else if (chamber != 0)
                    {//找到了
                        break;
                    }
                    else
                    {
                        Debug.Log("repeat");
                        DescriptionText.text = "repeat";
                        chamber = 0;
                        temporary = 1;
                        break;

                    }
            }
            message2 = s.result != null && s.result.Length > 0 ? s.result[0] : "未识别到声音";
            //0none，1开灯2关灯3消毒4通风
            int operate = 0;
            string[] operates = { "", "开灯", "关灯", "消毒", "喷淋" };
            for (int i = 1; i <= 4;)
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
            else if(chamber == 1 && operate == 2)
            {
                light1.SetActive(false);
            }
            else if (chamber == 1 && operate == 3)
            {
                Debug.Log(dog);
                if (dog==1) 
                {
                    ap.Play();
                }
                else 
                {
                smoke1.Play();
                    cock1.Stop();
                }
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
                Debug.Log(dog);
                if (dog == 2)
                {
                    ap.Play();
                }
                else
                {
                    smoke2.Play();
                    cock2.Stop();
                }
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
                Debug.Log(dog);
                if (dog == 3)
                {
                    
                    ap.Play();
                }
                else
                {
                    smoke3.Play();
                    cock3.Stop();
                }
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
                Debug.Log(dog);
                if (dog == 4)
                {
                    ap.Play();
                }
                else
                {
                    smoke4.Play();
                    cock4.Stop();
                }
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
                Debug.Log(dog);
                if (dog == 5)
                {
                    ap.Play();
                }
                else
                {
                    smoke5.Play();
                    cock5.Stop();
                }
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
                Debug.Log(dog);
                if (dog == 6)
                {
                    ap.Play();
                }
                else
                {
                    smoke6.Play();
                    cock6.Stop();
                }
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
                Debug.Log(dog);
                if (dog == 7)
                {
                    ap.Play();
                }
                else
                {
                    smoke7.Play();
                    cock7.Stop();
                }
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
                Debug.Log(dog);
                if (dog == 8)
                {
                    ap.Play();
                }
                else
                {
                    smoke8.Play();
                    cock8.Stop();
                }
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






