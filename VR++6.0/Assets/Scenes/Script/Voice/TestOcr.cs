using UnityEngine;

using Baidu.Aip.Ocr;
using Wit.BaiduAip.Speech;

using System.IO;

using System;

using UnityEngine.UI;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;


public class TestOcr : MonoBehaviour
{
    public string APIKey = "";
    public string SecretKey = "";
    public Button SynthesisButton;
    public InputField Input;
    public Text DescriptionText;

    string stri;
    private Tts _asr;
    private AudioSource _audioSource;
    private bool _startPlaying;
    // 设置APPID/AK/SK
    public Text text;
    const string APP_ID = "16892473";

    const string API_KEY = "nUxnXglvKdjHDjSGtZCVrR6x";

    const string SECRET_KEY = "4pWdivsnUTU6xKR4Q8sBEdy2dzpi3U6C";

    Ocr client;

    void Awake()
    {

        client = new Ocr(API_KEY, SECRET_KEY);

        client.Timeout = 60000;  // 修改超时时间

    }

    // Use this for initialization

    void Start()
    {

        //调用文字识别函数

        GeneralBasicDemo();
        _asr = new Tts(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());

        _audioSource = gameObject.AddComponent<AudioSource>();

        DescriptionText.text = "";

        SynthesisButton.onClick.AddListener(OnClickSynthesisButton);
    }

   

    public void GeneralBasicDemo()
    {

        //读取对应"图片文件路径"的图片文件

        byte[] image = File.ReadAllBytes(Application.dataPath +  "/StreamingAssets.jpg");

        // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获

        try
        {

            //调取API是图片文字

            //  var result = client.GeneralBasic(image);
            var result = client.GeneralBasic(image);

            //打印获取到的结果

            var regex = new Regex(
                      "\"words\": \"(?<word>[\\s\\S]*?)\"",
                      RegexOptions.CultureInvariant
                      | RegexOptions.Compiled
                      );
            var str = new StringBuilder();
            foreach (Match match in regex.Matches(result.ToString()))
            {
                str.AppendLine(match.Groups["word"].Value.Trim());
            }
            stri = str.ToString();
            text.text = stri;
            //Debug.Log(result);
            //Debug.Log(result["words_result"][0]["words"].ToString());

            //text.text = result["words_result"][0]["words"].ToString();
        }
        catch (Exception e)
        {

            //打印异常信息

            Debug.Log("异常：" + e);

        }

    }
 private void OnClickSynthesisButton()
    {
        SynthesisButton.gameObject.SetActive(false);
        DescriptionText.text = "合成中...";
        
        StartCoroutine(_asr.Synthesis(Input.text=stri, s =>
        {
            if (s.Success)
            {
                DescriptionText.text = "正在播放";
                _audioSource.clip = s.clip;
                _audioSource.Play();

                _startPlaying = true;
            }
            else
            {
                DescriptionText.text = s.err_msg;
                SynthesisButton.gameObject.SetActive(true);
            }
        }));
    }

    void Update()
    {
        if (_startPlaying)
        {
            if (!_audioSource.isPlaying)
            {
                _startPlaying = false;
                DescriptionText.text = "点击修改";
                SynthesisButton.gameObject.SetActive(true);
            }
        }
    }
}