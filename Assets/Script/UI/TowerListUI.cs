using UnityEngine;

namespace UI
{
    public class TowerListUI : MonoBehaviour
    {
        TowerButton[] towerButtons;
        
        void Start()
        {
            towerButtons = GetComponentsInChildren<TowerButton>();
        }
        
        public void ToggleAttributeImage(int index, bool toggle)
        {
            towerButtons[index].ToggleAttributeImage(toggle);
        }
    }
}
