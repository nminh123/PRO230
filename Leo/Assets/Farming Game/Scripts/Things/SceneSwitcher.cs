using FarmingGame.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] int SceneToLoad; //return 0 & 1. 0 is outdoors, 1 is indoors.
    [SerializeField] Transform startPoint;
    private EdgeCollider2D mCollider;

    void Awake()
    {
        PlayerController.instance.transform.position = startPoint.position;
    }

    void Start()
    {
        mCollider = GetComponent<EdgeCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneToLoad);
        }
    }
}