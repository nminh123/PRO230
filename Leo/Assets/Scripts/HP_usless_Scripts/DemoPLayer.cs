using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public float speed = 5f;
    public int maxHp = 100;
    public int currentHp;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHp = maxHp;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rb.velocity = movement.normalized * speed;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log("Player HP: " + currentHp + "/" + maxHp);

        if (currentHp <= 0)
        {
            Debug.Log("Player Died!");
            Destroy(gameObject);
        }
    }
}
