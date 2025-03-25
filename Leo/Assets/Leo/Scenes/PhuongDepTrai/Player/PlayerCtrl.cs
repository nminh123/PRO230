using UnityEngine;
using Photon.Pun;
using System.Collections;

namespace Fin.Photon
{
    public class PlayerCtrl : FinalStateMachine
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private PlayerStats playerStats;
        private float horizontal;
        private float vertical;

        private bool isAttack = false;
        private bool isDie = false;
        protected override void Init()
        {
            if (!photonView.IsMine) return;
            SetDefautlState();
        }

        protected override void FSMUpdate()
        {
            if (!photonView.IsMine) return;

            if(playerStats.CurrentHealth <= 0 && !isDie)
                ChangeState(State.Die);
            CheckInput();
        }
        private void CheckInput()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttack)
                ChangeState(State.Attack);
        }
        protected override void FSMFixedUpdate()
        {
            if (!photonView.IsMine) return;

            switch (currentState)
            {
                case State.Idle:
                    IdleState();
                    break;
                case State.Run:
                    RunState();
                    break;
                case State.Attack:
                    AttackState();
                    break;
                case State.Die:
                    DieState();
                    break;
            }
        }

        private void IdleState()
        {
            if (isDie) return;
            PlayAnimation(Tag.IDLE);

            if (Mathf.Abs(horizontal) > Mathf.Epsilon || Mathf.Abs(vertical) > Mathf.Epsilon)
                ChangeState(State.Run);
        }

        private void RunState()
        {
            if (isDie) return;
            PlayAnimation(Tag.RUN);
            SetVelocity(horizontal, vertical, speed);

            if (Mathf.Abs(horizontal) <= Mathf.Epsilon && Mathf.Abs(vertical) <= Mathf.Epsilon)
                ChangeState(State.Idle);

        }
        private void AttackState()
        {
            if(isDie) return;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(Tag.ATTACK))
            {
                PlayAnimation(Tag.ATTACK);
                SetZeroVelocity();
            }

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .8f)
            {
                ChangeState(State.Idle);
                SetVelocity(horizontal,vertical,speed);
            }
        }
        private void DieState()
        {
            if(playerStats.CurrentHealth <= 0)
            {
                PlayAnimation(Tag.DIE);
                isDie = true;
            }
            else
                isDie = false;
        }
    }
}
