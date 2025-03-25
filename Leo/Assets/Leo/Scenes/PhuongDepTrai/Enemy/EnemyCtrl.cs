using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Fin.Photon
{
    public class EnemyCtrl : FinalStateMachine
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private float roamTimer = 3f;
        [SerializeField] private float detectionRange = 5f;
        [SerializeField] private LayerMask playerLayer;

        private Vector2 moveDirection;
        private Transform targetPlayer;
        private bool isAttacking = false;

        protected override void Init()
        {
            if (!photonView.IsMine) return;
            SetDefautlState();
            StartCoroutine(Roam());
        }

        protected override void FSMFixedUpdate()
        {
            if (!photonView.IsMine) return;

            FindToPlayer();

            switch (currentState)
            {
                case State.Idle:
                    IdleState();
                    break;
                case State.Run:
                    RunState();
                    break;
                //case State.Attack:
                //    AttackState();
                //    break;
            }
        }

        private void FindToPlayer()
        {
            Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
            if (player != null)
            {
                targetPlayer = player.transform;
                ChangeState(State.Attack);
            }
            else
            {
                targetPlayer = null;
                if (currentState == State.Attack) ChangeState(State.Idle);
            }
        }

        private IEnumerator Roam()
        {
            while (true)
            {
                if (currentState != State.Attack) 
                {
                    moveDirection = Random.insideUnitCircle.normalized;
                    ChangeState(State.Run);
                }
                yield return new WaitForSeconds(roamTimer);
                ChangeState(State.Idle);
                yield return new WaitForSeconds(1f);
            }
        }

        private void IdleState()
        {
            PlayAnimation(Tag.IDLE);
            SetZeroVelocity();
        }

        private void RunState()
        {
            PlayAnimation(Tag.RUN);
            SetVelocity(moveDirection.x, moveDirection.y, speed);
        }

        //private void AttackState()
        //{
        //    if (targetPlayer == null) return;

        //    isAttacking = true;
        //    PlayAnimation(Tag.ATTACK);
        //    SetVelocity((targetPlayer.position.x - transform.position.x), 
        //                (targetPlayer.position.y - transform.position.y), speed * 1.5f);

        //    Invoke(nameof(ResetAttackState), 1f);
        //}

        //private void ResetAttackState()
        //{
        //    isAttacking = false;
        //    ChangeState(State.Idle);
        //}
    }
}
