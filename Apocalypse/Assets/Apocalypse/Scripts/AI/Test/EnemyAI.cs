using System.Collections.Generic;
using UnityEngine;
using Apocalypse.AI.Base;

namespace Apocalypse.AI.Test
{
    public class EnemyAI : MonoBehaviour
    {
        // target
        public Transform targetPlayer;
        // phạm vi phát hiện
        public float detectionRange = 10f;
        // ngưỡng máu chạy trốn
        public float fleeHealthThreshold = 20f;
        // Máu của kẻ thù
        public float currentHealth = 100f;

        // Danh sách các điểm đi tuần
        public Transform[] patrolPoints;
        private int currentPatrolIndex = 0;
        // tốc độ đi tuần
        public float patrolSpeed = 2f;
        // tốc độ chạy trốn
        public float fleeSpeed = 5f;
        // tốc độ tấn công
        public float attackSpeed = 3f;

        // khoảng cách đến điểm đi tuần
        public float minDistanceToPoint = 0.5f;

        private Node _rootNode; // Cây hành vi gốc

        private Animator _animator; // Animator của kẻ thù

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _animator = GetComponent<Animator>();
            SetupBehaviorTree();
        }

        // Update is called once per frame
        void Update()
        {
            _rootNode?.Evaluate(); // Đánh giá cây hành vi
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            // gọi ở đây
        }

        // Hàm khởi tạo cây hành vi
        private void SetupBehaviorTree()
        {
            // Điều kiện: HP bé hơn 20%
            var isLowHP = new ConditionNode(() => currentHealth < fleeHealthThreshold);
            // Hành động: Chạy trốn khỏi Player
            var fleeAction = new ActionNode(FleeFromPlayer);

            // Điều kiện: Player trong phạm vi phát hiện
            var isPlayerInRange = new ConditionNode(IsPlayerInDetectionRange);
            // Hành động: Tấn công Player
            var attackAction = new ActionNode(AttackPlayer);

            // Hành động: Đi tuần qua các điểm
            var patrolAction = new ActionNode(Patrol);

            // Tạo cây hành vi
            _rootNode = new SelectorNode(new List<Node>
        {
            // nếu HP thấp thì chạy trốn
            new SequenceNode(new List<Node> { isLowHP, fleeAction }),
            // nếu Player trong phạm vi thì tấn công
            new SequenceNode(new List<Node> { isPlayerInRange, attackAction }),
            // nếu không thì đi tuần
            patrolAction
        });
        }

        // các hàm hành động với điều kiện cụ thể
        private NodeState FleeFromPlayer()
        {
            Debug.Log("Enemy is fleeing from player.....");
            if (targetPlayer == null) return NodeState.FAILURE;
            // tìm 1 điểm an toàn ngược hướng với Player
            Vector3 runDirection = transform.position - targetPlayer.position;
            Vector3 fleePosition = transform.position + runDirection.normalized * 20f;
            // enemy chạy ngược hướng với Player
            transform.position = Vector3.MoveTowards(transform.position,
                    fleePosition, fleeSpeed * Time.deltaTime);
            // cập nhật Animator
            if (_animator != null)
            {
                _animator.SetBool("isRunning", true);
            }
            // nếu đa đến gần vị trí an toàn thì coi như chạy trốn thành công
            if (Vector3.Distance(transform.position, fleePosition) < minDistanceToPoint)
            {
                Debug.Log("Enemy: Đã đến đểm chạy trốn......");
                if (_animator != null)
                {
                    _animator.SetBool("isRunning", false);
                }
                return NodeState.SUCCESS;
            }
            return NodeState.RUNNING;

            // // dành cho 3D
            // UnityEngine.AI.NavMeshHit hit;
            // if (UnityEngine.AI.NavMesh.SamplePosition(fleePosition, out hit, 20f, 
            //         UnityEngine.AI.NavMesh.AllAreas))
            // {
            //     transform.position = hit.position; // di chuyển đến vị trí an toàn
            // }
        }

        // hàm kiểm tra nhân vật Player có trong phạm vi phát hiện hay không
        private bool IsPlayerInDetectionRange()
        {
            if (targetPlayer == null) return false;
            // kiểm tra khoảng cách với Player
            return Vector3.Distance(transform.position, targetPlayer.position) <= detectionRange;
        }

        // tấn công nhân vật
        private NodeState AttackPlayer()
        {
            Debug.Log("Enemy is attacking the player.....");
            if (targetPlayer == null) return NodeState.FAILURE;
            // Di chuyển đến gần Player và tấn công
            transform.position = Vector3.MoveTowards(transform.position,
                    targetPlayer.position, attackSpeed * Time.deltaTime);
            // thêm logic tấn công (ví dụ: giảm máu của Player, tạo sát thương, effects, ...)
            // Giả sử tấn công thành công nếu đã đến gần Player
            if (Vector3.Distance(transform.position, targetPlayer.position) < minDistanceToPoint)
            {
                Debug.Log("Enemy: Đã tấn công Player thành công......");
                // cập nhật Animator
                // if (_animator != null)
                // {
                //     _animator.SetTrigger("Attack");
                // }
                return NodeState.SUCCESS;
            }
            return NodeState.RUNNING; // Vẫn đang di chuyển hoặc tấn công
        }

        // đi tuần qua các điểm
        private NodeState Patrol()
        {
            Debug.Log("Enemy is patrolling.....");
            if (patrolPoints.Length == 0) return NodeState.FAILURE; // Không có điểm đi tuần

            Vector3 targetPoint = patrolPoints[currentPatrolIndex].position;
            // Di chuyển đến điểm đi tuần
            transform.position = Vector3.MoveTowards(transform.position,
                    targetPoint, patrolSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPoint) < minDistanceToPoint)
            {
                // Đã đến điểm đi tuần, chuyển sang điểm tiếp theo
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                Debug.Log($"Enemy: Đã đến điểm đi tuần {currentPatrolIndex}, " +
                          $"chuyển sang điểm tiếp theo......");
            }

            return NodeState.RUNNING;
        }
    }
}