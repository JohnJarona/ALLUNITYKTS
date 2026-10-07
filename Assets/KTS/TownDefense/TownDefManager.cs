using UnityEngine;

namespace TownDefense
{
    public class TownDefManager : MonoBehaviour
    {
        public static TownDefManager Instance;
        public void Start()
        {
            Instance = this;
        }
        public int wheat;
        public int UNITS_krestyane;
        public int UNITS_voins;
        public void Update()
        {
            
        }
    }
}
