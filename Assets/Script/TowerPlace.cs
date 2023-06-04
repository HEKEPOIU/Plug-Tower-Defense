using System;
using UnityEngine;

public class TowerPlace : MonoBehaviour
{
    GameObject tower;
    SpriteRenderer childRenderer;
    Material childMaterial;
    [ColorUsage(true, true)]
    [SerializeField] Color normalColor,heightLihtColor;

    static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    void Start()
    {
        childRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        childMaterial = childRenderer.material;
        childRenderer.color = normalColor;
        childMaterial.SetColor(EmissionColor, normalColor);
    }
    
    public void Build()
    {
        if (tower != null || BuildManager.Instance.Tower == null) return;

        //Spawn Tower to Child
        GameObject towerToBuild = BuildManager.Instance.Tower;
        tower = Instantiate(towerToBuild, transform.position, Quaternion.identity);
        tower.transform.SetParent(transform);
        
        //Deduct Money
        GameManager.Instance.Money -= BuildManager.Instance.Cost;
        UIManager.Instance.MoneyChange(GameManager.Instance.Money);
        
        //Reset
        BuildManager.Instance.Tower = null;
        BuildManager.Instance.Cost = 0;
        
        
    }

    public void OnBuildReady()
    {
        if (tower != null) return;
        childRenderer.color = Color.white;
        childMaterial.SetColor(EmissionColor, heightLihtColor);
    }
    
    public void OnBuildCancel()
    {
        childRenderer.color = normalColor;
        childMaterial.SetColor(EmissionColor, normalColor);
    }

    public void OnCharge(Color chargeColor)
    {
        childRenderer.color = Color.white;
        childMaterial.SetColor(EmissionColor, chargeColor);
    }
}
