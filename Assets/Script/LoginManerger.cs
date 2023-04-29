using DG.Tweening;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManerger : MonoBehaviour
{
    [SerializeField] Button sumitButten;
    [SerializeField] InputField[] inputField = new InputField[2];
    [SerializeField] Text Message;
    [SerializeField] GameObject loginPannal;
    [SerializeField] Color errorColor;
    [SerializeField] Color normalColor;


    private void Awake()
    {
        //if login before, skip the login page.
        if (PlayerPrefs.GetInt("LoginState", -1) != -1 )
        {
            WebManerger.Instance.Email = PlayerPrefs.GetString("Email");
            WebManerger.Instance.Password = PlayerPrefs.GetString("Password");
            SceneManager.LoadScene(1);
        }
    }

    void Start()
    {
        sumitButten.onClick.AddListener(Login);

        //why unity use passwordinput need use script??
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
    async void Login()
    {
        Message.color = Color.clear;
        if ( !IsvalidEmail(inputField[0].text) )
        {
            Message.color = errorColor;
            Message.text = "帳號必須為Email";
            return;
        }
        //test the account isn't right.
        WebManerger.Instance.Email = inputField[0].text;
        WebManerger.Instance.Password = inputField[1].text;
        sumitButten.GetComponentInChildren<Text>().text = "Loading...";
        string testMessage = await WebManerger.Instance.GetPluginfor(0,"BaseInformation");

        if (testMessage == "Error")
        {
            WebManerger.Instance.Email = "";
            WebManerger.Instance.Password = "";
            sumitButten.GetComponentInChildren<Text>().text = "確　認";
            Message.color = errorColor;
            Message.text = "帳號或密碼錯誤";
            return;
        }
        sumitButten.GetComponentInChildren<Text>().text = "成 功！";
        PlayerPrefs.SetInt("LoginState", 1);
        PlayerPrefs.SetString("Email", inputField[0].text);
        PlayerPrefs.SetString("Password", inputField[1].text);

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
