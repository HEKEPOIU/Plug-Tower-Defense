
using System.Net.Sockets;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

//this Class Control all of Plug web request.
public class WebManager : MonoBehaviour
{
    public static WebManager Instance { get; private set; }

    [SerializeField] string serverIP; //wait for change to socket.

    ObservableCollection<string> tapoIP = new ObservableCollection<string>();
    public string Email { get; set; }
    public string Password { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public ObservableCollection<string> TapoIP { get =>tapoIP; set=> tapoIP = value; }
    public string ServerIP { get; set; }

    #region Socket Version

    public async Task<string> GetPluginSocket(int which, string whatTodo)
    {
        //創建客戶端，並且等待連線。
        Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
        await tcpClient.ConnectAsync(serverIP,8888);
        
        //成功連線後準備需要送出的訊息，並送出。
        string message = Email + " " + Password + " " + tapoIP[which] + " " + whatTodo;
        byte[] data = Encoding.UTF8.GetBytes(message);
        await tcpClient.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
        
        //準備用來接收訊息的buffer，因為他是會傳byte過來，然後等待回復bytes長度，並解碼訊息。
        byte[] receiveBuffer = new byte[1024];
        int bytesRead = await tcpClient.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), SocketFlags.None);
        string receivedMessage  = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);

        return receivedMessage;
    }

    public async Task<string> SwitchPluginSocket(int which, bool isOn)
    {
        //創建客戶端，並且等待連線。
        Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
        await tcpClient.ConnectAsync(serverIP,8888);
        
        //成功連線後準備需要送出的訊息，並送出。
        string whatTodo = isOn ? "On" : "Off";
        print(whatTodo);
        string message = Email + " " + Password + " " + tapoIP[which] + " " + whatTodo;
        byte[] data = Encoding.UTF8.GetBytes(message);
        await tcpClient.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
        
        //準備用來接收訊息的buffer，因為他是會傳byte過來，然後等待回復bytes長度，並解碼訊息。
        byte[] receiveBuffer = new byte[1024];
        int bytesRead = await tcpClient.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), SocketFlags.None);
        string receivedMessage  = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);

        return receivedMessage;
    }
    
    

    #endregion
    
    

    
    
    #region Http Version
    public async Task<string> GetPluginHttp(int which,string whatTodo)
    {
        UnityWebRequest request = UnityWebRequest.Get(serverIP + "/" + Email + "/" + Password + "/" + tapoIP[which] +"/"+whatTodo);

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


    public async Task<string> SwitchPluginHttp(int which, bool isOn)
    {
        string whatTodo = isOn ? "On" : "Off";

        UnityWebRequest request = UnityWebRequest.Get(serverIP + "/" + Email + "/" + Password + "/" + tapoIP[which] + "/" + whatTodo);

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

    #endregion
}
