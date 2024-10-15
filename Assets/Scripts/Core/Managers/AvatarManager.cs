using System.Collections.Generic;
using System.Linq;
using Core.GameData;
using UnityEngine;
using ScriptableObjects;
using Avatar = ScriptableObjects.Avatar;

namespace Core.Managers
{
    public class AvatarManager : MonoBehaviour
    {
        public List<Avatar> avatars; // All available avatars
    
        public Avatar GetEquippedAvatar(Item avatarType)
        {
            // Return the equipped avatar by its name
                 return avatars.FirstOrDefault(avatar => avatar.type == avatarType);
        }
    }

}