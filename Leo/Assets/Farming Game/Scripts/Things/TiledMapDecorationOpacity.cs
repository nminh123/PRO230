using FarmingGame.Player;
using Global;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FarmingGame.Things
{
    public class TiledMapDecorationOpacity : MonoBehaviour
    {
        private PlayerController mPlayer;
        private Tilemap mTileMap;
        private Constants mConsts;
        private Color mColor;

        void Awake()
        {
            mPlayer = FindObjectOfType<PlayerController>();
            mTileMap = GetComponent<Tilemap>();
            mConsts = FindObjectOfType<Constants>();
            mColor = mTileMap.color;
        }

        void Update()
        {
            UpdateTilemapOpacity(mPlayer.isFaded);
        }

        void UpdateTilemapOpacity(bool isFaded)
        {
            if (mTileMap == null || mConsts == null)
                return;

            float targetAlpha = isFaded ? mConsts.gCOLOR_BA : mConsts.gCOLOR_AA;

            if (Mathf.Approximately(mColor.a, targetAlpha))
                return;
//
            mColor.a = targetAlpha;
            mTileMap.color = mColor;

            // Debug.Log($"Tilemap alpha updated to: {mTileMap.color.a}");
        }
    }
}