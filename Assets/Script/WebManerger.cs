using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

//this Class Control all of Plug web request.
public class WebManerger : MonoBehaviour
{
    static WebManerger instance;
    [SerializeField] string serverIP; //wait for change to socket.
    [SerializeField] ObservableCollection<string> tapoIP;
    public string Email { get; set; }
    public string Password { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

    }
    public static WebManerger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new WebManerger();
            }
            return instance;
        }
    }
    public ObservableCollection<string> TapoIP { get {  return tapoIP; } set {  tapoIP = value; } }
    public string ServerIP { get { return serverIP; } set { serverIP = value; } }

    //���N�󦨪�c#�w�]�g�k,�ѩ�L�i�H�^�ǪF��A�ҥH�ڥγo�ӡC
    //task�᭱�n�g�n�Ǧ^���F��C
    public async Task<string> GetPluginfor(int which,string _whatTodo)
    {
        UnityWebRequest request = UnityWebRequest.Get(serverIP + "/" + Email + "/" + Password + "/" + tapoIP[which] +"/"+_whatTodo);

        request.SetRequestHeader("ngrok-skip-browser-warning", "1");


        request.SendWebRequest();
        
        while (!request.isDone)
        {
            Debug.Log("���椤");
            await Task.Yield();//�o�̯µ��ݰ��槹���C

        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            return request.downloadHandler.text;
        }
        else
        {
            return "Error";
        }


    }
    public async Task<string> SwitchPlugin(int which, bool isOn)
    {
        string _whatTodo = isOn ? "On" : "Off";

        UnityWebRequest request = UnityWebRequest.Get(serverIP + "/" + Email + "/" + Password + "/" + tapoIP[which] + "/" + _whatTodo);

        request.SetRequestHeader("ngrok-skip-browser-warning", "1");


        request.SendWebRequest();

        while (!request.isDone)
        {
            Debug.Log("���椤");
            await Task.Yield();//�o�̯µ��ݰ��槹���C

        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            return request.downloadHandler.text;
        }
        else
        {
            return "Error";
        }


    }

}
