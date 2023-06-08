using System;
using System.Collections.Generic;
using System.Linq;
using Tower;
using UI;
using UnityEngine;

namespace Manager
{
    public class TowerManager : MonoBehaviour
    {
        public List<Tower.Tower> Towers { get;} = new List<Tower.Tower>();
        [SerializeField] TowerListUI towerListUI;
        
        /// <summary>
        /// 0:Fire,
        /// 1:Wind,
        /// 2:Water,
        /// 3:Electromagnetic,
        /// 4:None
        /// </summary>
        TowerAttribute[] isAttributeTriggle = new TowerAttribute[5];
        string[,] attributeNames = new string[,] { { "火","烤箱"} , { "吹風機","電風扇" }, { "熱水器","水壺" }, { "電","微波爐" }};
        public static TowerManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            for (int i = 0; i < isAttributeTriggle.Length; i++)
            {
                isAttributeTriggle[i] = new TowerAttribute{Index = i};
                isAttributeTriggle[i].OnValueChanged += OnAttributeChange;

            }
        }

        void OnAttributeChange(int index,bool value)
        {
            foreach (Tower.Tower tower in Towers.Where(tower => tower.towerAttributes == (TowerAttributes) index))
            {
                tower.EffectToggle(value);
            }
            towerListUI.ToggleAttributeImage(index, value);
        }

        public void AttributeDetect(string newName , bool change)
        {
            
            //it should be put on server, but I lazy to write.
            for (int i = 0; i < attributeNames.GetLength(0); i++)
            {
                for (int j = 0; j < attributeNames.GetLength(1); j++)
                {
                    if (!newName.Contains(attributeNames[i, j])) continue;

                    isAttributeTriggle[i].Value = change;
                    return;
                }
            }
            
            isAttributeTriggle[4].Value = change;
        }
    }
}
