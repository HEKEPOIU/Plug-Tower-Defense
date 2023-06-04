using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

//And also this UI system need to be put on Ui namespace.
//better for manage.
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //UIManager Class is too big now, I need split it to some small class.
    //but i lazy to do it, so i just write some note remind future me.
    [SerializeField] Toggle musicToggle;
    [SerializeField] Button settingButton;
    [SerializeField] Button closeButton;
    [SerializeField] Image plugPanel;
    [SerializeField] Image lostPanel;
    [SerializeField] Text monetBarText;
    [SerializeField] Slider monetBarSlider;
    [SerializeField] Slider hpBarSlider;
    [SerializeField] Sprite musicOnSprite;
    [SerializeField] Sprite musicOffSprite;
    [SerializeField] Button addPlugButton;
    [SerializeField] InputField ipInputField;
    [SerializeField] TowerUi towerUi;
    bool isplugPanelVisible = false;
    bool islostPanelVisible = false;
    

    [SerializeField] GameObject plugList;
    [SerializeField] GameObject plugPrefab;
    [SerializeField] Sprite plugOnSprite;
    [SerializeField] Sprite plugOffSprite;
    Image musicImage;
    readonly List<GameObject> plugs = new List<GameObject>();
    public List<Toggle> plugsToggleList = new List<Toggle>();
    public List<InputField> plugsInputList = new List<InputField>();
    List<Image> plugsImageList = new List<Image>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    async void Start()
    {
        musicImage = musicToggle.GetComponent<Image>();
        musicToggle.onValueChanged.AddListener(ToggleMusic);
        settingButton.onClick.AddListener(TogglePlugPannal);
        closeButton.onClick.AddListener(TogglePlugPannal);
        PlayerInputManager.Instance.OnGrab += Grab;
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
                plugsImageList[p].sprite = change ? plugOnSprite : plugOffSprite;
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

    void Grab(Vector2 dir)
    {
        towerUi.OpenToggle(dir.y > 0);
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
                plugsImageList[plugIndex].sprite = change ? plugOnSprite : plugOffSprite;
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
                plugsImageList.RemoveAt(e.OldStartingIndex);

            }
            catch (Exception exception)
            {
                print(exception);
            }
        }
    }
    
    public void UpdatePlugState(int index, string inform)
    {
        string[] informs = inform.Split();
        bool isOn = Convert.ToBoolean(informs[1]);
        plugsToggleList[index].isOn = isOn;
        plugsImageList[index].sprite = isOn ? plugOnSprite : plugOffSprite;
        plugsInputList[index].text = informs[3];
        
    }
    
    void ToggleMusic(bool isMute)
    {
        MusicManager.Instance.MuteMusic(!isMute);
        musicImage.sprite = !isMute ? musicOnSprite : musicOffSprite;
    }
    void TogglePlugPannal()
    {
        if (isplugPanelVisible)
        {
            plugPanel.transform.DOScale(Vector3.zero, .3F);
            isplugPanelVisible = false;
        }
        else
        {
            plugPanel.transform.DOScale(Vector3.one, .3F);
            isplugPanelVisible = true;
        }
    }

    public void ToggleLostPanel()
    {
        if (islostPanelVisible)
        {
            lostPanel.transform.DOScale(Vector3.zero, .3F);
            isplugPanelVisible = false;
        }
        else
        {
            lostPanel.transform.DOScale(Vector3.one, .3F);
            isplugPanelVisible = true;
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
        Image plugImage = plugButton.GetComponent<Image>();
        plugs.Add(plugButton);
        plugsToggleList.Add(toggle);
        plugsInputList.Add(plugsInput);
        plugsImageList.Add(plugImage);
        try
        {
            string[] plugInform = (await WebManager.Instance.GetPluginSocket(index, "BaseInformation")).Split(" ");
            plugsInput.text = plugInform[3];
            toggle.isOn = Convert.ToBoolean(plugInform[1]);
            plugImage.sprite = Convert.ToBoolean(plugInform[1]) ? plugOnSprite : plugOffSprite;
            
        }
        catch (Exception e)
        {
            print("找不到對應IP插座。");
            WebManager.Instance.TapoIP.RemoveAt(index);
            return;
        }

    }

    public void MoneyChange(int value)
    {
        monetBarText.text = "$ "+ value;
        monetBarSlider.value = value;
    }

    public void HpChange(int value)
    {
        hpBarSlider.value = value;
    }
    
}
