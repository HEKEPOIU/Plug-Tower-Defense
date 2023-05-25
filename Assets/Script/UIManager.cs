using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] Toggle musicToggle;
    [SerializeField] Button settingButton;
    [SerializeField] Button closeButton;
    [SerializeField] Image plugPannal;
    [SerializeField] Text monetBarText;
    [SerializeField] Sprite musicOnSprite;
    [SerializeField] Sprite musicOffSprite;
    [SerializeField] Button addPlugButton;
    [SerializeField] InputField ipInputField;
    bool isplugPannalVisible = false;

    [SerializeField] GameObject plugList;
    [SerializeField] GameObject plugPrefab;
    [SerializeField] Sprite plugOnSprite;
    [SerializeField] Sprite plugOffSprite;
    Image musicImage;
    readonly List<GameObject> plugs = new List<GameObject>();
    public List<Toggle> plugsToggleList;
    public List<InputField> plugsInputList;
    // Start is called before the first frame update
    async void Start()
    {
        musicImage = musicToggle.GetComponent<Image>();
        musicToggle.onValueChanged.AddListener(ToggleMusic);
        settingButton.onClick.AddListener(TogglePlugPannal);
        closeButton.onClick.AddListener(TogglePlugPannal);
        addPlugButton.onClick.AddListener(() =>
        {
            WebManager.Instance.TapoIP.Add(ipInputField.text);
            ipInputField.text = "";
        });

        //init Plug

        for (int i = 0; i < WebManager.Instance.TapoIP.Count; i++)
        {
            await AddPlugIcon(i);
            int p = i; //如果直接寫i會出現i直接變成最大值，好像是因為觸發的時候，i這東西已經跑完了，所以他會死掉，所以用一個p來保持原本狀態。
            plugs[i].GetComponent<Toggle>().onValueChanged.AddListener(async (bool change) =>
            {
                MusicManager.Instance.PlayPlugAudio();
                plugs[p].GetComponent<Image>().sprite = change ? plugOnSprite : plugOffSprite;
                await WebManager.Instance.SwitchPluginSocket(p, change);
            });
            plugsInputList[i].GetComponent<InputField>().onEndEdit.AddListener(async (string text) =>
            {
                await WebManager.Instance.ChangePluginNameSocket(p, text);
            });
        }
        //當TapoIP數量改變時時，改變UI。
        WebManager.Instance.TapoIP.CollectionChanged += TapoIP_CollectionChanged;
    }

    //這樣之後我只要改變TapoIP長度就可以一起改ui跟列表，但目前待測試。
    //e可以用來查詢改變的動作是甚麼或刪除的參數是啥之類的，sender。
    async void TapoIP_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            
            await AddPlugIcon(plugs.Count);
            int plugIndex = plugs.Count - 1;
            plugsInputList[plugIndex].onEndEdit.AddListener(async (string text) =>
            {
                await WebManager.Instance.ChangePluginNameSocket(plugIndex, text);
            });
            plugsToggleList[plugIndex].onValueChanged.AddListener(async (bool change) =>
            {
                MusicManager.Instance.PlayPlugAudio();
                plugs[plugIndex].GetComponent<Image>().sprite = change ? plugOnSprite : plugOffSprite;
                await WebManager.Instance.SwitchPluginSocket(plugIndex, change);
            });
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            try
            {
                Destroy(plugs[e.OldStartingIndex]);
                plugs.RemoveAt(e.OldStartingIndex);
                plugsToggleList.RemoveAt(e.OldStartingIndex);
                plugsInputList.RemoveAt(e.OldStartingIndex);

            }
            catch (Exception exception)
            {
                print(exception);
            }
        }
    }
    void ToggleMusic(bool isMute)
    {
        MusicManager.Instance.MuteMusic(!isMute);
        musicImage.sprite = !isMute ? musicOnSprite : musicOffSprite;
    }
    void TogglePlugPannal()
    {
        if (isplugPannalVisible)
        {
            plugPannal.transform.DOScale(Vector3.zero, .3F);
            isplugPannalVisible = false;
        }
        else
        {
            plugPannal.transform.DOScale(Vector3.one, .3F);
            isplugPannalVisible = true;
        }
    }

    public void SwitchPlugName()
    {
        foreach (InputField input in plugsInputList)
        {
            input.interactable = !input.interactable;
        }
    }
    
    async Task AddPlugIcon(int index)
    {
        GameObject plugButton = Instantiate(plugPrefab, plugList.transform, false);
        plugButton.GetComponentInChildren<Text>().text = "" + (index + 1);
        InputField plugsInput = plugButton.GetComponentInChildren<InputField>();
        Toggle toggle = plugButton.GetComponent<Toggle>();
        plugs.Add(plugButton);
        plugsToggleList.Add(toggle);
        plugsInputList.Add(plugsInput);
        try
        {
            string[] plugInform = (await WebManager.Instance.GetPluginSocket(index, "BaseInformation")).Split(" ");
            plugsInput.text = plugInform[3];
            toggle.isOn = Convert.ToBoolean(plugInform[1]);
            
        }
        catch (Exception e)
        {
            print("找不到對應IP插座。");
            WebManager.Instance.TapoIP.RemoveAt(index);
            return;
        }

    }

    public void MoneyChange(string value)
    {
        monetBarText.text = "$ "+ value;
    }
}
