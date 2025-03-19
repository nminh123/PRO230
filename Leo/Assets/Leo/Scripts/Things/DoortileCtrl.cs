using Global;
using UnityEngine;

public class DoortileCtrl : MonoBehaviour
{
    private Constants consts;
    [SerializeField] private GameObject dooropen;
    [SerializeField] private GameObject doorclose;

    void Start()
    {
        consts = FindFirstObjectByType<Constants>();
    }

    void OnDisable()
    {
        doorclose.SetActive(true);
        dooropen.SetActive(false);
    }

    void Update()
    {
        if (consts.gIsSwitched == true)
        {
            doorclose.SetActive(false);
            dooropen.SetActive(true);
        }
    }
}