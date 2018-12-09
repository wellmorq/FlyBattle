using System.Collections;
using System.Collections.Generic;
using FlyBattle.UI;
using FlyBattle.Utils;
using Gamelogic.Extensions;
using UnityEngine;

namespace FlyBattle
{
    public class CloudMoving : GLMonoBehaviour
    {
        [SerializeField] private Sprite[] _cloudSprites;
        [SerializeField] private GameObject[] _clouds;
        [SerializeField] private float _speed = 0.01f;


        private float _startPointX, _endPointX, _startPointY, _endPointY; // Spawn point, transfer point
        private Coroutine _mover;
        private BorderSetup _border;

        #region Subscriptions

        private void OnEnable()
        {
            GameManager.Instance.GameInit += CloudsMoveStart;
            GameManager.Instance.GameEnd += CloudsMoveStop;

            GameManager.Instance.ResumeInvoke += CloudsMoveStart;
            GameManager.Instance.PauseInvoke += CloudsMoveStop;
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.GameInit -= CloudsMoveStart;
            GameManager.Instance.GameEnd -= CloudsMoveStop;

            GameManager.Instance.ResumeInvoke -= CloudsMoveStart;
            GameManager.Instance.PauseInvoke -= CloudsMoveStop;
        }

        #endregion

        private void CloudsMoveStart()
        {
            BorderInit();
            _mover = StartCoroutine(CloudsTranslate());
        }

        private void CloudsMoveStop()
        {
            StopCoroutine(_mover);
        }

        private void CloudRespawn(GameObject cloud)
        {
            if (cloud.transform.position.x <= _endPointX) return;
            cloud.transform.position = new Vector2(_startPointX, Random.Range(_startPointY, _endPointY));
            cloud.GetComponent<SpriteRenderer>().sprite = _cloudSprites[Random.Range(0, 3)];
        }

        private IEnumerator CloudsTranslate()
        {
            while (true)
            {
                foreach (var cloud in _clouds)
                {
                    cloud.transform.Translate(Vector2.right * Time.deltaTime * _speed);
                    CloudRespawn(cloud);
                }

                yield return null;
            }
        }

        private void BorderInit()
        {
            _startPointX = BorderSetup.BottomLeftCorner.x - 6f;
            _endPointX = BorderSetup.TopRightCorner.x + 6f;
            _startPointY = BorderSetup.BottomLeftCorner.y + 8.5f;
            _endPointY = BorderSetup.TopRightCorner.y - 2.5f;
        }
    }
}