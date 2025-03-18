using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHp = 50;
    public int currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log(gameObject.name + " HP: " + currentHp + "/" + maxHp);

        if (currentHp <= 0)
        {
            Destroy(gameObject);
            Debug.Log(gameObject.name + " bị phá hủy!");
        }
    }
}
