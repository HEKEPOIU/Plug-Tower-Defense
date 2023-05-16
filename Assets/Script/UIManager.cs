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
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip plugIconAudio;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle downListToggle;
    [SerializeField] Button settingButton;
    [SerializeField] Button closeButton;
    [SerializeField] Image plugPannal;
    [SerializeField] Text monetBarText;
    bool _isplugPannalVisible = false;

    [SerializeField] GameObject plugList;
    [SerializeField] GameObject plugPrefab;
    List<GameObject> _plugs = new List<GameObject>();
    public List<Toggle> plugsToggleList;
    // Start is called before the first frame update
    async void Start()
    {
        musicToggle.onValueChanged.AddListener(ToggleMusic);
        downListToggle.onValueChanged.AddListener(ToggleDownlist);
        settingButton.onClick.AddListener(TogglePlugPannal);
        closeButton.onClick.AddListener(TogglePlugPannal);

        //init Plug

        for (int i = 0; i < WebManager.Instance.TapoIP.Count; i++)
        {
            await AddPlugIcon(i);
            int p = i; //如果直接寫i會出現i直接變成最大值，好像是因為觸發的時候，i這東西已經跑完了，所以他會死掉，所以用一個p來保持原本狀態。
            _plugs[i].GetComponent<Toggle>().onValueChanged.AddListener(async (bool change) =>
            {
                audioSource.PlayOneShot(plugIconAudio);
                await WebManager.Instance.SwitchPlugin(p, change);
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
            await AddPlugIcon(_plugs.Count - 1);
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            Destroy(_plugs[e.OldStartingIndex]);
            _plugs.RemoveAt(e.OldStartingIndex);
            plugsToggleList.RemoveAt(e.OldStartingIndex);
        }
    }
    void ToggleMusic(bool isMute)
    {
        audioSource.mute = !isMute;
    }
    void ToggleDownlist(bool isDownListVisible)
    {
        if (isDownListVisible) downListToggle.transform.DOMoveY(-190,.5f);
        else downListToggle.transform.DOMoveY(0, .5f);
    }
    void TogglePlugPannal()
    {
        if (_isplugPannalVisible)
        {
            plugPannal.transform.DOScale(Vector3.zero, .3F);
            _isplugPannalVisible = false;
        }
        else
        {
            plugPannal.transform.DOScale(Vector3.one, .3F);
            _isplugPannalVisible = true;
        }
    }
    async Task AddPlugIcon(int index)
    {
        GameObject plugButton = Instantiate(plugPrefab, plugList.transform, false);
        plugButton.GetComponentInChildren<Text>().text = "" + (index + 1);
        try
        {
            Toggle toggle = plugButton.GetComponent<Toggle>();
            toggle.isOn = Convert.ToBoolean((await WebManager.Instance.GetPlugin(index, "BaseInformation")).Split(" ")[1]);
            _plugs.Add(plugButton);
            plugsToggleList.Add(toggle);
        }
        catch
        {
            print("找不到對應IP插座。");
            return;
        }

    }

    public void MoneyChange(string value)
    {
        monetBarText.text = value;
    }
}
