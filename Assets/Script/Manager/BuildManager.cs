using Tower;
using UnityEngine;

namespace Manager
{
    public class BuildManager : MonoBehaviour
    {
        public static BuildManager Instance { get;private set;}
        public GameObject Tower { get; set; }
    
        TowerPlace[] towerPlaces;
        public int Cost { get; set; }
    
        public bool buildReady = false;

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
            towerPlaces = new TowerPlace[transform.childCount];
            for (int i = 0; i < towerPlaces.Length; i++)
            {
                towerPlaces[i] = transform.GetChild(i).GetComponent<TowerPlace>();
            }
        }

        public void BuildReady()
        {
            foreach (TowerPlace place in towerPlaces)
            {
                place.OnBuildReady();
            }

            buildReady = true;
        }
    
        public void BuildCancel()
        {
            foreach (TowerPlace place in towerPlaces)
            {
                place.OnBuildCancel();
            }

            buildReady = false;
        }
    }
}
