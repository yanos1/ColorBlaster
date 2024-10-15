using Core.GameData;
using Core.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewAvatar", menuName = "Avatars/Avatar", order = 1)]

    public class Avatar : ScriptableObject
    {
        public static UserDataManager.FirebasePath path = UserDataManager.FirebasePath.avatarsOwned;
        public Item type;
        public Sprite playerSprite;
        
    }
}