using UnityEngine;
using Photon.Pun;
using System.Collections;

namespace Fin.Photon
{
    public class PlayerCtrl : FinalStateMachine
    {
        [SerializeField] private float speed = 10f;

        private float horizontal;
        private float vertical;

        private bool isAttack = false;
        protected override void Init()
        {
            if (!photonView.IsMine) return;
            SetDefautlState();
        }

        protected override void FSMUpdate()
        {
            if (!photonView.IsMine) return;

            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttack)
            {
                ChangeState(State.Attack);
            }
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
            }
        }

        private void IdleState()
        {
            PlayAnimation(Tag.IDLE);

            if (Mathf.Abs(horizontal) > Mathf.Epsilon || Mathf.Abs(vertical) > Mathf.Epsilon)
            {
                ChangeState(State.Run);
            }
        }

        private void RunState()
        {
            PlayAnimation(Tag.RUN);
            SetVelocity(horizontal, vertical, speed);

            if (Mathf.Abs(horizontal) <= Mathf.Epsilon && Mathf.Abs(vertical) <= Mathf.Epsilon)
            {
                ChangeState(State.Idle);
            }
        }
        private void AttackState()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                PlayAnimation(Tag.ATTACK);
                SetZeroVelocity();
            }

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .9f)
            {
                ChangeState(State.Idle);
                SetVelocity(horizontal,vertical,speed);
            }

        }
    }
}
