using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Color errorColor;
    [SerializeField] Color normalColor;

    bool loginState;
    // Start is called before the first frame update
    void Start()
    {
        sumitButten.onClick.AddListener(Login);

        //why unity use passwordinput need use script??
        //why i can't just set in the ui??
        inputField[1].contentType = InputField.ContentType.Password;
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

        await Task.Delay(1000);
        SceneManager.LoadSceneAsync(1);
    }

}
