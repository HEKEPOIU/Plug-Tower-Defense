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

    //取代協成的c#預設寫法,由於他可以回傳東西，所以我用這個。
    //task後面要寫要傳回的東西。
    public async Task<string> GetPluginfor(int which,string _whatTodo)
    {
        UnityWebRequest request = UnityWebRequest.Get(serverIP + "/" + Email + "/" + Password + "/" + tapoIP[which] +"/"+_whatTodo);

        request.SetRequestHeader("ngrok-skip-browser-warning", "1");


        request.SendWebRequest();
        
        while (!request.isDone)
        {
            Debug.Log("執行中");
            await Task.Yield();//這裡純等待執行完畢。

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
            Debug.Log("執行中");
            await Task.Yield();//這裡純等待執行完畢。

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
