using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;
using Apocalypse.AI.Base;
using Game.Tutorials;

[RequireComponent(typeof(AIPath), typeof(Seeker))]
public class EnemyBaseAI : BaseEnemy
{
    [Header("Tree Settings")]
    public float chopRange = 1f;
    public float chopInterval = 2f;
    public float searchRadius = 10f;
    public LayerMask treeLayer;

    [Header("Wood Carry Settings")]
    public int woodCapacity = 10;
    private int carriedWood = 0;

    [Header("UI Elements")]
    public TextMeshProUGUI woodText;

    [Header("Wood Gain Panel")]
    public GameObject woodGainPanelPrefab;
    public Transform woodPanelSpawnParent;

    private AIPath aiPath;
    private TreeItem targetTree;
    private float chopTimer = 0f;
    private Vector3 offsetPosition;
    private TreeGroupManager groupManagerTree;
    private Node _rootNode;

    private bool isUnderAttack = false;
    private Vector3 lastAttackSource;

    [SerializeField] private float runSpeed = 0;

    protected override void Start()
    {
        base.Start();
        aiPath = GetComponent<AIPath>();
        SetupBehaviorTree();
        InvokeRepeating(nameof(AutoFindNearestTree), 0f, 2f);
    }

    protected override void Update()
    {
        base.Update();
        if (currentState == EnemyState.Die) return;
        _rootNode?.Evaluate();
        UpdateSpriteDirection();
        UpdateWoodUI();
    }

    private void SetupBehaviorTree()
    {
        var flee = new SequenceNode(new List<Node>
        {
            new ConditionNode(() => isUnderAttack && currentHp <= stats.maxHP * 0.3f),
            new ActionNode(() => FleeWithSpeed(2.5f))
        });

        var isInventoryFull = new ConditionNode(() => carriedWood >= woodCapacity);
        var goBack = new ActionNode(() => GoBackToTurret());

        var isWoodAround = new ConditionNode(() => FindNearestTree() != null);
        var takeWood = new ActionNode(() => TakeSomeWood());

        var goFarm = new SequenceNode(new List<Node>
        {
            new InverterNode(isInventoryFull),
            isWoodAround,
            takeWood
        });

        var goBackTurret = new SequenceNode(new List<Node>
        {
            isInventoryFull,
            goBack
        });

        _rootNode = new SelectorNode(new List<Node>
        {
            flee,
            goBackTurret,
            goFarm
        });
    }

    private NodeState TakeSomeWood()
    {
        if (targetTree == null || targetTree.currentHealth <= 0 || !targetTree.gameObject.activeInHierarchy)
        {
            TreeItem newTree = FindNearestTree();
            if (newTree == null || newTree == targetTree)
            {
                targetTree = null;
                return NodeState.FAILURE;
            }
            targetTree = newTree;
            chopTimer = 0f;
            return NodeState.RUNNING;
        }

        float dist = Vector2.Distance(transform.position, targetTree.transform.position);
        if (dist <= chopRange)
        {
            aiPath.canMove = false;
            rb.velocity = Vector2.zero;
            animator.SetBool("isRun", false);
            SetState(EnemyState.Attack);

            chopTimer += Time.deltaTime;
            if (chopTimer >= chopInterval)
            {
                if (carriedWood < woodCapacity)
                {
                    targetTree.Chop();
                    carriedWood++;
                    ShowWoodGainEffect();
                    if (carriedWood >= woodCapacity) return NodeState.SUCCESS;
                }
                chopTimer = 0f;
            }
            return NodeState.RUNNING;
        }
        else
        {
            aiPath.canMove = true;
            aiPath.destination = offsetPosition;
            SetState(EnemyState.Run);
            return NodeState.RUNNING;
        }
    }

    private NodeState GoBackToTurret()
    {
        if (WoodStorage.Instance == null) return NodeState.FAILURE;

        aiPath.canMove = true;
        aiPath.destination = WoodStorage.Instance.storagePoint.position;
        SetState(EnemyState.Run);

        if (Vector2.Distance(transform.position, WoodStorage.Instance.storagePoint.position) < 0.5f)
        {
            CastleManager castle = FindObjectOfType<CastleManager>();
            if (castle != null)
                castle.AddResource(ResourceType.Wood, carriedWood);

            TreeItem.totalWood += carriedWood;
            carriedWood = 0;
            targetTree = null;
            AutoFindNearestTree();
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }

    private NodeState FleeWithSpeed(float runSpeed)
    {
        this.runSpeed = runSpeed;
        Vector3 dir = (transform.position - lastAttackSource).normalized;
        Vector3 target = transform.position + dir * 10f;
        aiPath.canMove = true;
        aiPath.maxSpeed = runSpeed;
        aiPath.destination = target;
        SetState(EnemyState.Run);
        return NodeState.RUNNING;
    }

    private void UpdateWoodUI()
    {
        if (woodText != null)
            woodText.text = "Wood: " + carriedWood + "/" + woodCapacity;
    }

    private void ShowWoodGainEffect()
    {
        if (woodGainPanelPrefab != null && woodPanelSpawnParent != null)
        {
            Vector3 spawnPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
            Instantiate(woodGainPanelPrefab, woodPanelSpawnParent).transform.position = spawnPos;
        }
    }

    public void AutoFindNearestTree()
    {
        FindNearestTree();
    }

    public TreeItem FindNearestTree()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, searchRadius, treeLayer);
        float minDist = Mathf.Infinity;
        TreeItem closest = null;

        foreach (var hit in hits)
        {
            TreeItem tree = hit.GetComponent<TreeItem>();
            if (tree != null && tree.currentHealth > 0)
            {
                float dist = Vector3.Distance(transform.position, tree.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = tree;
                }
            }
        }

        if (closest != null)
        {
            SetTargetWithOffset(closest);
        }

        return closest;
    }

    private void SetTargetWithOffset(TreeItem tree)
    {
        if (groupManagerTree != null)
        {
            groupManagerTree.UnregisterWooder(this);
            groupManagerTree = null;
        }

        targetTree = tree;

        groupManagerTree = tree.GetComponent<TreeGroupManager>();
        if (groupManagerTree == null)
            groupManagerTree = tree.gameObject.AddComponent<TreeGroupManager>();

        groupManagerTree.RegisterWooder(this);
        SetOffsetPosition(tree.transform.position + (Vector3)(Random.insideUnitCircle * 0.5f));
    }

    public void SetOffsetPosition(Vector3 pos)
    {
        offsetPosition = pos;
        if (aiPath != null)
        {
            aiPath.destination = offsetPosition;
        }
    }

    private void UpdateSpriteDirection()
    {
        if (aiPath == null || spriteRenderer == null) return;
        if (Mathf.Abs(aiPath.velocity.x) > 0.01f)
            spriteRenderer.flipX = aiPath.velocity.x < 0f;
    }

    protected override void Idle()
    {
        base.Idle();
        if (animator != null)
        {
            animator.SetBool("isRun", false);
        }
    }

    protected override void Run()
    {
        base.Run();
        if (animator != null && aiPath != null)
        {
            animator.SetBool("isRun", aiPath.velocity.magnitude > 0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lastAttackSource = other.transform.position;
            isUnderAttack = true;
            TakeCustomDamage(100);
            StartCoroutine(ResetUnderAttackFlag());
        }
    }

    private void TakeCustomDamage(int amount)
    {
        currentHp -= amount;
        Debug.Log($"{stats.enemyName} bị tấn công và mất {amount} máu. HP còn lại: {currentHp}");

        if (currentHp <= 0 && currentState != EnemyState.Die)
        {
            SetState(EnemyState.Die);
        }
    }

    private IEnumerator ResetUnderAttackFlag()
    {
        yield return new WaitForSeconds(5f);
        isUnderAttack = false;
    }
}
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Pathfinding;
// using TMPro;
// using Apocalypse.AI.Base;
// using Game.Tutorials;

// [RequireComponent(typeof(AIPath), typeof(Seeker))]
// public class EnemyBaseAI : BaseEnemy
// {
//     [Header("Tree Settings")]
//     public float chopRange = 1f;
//     public float chopInterval = 2f;
//     public float searchRadius = 10f;
//     public LayerMask treeLayer;

//     [Header("Wood Carry Settings")]
//     public int woodCapacity = 10;
//     private int carriedWood = 0;

//     [Header("UI Elements")]
//     public TextMeshProUGUI woodText;

//     [Header("Wood Gain Panel")]
//     public GameObject woodGainPanelPrefab;
//     public Transform woodPanelSpawnParent;

//     [Header("Flee Settings")]
//     public float fleeDistance = 10f;
//     public float fleeSpeedMultiplier = 2.5f;
//     public float safeDistanceFromPlayer = 8f;
//     public float safeTime = 3f;
//     public LayerMask playerLayer;

//     private AIPath aiPath;
//     private TreeItem targetTree;
//     private float chopTimer = 0f;
//     private Vector3 offsetPosition;
//     private TreeGroupManager groupManagerTree;

//     private Node _rootNode;
//     private bool isUnderAttack = false;
//     private Vector3 lastAttackSource;
//     private bool isFleeing = false;
//     private float safeTimer = 0f;
//     private Vector3 fleeTargetPosition;
//     private bool fleeTargetSet = false;


//     protected override void Start()
//     {
//         base.Start();
//         aiPath = GetComponent<AIPath>();
//         SetupBehaviorTree();
//         InvokeRepeating(nameof(AutoFindNearestTree), 0f, 2f);
//     }

//     protected override void Update()
//     {
//         base.Update();
//         if (currentState == EnemyState.Die) return;

//         if (isFleeing)
//         {
//             safeTimer += Time.deltaTime;

//             if (!IsPlayerNearby() && safeTimer >= safeTime)
//             {
//                 isFleeing = false;
//                 isUnderAttack = false;
//                 fleeTargetSet = false;
//                 aiPath.maxSpeed = stats.moveSpeed;
//             }
//         }

//         _rootNode?.Evaluate();
//         UpdateSpriteDirection();
//         UpdateWoodUI();
//     }

//     private void SetupBehaviorTree()
//     {
//         var flee = new SequenceNode(new List<Node>
//         {
//             new ConditionNode(() => isFleeing),
//             new ActionNode(() => ContinueFleeing())
//         });

//             var triggerFlee = new SequenceNode(new List<Node>
//         {
//             new ConditionNode(() => isUnderAttack && currentHp <= stats.maxHP * 0.3f),
//             new ActionNode(() => StartFleeing())
//         });

//             var isInventoryFull = new ConditionNode(() => carriedWood >= woodCapacity);
//             var goBack = new ActionNode(() => GoBackToTurret());

//             var isWoodAround = new ConditionNode(() => FindNearestTree() != null);
//             var takeWood = new ActionNode(() => TakeSomeWood());

//             var goFarm = new SequenceNode(new List<Node>
//         {
//             new InverterNode(isInventoryFull),
//             isWoodAround,
//             takeWood
//         });

//             var goBackTurret = new SequenceNode(new List<Node>
//         {
//             isInventoryFull,
//             goBack
//         });

//             _rootNode = new SelectorNode(new List<Node>
//         {
//             flee,          // nếu đang chạy trốn thì ưu tiên chạy tiếp
//             triggerFlee,   // nếu mới bị tấn công thì bắt đầu chạy
//             goBackTurret,
//             goFarm
//         });
//     }

//     private NodeState FleeWithSpeed(float runSpeed)
//     {
//         if (!isFleeing)
//         {
//             Vector3 dir = (transform.position - lastAttackSource).normalized;
//             fleeTargetPosition = transform.position + dir * fleeDistance;
//             aiPath.destination = fleeTargetPosition;
//             aiPath.maxSpeed = runSpeed;
//             aiPath.canMove = true;
//             SetState(EnemyState.Run);
//             isFleeing = true;
//             safeTimer = 0f;
//             return NodeState.RUNNING;
//         }

//         float distanceToPlayer = Vector3.Distance(transform.position, lastAttackSource);
//         if (distanceToPlayer >= safeDistanceFromPlayer)
//         {
//             safeTimer += Time.deltaTime;
//             if (safeTimer >= safeTime)
//             {
//                 isUnderAttack = false;
//                 isFleeing = false;
//                 return NodeState.SUCCESS;
//             }
//         }
//         else
//         {
//             safeTimer = 0f; // reset timer nếu vẫn còn gần player
//         }

//         return NodeState.RUNNING;
//     }


//     private NodeState StartFleeing()
//     {
//         isFleeing = true;
//         fleeTargetSet = false;
//         safeTimer = 0f;
//         return NodeState.SUCCESS;
//     }
//     private NodeState ContinueFleeing()
//     {
//         // Nếu chưa đặt hướng chạy, thì đặt
//         if (!fleeTargetSet)
//         {
//             Vector3 dir = (transform.position - lastAttackSource).normalized;
//             fleeTargetPosition = transform.position + dir * fleeDistance;
//             aiPath.maxSpeed = stats.moveSpeed * fleeSpeedMultiplier;
//             aiPath.canMove = true;
//             aiPath.destination = fleeTargetPosition;
//             fleeTargetSet = true;
//         }

//         SetState(EnemyState.Run);

//         // Nếu player còn trong vùng nguy hiểm, reset timer
//         Collider2D playerNearby = Physics2D.OverlapCircle(transform.position, safeDistanceFromPlayer, playerLayer);
//         if (playerNearby)
//         {
//             safeTimer = 0f;
//             return NodeState.RUNNING;
//         }

//         // Player không còn gần
//         safeTimer += Time.deltaTime;
//         if (safeTimer >= safeTime)
//         {
//             isFleeing = false;
//             fleeTargetSet = false;
//             safeTimer = 0f;
//             return NodeState.SUCCESS;
//         }

//         return NodeState.RUNNING;
//     }
//     private bool IsPlayerNearby()
//     {
//         Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, safeDistanceFromPlayer, playerLayer);
//         return hits.Length > 0;
//     }

//     private NodeState TakeSomeWood()
//     {
//         if (isFleeing) return NodeState.FAILURE;

//         if (targetTree == null || targetTree.currentHealth <= 0 || !targetTree.gameObject.activeInHierarchy)
//         {
//             TreeItem newTree = FindNearestTree();
//             if (newTree == null || newTree == targetTree)
//             {
//                 targetTree = null;
//                 return NodeState.FAILURE;
//             }
//             targetTree = newTree;
//             chopTimer = 0f;
//             return NodeState.RUNNING;
//         }

//         float dist = Vector2.Distance(transform.position, targetTree.transform.position);
//         if (dist <= chopRange)
//         {
//             aiPath.canMove = false;
//             rb.velocity = Vector2.zero;
//             animator.SetBool("isRun", false);
//             SetState(EnemyState.Attack);

//             chopTimer += Time.deltaTime;
//             if (chopTimer >= chopInterval)
//             {
//                 if (carriedWood < woodCapacity)
//                 {
//                     targetTree.Chop();
//                     carriedWood++;
//                     ShowWoodGainEffect();
//                     if (carriedWood >= woodCapacity) return NodeState.SUCCESS;
//                 }
//                 chopTimer = 0f;
//             }
//             return NodeState.RUNNING;
//         }
//         else
//         {
//             aiPath.canMove = true;
//             aiPath.destination = offsetPosition;
//             SetState(EnemyState.Run);
//             return NodeState.RUNNING;
//         }
//     }

//     private NodeState GoBackToTurret()
//     {
//         if (isFleeing) return NodeState.FAILURE;

//         if (WoodStorage.Instance == null) return NodeState.FAILURE;

//         aiPath.canMove = true;
//         aiPath.destination = WoodStorage.Instance.storagePoint.position;
//         SetState(EnemyState.Run);

//         if (Vector2.Distance(transform.position, WoodStorage.Instance.storagePoint.position) < 0.5f)
//         {
//             CastleManager castle = FindObjectOfType<CastleManager>();
//             if (castle != null)
//                 castle.AddResource(ResourceType.Wood, carriedWood);

//             TreeItem.totalWood += carriedWood;
//             carriedWood = 0;
//             targetTree = null;
//             AutoFindNearestTree();
//             return NodeState.SUCCESS;
//         }

//         return NodeState.RUNNING;
//     }

//     public void AutoFindNearestTree() => FindNearestTree();

//     public TreeItem FindNearestTree()
//     {
//         if (isUnderAttack || isFleeing) return null;
//         Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, searchRadius, treeLayer);
//         float minDist = Mathf.Infinity;
//         TreeItem closest = null;

//         foreach (var hit in hits)
//         {
//             TreeItem tree = hit.GetComponent<TreeItem>();
//             if (tree != null && tree.currentHealth > 0)
//             {
//                 float dist = Vector3.Distance(transform.position, tree.transform.position);
//                 if (dist < minDist)
//                 {
//                     minDist = dist;
//                     closest = tree;
//                 }
//             }
//         }

//         if (closest != null)
//             SetTargetWithOffset(closest);

//         return closest;
//     }

//     private void SetTargetWithOffset(TreeItem tree)
//     {
//         if (groupManagerTree != null)
//         {
//             groupManagerTree.UnregisterWooder(this);
//             groupManagerTree = null;
//         }

//         targetTree = tree;
//         groupManagerTree = tree.GetComponent<TreeGroupManager>() ?? tree.gameObject.AddComponent<TreeGroupManager>();
//         groupManagerTree.RegisterWooder(this);

//         SetOffsetPosition(tree.transform.position + (Vector3)(Random.insideUnitCircle * 0.5f));
//     }

//     public void SetOffsetPosition(Vector3 pos)
//     {
//         offsetPosition = pos;
//         if (aiPath != null)
//             aiPath.destination = offsetPosition;
//     }

//     private void UpdateWoodUI()
//     {
//         if (woodText != null)
//             woodText.text = $"Wood: {carriedWood}/{woodCapacity}";
//     }

//     private void ShowWoodGainEffect()
//     {
//         if (woodGainPanelPrefab != null && woodPanelSpawnParent != null)
//         {
//             Vector3 spawnPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
//             Instantiate(woodGainPanelPrefab, woodPanelSpawnParent).transform.position = spawnPos;
//         }
//     }

//     private void UpdateSpriteDirection()
//     {
//         if (aiPath == null || spriteRenderer == null) return;
//         if (Mathf.Abs(aiPath.velocity.x) > 0.01f)
//             spriteRenderer.flipX = aiPath.velocity.x < 0f;
//     }

//     protected override void Idle()
//     {
//         base.Idle();
//         animator?.SetBool("isRun", false);
//     }

//     protected override void Run()
//     {
//         base.Run();
//         animator?.SetBool("isRun", aiPath.velocity.magnitude > 0.1f);
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             lastAttackSource = other.transform.position;
//             TakeCustomDamage(100);
//         }
//     }

//     private void TakeCustomDamage(int amount)
//     {
//         currentHp -= amount;
//         isUnderAttack = true;

//         if (currentHp <= 0 && currentState != EnemyState.Die)
//         {
//             SetState(EnemyState.Die);
//         }

//         StopCoroutine(nameof(ResetUnderAttackFlag));
//         StartCoroutine(ResetUnderAttackFlag());
//     }

//     private IEnumerator ResetUnderAttackFlag()
//     {
//         yield return new WaitForSeconds(1f);
//         isUnderAttack = false;
//     }
// }
