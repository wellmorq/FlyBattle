using System;
using FlyBattle.Controllers;
using FlyBattle.Utils;
using Gamelogic.Extensions;
using FlyBattle.Interface;
using ScriptablePattern;
using UnityEngine;

namespace FlyBattle
{
    public class PlaneManager
    {
        public GameObject Plane { get; set; } //todo сделать приватным debug
        private GameObject SkyDriver { get; set; }
        private InputController Controller { get; }

        public Profile Profile { get; }

        public int Win;
        public int Life;

        public PlaneManager(Profile profile, Consts.BattleType type, GameObject homeObject)
        {
            Life = Consts.c_game_liveMaxCount;
            Win = 0;
            Profile = profile;
            if (profile == null)
                Debug.LogException(new UnityException($"Переданный профиль {nameof(profile)} = null!"));

            if (homeObject == null) homeObject = GameObject.FindWithTag(Consts.c_game_gameController_name);

            #region Инициализация контролёра

            if (profile == ProfileController.CurrentProfile)
                Controller = homeObject.AddComponent<PlayerInputController>();
            else
            {
                switch (type)
                {
                    case Consts.BattleType.SinglePlayer:
                        Debug.LogException(new UnityException($"Выбран неверный тип {nameof(Consts.BattleType)}"));
                        break;
                    case Consts.BattleType.MultiPlayer:
                        Controller = homeObject.AddComponent<NetworkInputController>();
                        break;
                    case Consts.BattleType.PvP:
                        Controller = homeObject.AddComponent<LocalInputController>();
                        break;
                    case Consts.BattleType.PvE:
                        Controller = homeObject.AddComponent<AIInputController>();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }

            if (homeObject.GetComponents<InputController>().Length > 2)
                Debug.LogException(
                    new UnityException(
                        $"Не должно быть больше двух контроллеров ввода типа {nameof(InputController)}!"));

            #endregion
        }


        #region Process of the plane destroying

        /// <summary>
        /// Entry point to Destroying Player Plane
        /// </summary>
        /// <param name="sender">Plane Health</param>
        /// <param name="e"></param>
        public void PlayerObjectDestroy(IHealth sender, ObjectDestroyEventArgs e)
        {
            IHealth health = null;
            if (Plane != null)
                if (sender == Plane.GetComponent<IHealth>())
                    health = sender;
            if (SkyDriver != null)
                if (sender == SkyDriver.GetComponent<IHealth>())
                    health = sender;
            if (health == null) return;

            Controller.Receiver = null;

            // Create new Plane instance
            if (e.CurrentDestroyedObject == DestroyedObject.SkyDriver) return;
            if (e.LifeDestroy < 0) return;
            // Destroy

            if (e.LifeDestroy == 0) Life = 0;
            else if (e.LifeDestroy >= 0) Life -= e.LifeDestroy;

            if (Life > 0) SetSkyDriver(e.ObjectPosition);
        }

        public void PlayerObjectCreate(IHealth sender, GameObject respawn)
        {
            IHealth health = null;
            if (SkyDriver != null)
                if (sender == SkyDriver.GetComponent<IHealth>())
                    health = sender;
            if (health == null) return;

            Controller.Receiver = null;
            Setup(respawn);
            ChangeReady(true);
        }

        /// <summary>
        /// SkyDriver disembark
        /// </summary>
        /// <param name="pos">Disembark point</param>
        private void SetSkyDriver(Vector3 pos)
        {
            SkyDriver.transform.position = pos;
            // Передаём в контроллера то, чем он будет управлять
            Controller.Receiver = SkyDriver.GetComponent<IControllerReceiver>();
            SkyDriver.SetActive(true);
        }

        #endregion


        /// <summary>
        /// Change Plane ready: shooting and moving
        /// </summary>
        public void ChangeReady(bool flag)
        {
            var changer = Plane != null ? Plane.GetComponent<IChangeReady>() : null;
            if (changer == null)
            {
                if (SkyDriver == null || !SkyDriver.activeSelf) return;
                SkyDriver.SetActive(flag);
                return;
            }

            changer.SetMove(flag);
            changer.SetShooting(flag);
        }

        public void Setup(GameObject spawn)
        {
            //-----------Инициализация самолёта ------------
            Plane = ObjectPool.Spawn(Database.Instance.GameItems.GetItem(Profile.PlaneInfo).Prefab,
                spawn.transform.position, Quaternion.identity);
            Plane.SetActive(false);
            //-----------Инициализация парашютиста ------------
            SkyDriver = ObjectPool.Spawn(Database.Instance.GameItems.GetItem(Profile.SkyDriverInfo).Prefab);
            SkyDriver.SetActive(false);

            var trans = Plane.transform;

            ChangeReady(false);

            switch (spawn.tag)
            {
                case Consts.c_game_spawnPoint_player1:
                    Plane.layer = LayerMask.NameToLayer(Consts.c_game_LayerName_player1);
                    SkyDriver.layer = LayerMask.NameToLayer(Consts.c_game_LayerName_player1);
                    trans.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Consts.c_game_spawnPoint_player2:
                    Plane.layer = LayerMask.NameToLayer(Consts.c_game_LayerName_player2);
                    SkyDriver.layer = LayerMask.NameToLayer(Consts.c_game_LayerName_player2);
                    trans.rotation = Quaternion.Euler(0, 0, 180);
                    trans.localScale = new Vector3(0.4f, -0.4f, 0.4f);
                    break;
            }

            // Передаём в контроллера то, чем он будет управлять
            Controller.Receiver = Plane.GetComponent<IControllerReceiver>();
            Plane.SetActive(true);
        }

        ~PlaneManager()
        {
        }
    }
}