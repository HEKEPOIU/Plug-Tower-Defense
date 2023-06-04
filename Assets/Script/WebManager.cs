
using System.Net.Sockets;
using System.Collections.ObjectModel;
using System.Text;
using System;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Networking;

//this Class Control all of Plug web request.
public class WebManager : MonoBehaviour
{
    public static WebManager Instance { get; private set; }

    public string Email { get; set; }
    public string Password { get; set; }
    
    bool onUpdateState = false;

    [SerializeField] float updateTime;

    float timer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        timer = updateTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer<=0 && onUpdateState == false)
        {
            UpdatePluginListState();
            timer = updateTime;
        }
    }

    public ObservableCollection<string> TapoIP { get; } = new ObservableCollection<string>();
    public string ServerIP { get; set; }

    #region Socket Version

    public async Task<string> GetPluginSocket(int which, string whatTodo)
    {
        
        try
        {
            Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await tcpClient.ConnectAsync(ServerIP, 8888);

            
            string message = Email + " " + Password + " " + TapoIP[which] + " " + whatTodo +" "+ "-1";
            byte[] data = Encoding.UTF8.GetBytes(message);
            await tcpClient.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);

            
            byte[] receiveBuffer = new byte[1024];
            int bytesRead = await tcpClient.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), SocketFlags.None);
            string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
            return receivedMessage;
        }
        catch
        {
            print("伺服器連結失敗。");
            return "Error";
        }
    }

    public async Task<string> SwitchPluginSocket(int which, bool isOn)
    {

        Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
        await tcpClient.ConnectAsync(ServerIP,8888);
        

        string whatTodo = isOn ? "On" : "Off";
        string message = Email + " " + Password + " " + TapoIP[which] + " " + whatTodo +" "+ "-1";
        byte[] data = Encoding.UTF8.GetBytes(message);
        await tcpClient.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
        

        byte[] receiveBuffer = new byte[1024];
        int bytesRead = await tcpClient.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), SocketFlags.None);
        string receivedMessage  = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);

        return receivedMessage;
    }
    public async Task<string> ChangePluginNameSocket(int which, string newName)
    {

        Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
        await tcpClient.ConnectAsync(ServerIP,8888);
        
        
        string message = Email + " " + Password + " " + TapoIP[which] + " " + "ChangeName" + " " + newName;
        byte[] data = Encoding.UTF8.GetBytes(message);
        await tcpClient.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
        

        byte[] receiveBuffer = new byte[1024];
        int bytesRead = await tcpClient.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), SocketFlags.None);
        string receivedMessage  = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);

        return receivedMessage;
    }

    async void UpdatePluginListState()
    {
        for (int i = 0; i < TapoIP.Count; i++)
        {
            try
            {
                onUpdateState = true;
                string plugInform = await GetPluginSocket(i, "BaseInformation");
                UIManager.Instance.UpdatePlugState(i, plugInform);

            }
            catch
            {
                print("Plug " + TapoIP[i] + " is offline.");
                TapoIP.RemoveAt(i);
                i--;
            }
            finally
            {
                onUpdateState = false;
            }
        }
    }
    

    #endregion
    
    

    
    
    #region Http Version
    public async Task<string> GetPluginHttp(int which,string whatTodo)
    {
        UnityWebRequest request = UnityWebRequest.Get(ServerIP + "/" + Email + "/" + Password + "/" + TapoIP[which] +"/"+whatTodo);

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

        UnityWebRequest request = UnityWebRequest.Get(ServerIP + "/" + Email + "/" + Password + "/" + TapoIP[which] + "/" + whatTodo);

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
