
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
        //?���B������??Û���@?�z
        try
        {
            Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await tcpClient.ConnectAsync(ServerIP, 8888);

            //���ո@?��??�z���ߵ�ǻ?����?�ߵ̩z
            string message = Email + " " + Password + " " + tapoIP[which] + " " + whatTodo;
            byte[] data = Encoding.UTF8.GetBytes(message);
            await tcpClient.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);

            //??�[?Ն��?��ǻbuffer�۶~?��멛?byte��?��?��Û��϶��bytes���H��?�v��?���z
            byte[] receiveBuffer = new byte[1024];
            int bytesRead = await tcpClient.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), SocketFlags.None);
            string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
            return receivedMessage;
        }
        catch
        {
            print("����?���P?");
            return "Error";
        }
    }

    public async Task<string> SwitchPluginSocket(int which, bool isOn)
    {
        //?���B������??Û���@?�z
        Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
        await tcpClient.ConnectAsync(ServerIP,8888);
        
        //���ո@?��??�z���ߵ�ǻ?����?�ߵ̩z
        string whatTodo = isOn ? "On" : "Off";
        string message = Email + " " + Password + " " + tapoIP[which] + " " + whatTodo;
        byte[] data = Encoding.UTF8.GetBytes(message);
        await tcpClient.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
        
        //??�[?Ն��?��ǻbuffer�۶~?��멛?byte��?��?��Û��϶��bytes���H��?�v��?���z
        byte[] receiveBuffer = new byte[1024];
        int bytesRead = await tcpClient.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), SocketFlags.None);
        string receivedMessage  = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);

        return receivedMessage;
    }
    
    

    #endregion
    
    

    
    
    #region Http Version
    public async Task<string> GetPluginHttp(int which,string whatTodo)
    {
        UnityWebRequest request = UnityWebRequest.Get(ServerIP + "/" + Email + "/" + Password + "/" + tapoIP[which] +"/"+whatTodo);

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

        UnityWebRequest request = UnityWebRequest.Get(ServerIP + "/" + Email + "/" + Password + "/" + tapoIP[which] + "/" + whatTodo);

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
