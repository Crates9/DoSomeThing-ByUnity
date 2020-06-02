using UnityEngine;
using UnityEngine.UI;
using Wit.BaiduAip.Speech;


public class AsrDemo : MonoBehaviour
{
    public string APIKey = "";
    public string SecretKey = "";
    public Button StartButton;
    public Button StopButton;
    public Text DescriptionText;

    private AudioClip _clipRecord;
    private Asr _asr;

  //  Microphone is not supported in Webgl
#if !UNITY_WEBGL
    static int ToDigit(char cn)

    {

        int number ;

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
            message = s.result!= null && s.result.Length > 0 ? s.result[0] : "未识别到声音";
            char[] messageChar = message.ToCharArray();
            int temporary = 1;
            int chamber = ToDigit(messageChar[(message.IndexOf("病房") - temporary)]);
            for(temporary=1;temporary<message.IndexOf("病房") ;)
            { //如病房前面还有字 第六个病房 etc. 接着往前找
                if (chamber == 0)
                {
                    chamber = ToDigit(message[(message.IndexOf("病房") - (++temporary))]);
                }
                else if (chamber !=0 )
                {//找到了
                    break;
                }
            }
            message2 = s.result != null && s.result.Length > 0 ? s.result[0] : "未识别到声音";
            //0none，1开灯2关灯3消毒4通风
            int operate = 0;
            string[] operates = { "", "开灯", "关灯", "消毒", "通风" };
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
            DescriptionText.text =chamber.ToString()+","+operate;
            StartButton.gameObject.SetActive(true);
        }));
    }
#endif
}
//一病房开灯，一区关灯，一区消毒，一区通风






