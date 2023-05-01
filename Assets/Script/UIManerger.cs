using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIManerger : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip plugIconAudio;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle downListToggle;
    [SerializeField] Button settingButton;
    [SerializeField] Button closeButton;
    [SerializeField] Image plugPannal;
    bool isplugPannalVisible = false;

    [SerializeField] GameObject plugList;
    [SerializeField] GameObject plugPrefab;
    List<GameObject> plugs = new List<GameObject>();
    // Start is called before the first frame update
    async void Start()
    {
        musicToggle.onValueChanged.AddListener(ToggleMusic);
        downListToggle.onValueChanged.AddListener(ToggleDownlist);
        settingButton.onClick.AddListener(TogglePlugPannal);
        closeButton.onClick.AddListener(TogglePlugPannal);

        await initPlug();
        for (int i = 0; i < plugs.Count; i++)
        {
            int p = i; //如果直接寫i會出現i直接變成最大值，好像是因為觸發的時候，i這東西已經跑完了，所以他會死掉，所以用一個p來保持原本狀態。
            plugs[i].GetComponent<Toggle>().onValueChanged.AddListener(async (bool change) =>
            {
                audioSource.PlayOneShot(plugIconAudio);
                await WebManerger.Instance.SwitchPlugin(p, change);
            });
        }
        //當TapoIP數量改變時時，改變UI。
        WebManerger.Instance.TapoIP.CollectionChanged += TapoIP_CollectionChanged;
    }

    //這樣之後我只要改變TapoIP長度就可以一起改ui跟列表，但目前待測試。
    //e可以用來查詢改變的動作是甚麼或刪除的參數是啥之類的，sender。
    async void TapoIP_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            await ADDPlug(plugs.Count - 1);
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            Destroy(plugs[e.OldStartingIndex]);
            plugs.RemoveAt(e.OldStartingIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ToggleMusic(bool isMute)
    {
        if (isMute) audioSource.mute = false;
        else audioSource.mute = true;
    }
    void ToggleDownlist(bool isdownListVisible)
    {
        if (isdownListVisible) downListToggle.transform.DOMoveY(-190,.5f);
        else downListToggle.transform.DOMoveY(0, .5f);
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
    async Task initPlug()
    {
        for (int i = 0; i < WebManerger.Instance.TapoIP.Count; i++)
        {
            await ADDPlug(i);
        }

    }

    async Task ADDPlug(int index)
    {
        GameObject plugButton = Instantiate(plugPrefab);
        plugButton.transform.SetParent(plugList.transform, false);
        plugButton.GetComponentInChildren<Text>().text = "" + (index + 1);
        plugs.Add(plugButton);
        plugButton.GetComponent<Toggle>().isOn = Convert.ToBoolean((await WebManerger.Instance.GetPluginfor(index, "BaseInformation")).Split(" ")[1]);
    }
}
