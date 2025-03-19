using UnityEngine;
using Leo.Planting.Block;
using Leo.Planting.Grid;
using Leo.UI;

namespace Leo.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;
        Constants consts;
        float playerSpeed;
        Rigidbody2D rb;
        CapsuleCollider2D collision;
        Animator animator;
        bool isMoving = false;
        bool hasSwitchedTool = false;
        Transform toolIndicator;
        float toolRange;
        bool isFaded = false;
        public bool IsFaded => isFaded;
        public ToolType currentTool;

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


            rb = GetComponent<Rigidbody2D>();
            collision = GetComponent<CapsuleCollider2D>();
            animator = GetComponent<Animator>();
            consts = FindFirstObjectByType<Constants>();
            toolIndicator = GameObject.FindGameObjectWithTag("Red_toolIndicator").transform;
        }

        void Start()
        {
            playerSpeed = consts.gPLAYER_SPEED;
            toolRange = consts.gToolRange;

            currentTool = ToolType.wateringCan;
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
            toolIndicator.position = Camera.main.ScreenToWorldPoint(mousePosition);
            toolIndicator.position = new Vector3(toolIndicator.position.x, toolIndicator.position.y, 0f);
            AdjustToolIndicator();
            toolIndicator.position = new Vector3(
                Mathf.FloorToInt(toolIndicator.position.x) + .5f,
                Mathf.FloorToInt(toolIndicator.position.y) + .5f,
                0f);
        }

        void AdjustToolIndicator()
        {
            if (Vector3.Distance(toolIndicator.position, transform.position) > toolRange)
            {
                Vector2 direction = toolIndicator.position - transform.position;

                direction = direction.normalized * toolRange;
                toolIndicator.position = transform.position + new Vector3(direction.x, direction.y, 0f);
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
            rb.velocity = input * playerSpeed;
            isMoving = true;
            animator.SetBool("isMoving", isMoving);
            if (input.magnitude == 0)
            {
                isMoving = false;
                animator.SetBool("isMoving", isMoving);
            }
            else
            {
                isMoving = true;
                animator.SetBool("isMoving", isMoving);
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
                playerSpeed = consts.gPLAYER_SPEED * 3;
                animator.SetFloat("speed", 1);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                //Defaul speed = 4 || After Shift speed = 6
                playerSpeed = consts.gPLAYER_SPEED;
                animator.SetFloat("speed", .5f);
            }
        }

        void LiftFace()
        {
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 0);
                animator.SetFloat("horizontal", -1);
            }
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 0);
                animator.SetFloat("horizontal", 1);
            }
        }

#region Can be delete stuff
        void SwitchToolTypeByTabKey()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                currentTool += 1;
                ChangeStateTool();
                hasSwitchedTool = true;
            }
        }

        void ChangeStateTool()
        {
            if ((int)currentTool >= 4)
            {
                currentTool = ToolType.plough;
            }
        }
#endregion

        //Chỉnh sửa lại nếu player đang chọn trang bị đó thì mới được sử dụng amin đó.
        void UseTool()
        {
            GrowBlock block = null;
            // block = FindFirstObjectByType<GrowBlock>();
            //                block.PloughSoil();
            block = GridController.instance.GetBlock(toolIndicator.position.x + .5f, toolIndicator.position.y + .5f);
            if (block != null)
            {
                switch (currentTool)
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

        //TODO: Đổi sang logic của UI Inventory
        void SwitchToolTypeByNumberKey()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentTool = ToolType.plough;

                hasSwitchedTool = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentTool = ToolType.wateringCan;

                hasSwitchedTool = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentTool = ToolType.seeds;

                hasSwitchedTool = true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                currentTool = ToolType.basket;

                hasSwitchedTool = true;
            }
            if (hasSwitchedTool == true)
            {
                UIController.instance.SwitchTool((int)currentTool);
            }
        }

        void PloughAnimation()
        {
            bool isPloughing = false;
            if (Input.GetKeyDown(KeyCode.E))
            {
                isPloughing = true;
                animator.SetBool("isPlough", isPloughing);
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                isPloughing = false;
                animator.SetBool("isPlough", isPloughing);
            }
        }

        void WateringAnimation()
        {
            bool isWatering = false;
            if (Input.GetKeyDown(KeyCode.R))
            {
                isWatering = true;
                animator.SetBool("isWatering", isWatering);
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                isWatering = false;
                animator.SetBool("isWatering", isWatering);
            }
        }

        #region OnTrigger Methods
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Decor"))
                isFaded = true;
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Decor"))
                isFaded = true;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Decor"))
                isFaded = false;
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