using DG.Tweening;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] Button sumitButton;
    [SerializeField] Button nextButton;
    [SerializeField] InputField[] inputField = new InputField[2];
    [SerializeField] Text message;
    [SerializeField] GameObject loginPannal;
    [SerializeField] Color errorColor;
    [SerializeField] Color normalColor;
    [SerializeField] GameObject[] panel; 


    void Awake()
    {
        //if login before, skip the login page.
        if (PlayerPrefs.GetInt("LoginState", -1) == -1) return;
        WebManager.Instance.Email = PlayerPrefs.GetString("Email");
        WebManager.Instance.Password = PlayerPrefs.GetString("Password");
        WebManager.Instance.TapoIP.Add(PlayerPrefs.GetString("PlugIp01"));
        WebManager.Instance.ServerIP = PlayerPrefs.GetString("ServerIp");
        SceneManager.LoadScene(1);
    }

    void Start()
    {
        sumitButton.onClick.AddListener(Login);
        nextButton.onClick.AddListener(Next);

        //why unity use passWordInput need use script??
        //why i can't just set in the ui??
        inputField[1].contentType = InputField.ContentType.Password;

        loginPannal.transform.DOLocalMoveY(-40, 1f).SetEase(Ease.InOutSine);
    }
    
        


    bool IsvalidEmail(string email)
    {
        string patten = @"^\w+@\w+.[a-z]{3}$";
        if (Regex.IsMatch(email,patten)) return true;
        else return false;
        
    }
    void Next()
    {
        message.color = Color.clear;
        if ( !IsvalidEmail(inputField[0].text) )
        {
            message.color = errorColor;
            message.text = "帳號必須為Email";
            return;
        }
        //test the account isn't right.
        WebManager.Instance.Email = inputField[0].text;
        WebManager.Instance.Password = inputField[1].text;
        panel[0].SetActive(false);
        panel[1].SetActive(true);
    }
    async void Login()
    {
        WebManager.Instance.TapoIP.Add(inputField[2].text);
        WebManager.Instance.ServerIP = inputField[3].text;
        sumitButton.GetComponentInChildren<Text>().text = "Loading...";
        string testMessage = await WebManager.Instance.GetPluginSocket(0,"BaseInformation");

        if (testMessage == "Error")
        {
            WebManager.Instance.Email = "";
            WebManager.Instance.Password = "";
            WebManager.Instance.ServerIP = "";
            WebManager.Instance.TapoIP.Clear();
            panel[0].SetActive(true);
            panel[1].SetActive(false);
            sumitButton.GetComponentInChildren<Text>().text = "確　認";
            message.color = errorColor;
            message.text = "帳號/密碼或插座IP錯誤";
            return;
        }
        sumitButton.GetComponentInChildren<Text>().text = "成 功！";
        PlayerPrefs.SetInt("LoginState", 1);
        PlayerPrefs.SetString("Email", inputField[0].text);
        PlayerPrefs.SetString("Password", inputField[1].text);
        PlayerPrefs.SetString("PlugIp01", inputField[2].text);
        PlayerPrefs.SetString("ServerIp", inputField[3].text);

        await Task.Delay(500);

        Sequence aniSequence = DOTween.Sequence();
        aniSequence.Append(loginPannal.transform.DOLocalMoveY(-150, .3f).SetEase(Ease.InOutSine));
        aniSequence.Append(loginPannal.transform.DOLocalMoveY(1350, .2f).SetEase(Ease.InOutSine));

        aniSequence.OnComplete(() =>
        {
            SceneManager.LoadScene(1);
        });
    }

}
