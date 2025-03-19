using Global;
using UnityEngine;
using FarmingGame.Planting.Block;

namespace FarmingGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;
        Constants consts;
        float mPlayerSpeed;
        Rigidbody2D mRigidbody;
        CapsuleCollider2D mCollider;
        Animator mAnimator;
        bool isMoving = false;
        bool hasSwitchedTool = false;
        Transform mToolIndicator;
        float mToolRange;
        bool mIsFaded = false;
        public bool isFaded => mIsFaded;
        public ToolType mCurrentTool;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }


            mRigidbody = GetComponent<Rigidbody2D>();
            mCollider = GetComponent<CapsuleCollider2D>();
            mAnimator = GetComponent<Animator>();
            consts = FindFirstObjectByType<Constants>();
            mToolIndicator = GameObject.FindGameObjectWithTag("Red_toolIndicator").transform;
        }

        void Start()
        {
            mPlayerSpeed = consts.gPLAYER_SPEED;
            mToolRange = consts.gToolRange;

            mCurrentTool = ToolType.wateringCan;
        }

        void Update()
        {
            Move();
#if UNITY_EDITOR
            ShiftInput();
#endif
            LiftFace();
            UseTool();
            PloughAnimation();
            WateringAnimation();
            SwitchToolTypeByTabKey();
            SwitchToolTypeByNumberKey();
            ToolIndicatorControl();
        }

        void ToolIndicatorControl()
        {
            Vector3 mousePosition = Input.mousePosition;
            mToolIndicator.position = Camera.main.ScreenToWorldPoint(mousePosition);
            mToolIndicator.position = new Vector3(mToolIndicator.position.x, mToolIndicator.position.y, 0f);
            AdjustToolIndicator();
            mToolIndicator.position = new Vector3(
                Mathf.FloorToInt(mToolIndicator.position.x) + .5f,
                Mathf.FloorToInt(mToolIndicator.position.y) + .5f,
                0f);
        }

        void AdjustToolIndicator()
        {
            if (Vector3.Distance(mToolIndicator.position, transform.position) > mToolRange)
            {
                Vector2 direction = mToolIndicator.position - transform.position;

                direction = direction.normalized * mToolRange;
                mToolIndicator.position = transform.position + new Vector3(direction.x, direction.y, 0f);
            }
        }

        public void Move()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // if (mToolWaitCounter > 0)
            // {
            //     mToolWaitCounter -= Time.deltaTime;
            // }
            // else
            // {
            Vector2 input = new Vector2(horizontal, vertical);
            if (input.magnitude > 1)
            {
                input = input.normalized;
            }
            mRigidbody.velocity = input * mPlayerSpeed;
            isMoving = true;
            mAnimator.SetBool("isMoving", isMoving);
            if (input.magnitude == 0)
            {
                isMoving = false;
                mAnimator.SetBool("isMoving", isMoving);
            }
            else
            {
                isMoving = true;
                mAnimator.SetBool("isMoving", isMoving);
            }
            // }
        }

        /// <summary>
        /// #if - #endif function
        /// </summary>
        void ShiftInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //Defaul speed = 4 || After Shift speed = 6
                mPlayerSpeed = consts.gPLAYER_SPEED * 3;
                mAnimator.SetFloat("speed", 1);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                //Defaul speed = 4 || After Shift speed = 6
                mPlayerSpeed = consts.gPLAYER_SPEED;
                mAnimator.SetFloat("speed", .5f);
            }
        }

        void LiftFace()
        {
            if (mRigidbody.velocity.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 0);
                mAnimator.SetFloat("horizontal", -1);
            }
            if (mRigidbody.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 0);
                mAnimator.SetFloat("horizontal", 1);
            }
        }

        void SwitchToolTypeByTabKey()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                mCurrentTool += 1;
                ChangeStateTool();
                hasSwitchedTool = true;
            }
        }

        void ChangeStateTool()
        {
            if ((int)mCurrentTool >= 4)
            {
                mCurrentTool = ToolType.plough;
            }
        }

        void UseTool()
        {
            GrowBlock block = null;
            // block = FindFirstObjectByType<GrowBlock>();
            //                block.PloughSoil();
            block = GridController.instance.GetBlock(mToolIndicator.position.x + .5f, mToolIndicator.position.y + .5f);
            if (block != null)
            {
                switch (mCurrentTool)
                {
                    case ToolType.plough:
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            block.PloughSoil();
                        }
                        break;
                    case ToolType.wateringCan:
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            block.WaterSoil();
                        }
                        break;
                    case ToolType.seeds:
                        if (Input.GetKeyDown(KeyCode.C))
                        {
                            block.PlantCrop();
                        }
                        break;
                    case ToolType.basket:
                        if (Input.GetKeyDown(KeyCode.X))
                        {
                            block.HarvestCrop();
                        }
                        break;
                }
            }
        }

        void SwitchToolTypeByNumberKey()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                mCurrentTool = ToolType.plough;

                hasSwitchedTool = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                mCurrentTool = ToolType.wateringCan;

                hasSwitchedTool = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                mCurrentTool = ToolType.seeds;

                hasSwitchedTool = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                mCurrentTool = ToolType.basket;

                hasSwitchedTool = true;
            }
            if (hasSwitchedTool == true)
            {
                UIController.instance.SwitchTool((int)mCurrentTool);
            }
        }

        void PloughAnimation()
        {
            bool isPloughing = false;
            if (Input.GetKeyDown(KeyCode.E))
            {
                isPloughing = true;
                mAnimator.SetBool("isPlough", isPloughing);
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                isPloughing = false;
                mAnimator.SetBool("isPlough", isPloughing);
            }
        }

        void WateringAnimation()
        {
            bool isWatering = false;
            if (Input.GetKeyDown(KeyCode.R))
            {
                isWatering = true;
                mAnimator.SetBool("isWatering", isWatering);
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                isWatering = false;
                mAnimator.SetBool("isWatering", isWatering);
            }
        }

        #region OnTrigger Methods
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Decor"))
                mIsFaded = true;
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Decor"))
                mIsFaded = true;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Decor"))
                mIsFaded = false;
        }
        #endregion
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("door"))
            {
                consts.gIsSwitched = false;
            }
        }
    }
}