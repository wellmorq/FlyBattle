using FlyBattle.Controllers;
using FlyBattle.Utils;
using UnityEditor;

namespace ScriptablePattern
{
    public class Database : Singleton<Database>
    {
        public AIPrefs AIPrefs;
        public ProfileController ProfileController;
        public LocalizationController LocalizationController;
        public PlayerSettings PlayerSettings;
        public GameItems GameItems;
    }
}