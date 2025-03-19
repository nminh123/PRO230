using UnityEngine;

namespace Leo
{
    public class Constants : MonoBehaviour
    {
        private static Constants instance;

        void Awake()
        {
            if (instance != null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        [Header("Player Settings")]
        public      float       gPLAYER_SPEED                  ;
        
        [Header("SetUp Color Opacity")]
        public      float       gCOLOR_BA           =       .5f;
        public      float       gCOLOR_AA           =       1f;

        [Header("Block Indicator")]
        public      float       gToolRange                      ;

        [Header("Scene Manager")]
        public      bool        gIsSwitched        =       false;
        public      float         gTimeToSwitch      =        .5f;
    }
}