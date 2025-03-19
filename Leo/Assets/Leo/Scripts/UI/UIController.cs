using UnityEngine;

namespace Leo.UI
{
    public class UIController : MonoBehaviour
    {
        public static UIController instance;
        [SerializeField] GameObject[] toolbarActivatorIcon;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        void Start()
        {
            foreach (GameObject icon in toolbarActivatorIcon)
            {
                icon.SetActive(false);
            }
        }

        public void SwitchTool(int valSeclected)
        {
            foreach (GameObject icon in toolbarActivatorIcon)
            {
                icon.SetActive(false);
            }

            toolbarActivatorIcon[valSeclected].SetActive(true);
        }
    }
}