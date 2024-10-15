using Core.GameData;
using Core.Managers;
using UnityEngine;
using Avatar = ScriptableObjects.Avatar;

namespace GameLogic.PlayerRelated
{
    public class PlayerAvatarHandler : MonoBehaviour
    {
        [SerializeField] private AvatarManager avatarManager;

        [SerializeField] private SpriteRenderer playerRenderer;


        public void SetPlayerAvatar()
        {
            Item currentAvatarType =
                CoreManager.instance.UserDataManager.GetEquippedItem(UserDataManager.FirebasePath.avatarsOwned);
            Avatar avatar = avatarManager.GetEquippedAvatar(currentAvatarType);
            playerRenderer.sprite = avatar.playerSprite;
        }
    }
}