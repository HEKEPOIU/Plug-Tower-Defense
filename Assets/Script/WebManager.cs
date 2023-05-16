using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

//this Class Control all of Plug web request.
public class WebManager : MonoBehaviour
{
    static WebManager _instance;
    [SerializeField] string serverIP; //wait for change to socket.
    public List<string> tapoIPList;
    ObservableCollection<string> _tapoIP;
    public string Email { get; set; }
    public string Password { get; set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        _tapoIP = new ObservableCollection<string>(tapoIPList);
    }
    public static WebManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new WebManager();
            }
            return _instance;
        }
    }
    public ObservableCollection<string> TapoIP { get => _tapoIP; set => _tapoIP = value; }
    public string ServerIP { get => serverIP; set => serverIP = value; }


    public async Task<string> GetPlugin(int which,string whatTodo)
    {
        UnityWebRequest request = UnityWebRequest.Get(serverIP + "/" + Email + "/" + Password + "/" + _tapoIP[which] +"/"+whatTodo);

        request.SetRequestHeader("ngrok-skip-browser-warning", "1");


        request.SendWebRequest();
        
        while (!request.isDone)
        {
            await Task.Yield();

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
        string whatTodo = isOn ? "On" : "Off";

        UnityWebRequest request = UnityWebRequest.Get(serverIP + "/" + Email + "/" + Password + "/" + _tapoIP[which] + "/" + whatTodo);

        request.SetRequestHeader("ngrok-skip-browser-warning", "1");


        request.SendWebRequest();

        while (!request.isDone)
        {
            await Task.Yield();

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
