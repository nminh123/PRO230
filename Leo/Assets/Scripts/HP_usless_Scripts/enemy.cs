using UnityEngine;
using System.Collections;
using System;

public class EnemyAI : MonoBehaviour
{
    public int hp = 100;
    public int attack = 10;
    public float speed = 3f;
    public float attackCooldown = 0.5f;
    private bool canAttack = true;

    private Rigidbody2D rb;
    private Transform target;

    [Header("Reward Settings")]
    public RewardItem[] rewards; // Danh sách phần thưởng có thể spawn

    public event Action OnDeath;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void FixedUpdate()
    {
        if (hp <= 0) Destroy(gameObject);
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!canAttack) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController1 player = collision.gameObject.GetComponent<PlayerController1>();
            if (player != null)
            {
                StartCoroutine(AttackPlayer(player));
            }
        }
        else
        {
            Health objectHealth = collision.gameObject.GetComponent<Health>();
            if (objectHealth != null)
            {
                StartCoroutine(AttackObject(objectHealth));
            }
        }
    }

    IEnumerator AttackPlayer(PlayerController1 player)
    {
        canAttack = false;
        player.TakeDamage(attack);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator AttackObject(Health objectHealth)
    {
        canAttack = false;
        objectHealth.TakeDamage(attack);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }


    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        DropReward();
        Destroy(gameObject);
    }

    private void DropReward()
    {
        foreach (var reward in rewards)
        {
            if (UnityEngine.Random.value <= reward.dropRate)
            {
                int dropAmount = UnityEngine.Random.Range(reward.minDrop, reward.maxDrop + 1);
                for (int i = 0; i < dropAmount; i++)
                {
                    Instantiate(reward.prefab, transform.position, Quaternion.identity);
                }
            }
        }
    }

    [Serializable]
    public class RewardItem
    {
        public GameObject prefab;
        public int minDrop = 1;
        public int maxDrop = 3;
        [Range(0, 1)] public float dropRate = 0.5f;
    }
}
