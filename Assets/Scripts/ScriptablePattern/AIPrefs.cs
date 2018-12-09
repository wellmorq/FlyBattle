using FlyBattle;
using UnityEngine;

namespace ScriptablePattern
{
    [CreateAssetMenu(menuName = "MyAssets/ScriptableObjects/AIPrefs", fileName = "AIPrefs", order = 2)]
    public class AIPrefs : ScriptableObject
    {
        public Profile[] AIProfiles;
    }
}