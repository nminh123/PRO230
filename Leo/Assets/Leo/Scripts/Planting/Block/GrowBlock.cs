using Leo.Planting.Grid;
using UnityEngine;

namespace Leo.Planting.Block
{
    public class GrowBlock : MonoBehaviour
    {
        public GrowthStage currentStage;
        [SerializeField] SpriteRenderer mSpriteRenderer;
        public SpriteRenderer spriteRenderer 
        {
            get
            {
                return mSpriteRenderer;
            }
            set
            {
                mSpriteRenderer = value;
            }
        }
        [SerializeField] Sprite mSoilTiled, mWaterTiled;
        public bool isWatered = false;
        bool isPloughed = false;
        private bool mIsPreventUse = false;
        public bool isPreventUse
        {
            get
            {
                return mIsPreventUse;
            }
            set
            {
                mIsPreventUse = value;
            }
        }
        private Vector2Int gridPosition;

        [Header("Crops/Seeds")]
        [SerializeField] SpriteRenderer mCropRenderer;
        [SerializeField] Sprite
                                planted,
                                grow1,
                                grow2,
                                ripe;

        void Update()
        {
#if UNITY_EDITOR
            InputAdvanceCrop();
#endif
        }

        /// <summary>
        /// #if - #endif function
        /// </summary>
        void InputAdvanceCrop()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                AdvanceCrop();
            }
        }

        void AdvanceStage()
        {
            currentStage = currentStage + 1;

            if ((int)currentStage >= 6)
            {
                currentStage = GrowthStage.barren;
            }
        }

        void InputGrow()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetSoilSprite(currentStage, mSpriteRenderer);
            }
        }

        public void SetSoilSprite(GrowthStage stage, SpriteRenderer spriteRenderer)
        {
            if (stage == GrowthStage.barren)
            {
                spriteRenderer.sprite = null;
            }
            else
            {
                if (isWatered == true)
                {
                    spriteRenderer.sprite = mWaterTiled;
                }
                else
                {
                    spriteRenderer.sprite = mSoilTiled;
                    isPloughed = true;
                }
            }
            UpdateGridInfo();
        }

        public void SetSoilSprite()
        {
            SetSoilSprite(currentStage, mSpriteRenderer);
        }

        public void PloughSoil()
        {
            if (currentStage == GrowthStage.barren && mIsPreventUse == false)
            {
                currentStage = GrowthStage.ploughed;

                SetSoilSprite(currentStage, mSpriteRenderer);
            }
        }

        public void WaterSoil()
        {
            if (isPloughed == true && mIsPreventUse == false)
            {
                isWatered = true;
                SetSoilSprite(currentStage, mSpriteRenderer);
            }
        }

        public void PlantCrop()
        {
            if (currentStage == GrowthStage.ploughed && isWatered == true && mIsPreventUse == false)
            {
                currentStage = GrowthStage.planted;
                UpdateCropSprite();
            }
        }

        public void UpdateCropSprite()
        {
            switch (currentStage)
            {
                case GrowthStage.planted:
                    mCropRenderer.sprite = planted;
                    break;
                case GrowthStage.growing1:
                    mCropRenderer.sprite = grow1;
                    break;
                case GrowthStage.growing2:
                    mCropRenderer.sprite = grow2;
                    break;
                case GrowthStage.ripe:
                    mCropRenderer.sprite = ripe;
                    break;
            }

            UpdateGridInfo();
        }

        public void AdvanceCrop()
        {
            if (isWatered == true && mIsPreventUse == false)
            {
                if (currentStage == GrowthStage.planted ||
                currentStage == GrowthStage.growing1 ||
                currentStage == GrowthStage.growing2)
                {
                    currentStage++;

                    isWatered = false;
                    SetSoilSprite(currentStage, mSpriteRenderer);
                    UpdateCropSprite();
                }
            }
        }

        public void HarvestCrop()
        {
            if(currentStage == GrowthStage.ripe && mIsPreventUse == false)
            {
                currentStage = GrowthStage.ploughed;
                SetSoilSprite(currentStage, mSpriteRenderer);
                mCropRenderer.sprite = null;
            }
        }

        public void SetGridPosition(int x, int y)
        {
            gridPosition = new Vector2Int(x, y);
        }

        void UpdateGridInfo()
        {
            GridInfo.instance.UpdateInfo(this, gridPosition.x, gridPosition.y);
        }
    }
}