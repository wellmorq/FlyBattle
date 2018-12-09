using System.Collections;
using FlyBattle.Interface;
using FlyBattle.UI;
using FlyBattle.Utils;
using UnityEngine;

namespace FlyBattle.Controllers
{
    [RequireComponent(typeof(IEngine))]
    [RequireComponent(typeof(IHealth))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlaneController : MonoBehaviour, IControllerReceiver, IChangeReady
    {
        [SerializeField] private float RotateSpeed = 5f;

        private Rigidbody2D _rb2D;
        private IEngine _engine;
        private IWeapon _weapon;
        private IHealth _health;

        private InputController _input;

        [Tooltip("Plane flip time, sec")] [SerializeField]
        private int flipTime = 60;

        private Coroutine c_flip;

        [SerializeField] private bool _moveLeft;
        private float _normalScaleY;

        private const float DistanceColliderFix = 0.5f;

        private void Start()
        {
            _moveLeft = gameObject.layer.Equals(LayerMask.NameToLayer(Consts.c_game_LayerName_player2));
            _engine = GetComponent<IEngine>();
            _health = GetComponent<IHealth>();
            _normalScaleY = Mathf.Abs(transform.localScale.y);
        }

        private void OnEnable()
        {
            SetMove(false);
            SetShooting(false);
        }

        private void OnDisable()
        {
            if (c_flip != null) StopCoroutine(c_flip);
            StopAllCoroutines();
        }

        private void Update()
        {
            FlipAround();
        }

        public void SetMove(bool flag)
        {
            if (_rb2D == null) _rb2D = GetComponent<Rigidbody2D>();
            _rb2D.simulated = flag;
        }

        public void SetShooting(bool flag)
        {
            if (_weapon == null) _weapon = GetComponent<IWeapon>();
            _weapon.SetShootActive(flag);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            int colLayer = collision.gameObject.layer;

            switch (colLayer)
            {
                case 13: // LeftWall
                    InvertPlayerPositionX(transform.position);
                    break;
                case 14: // RightWall
                    InvertPlayerPositionX(transform.position);
                    break;
                case 15: // Ceiling
                    _engine.StopEngine(obj: collision.gameObject);
                    break;
                case 12: // Ground
                    _health.OnDestroyObject(0, DestroyedObject.Plane);
                    break;
                case 11: // NonDestructionObjects
                    break;
                default:
                    break;
            }
        }

        public void Shoot()
        {
            _weapon?.OnShoot();
        }

        public void Move(float h_Input)
        {
            // Alternative version border control for plane
            /* 
            var posX = transform.position.x;
            if (posX < BorderSetup.BottomLeftCorner.x || posX > BorderSetup.TopRightCorner.x)
            InvertPlayerPositionX(transform.position);
            */

            if (_rb2D == null) return;
            _rb2D.MoveRotation(_rb2D.rotation -
                               h_Input * RotateSpeed
            );
        }

        // ----- flip around axe -----

        #region Flip 

        private void FlipAround()
        {
            if (Mathf.Abs(transform.rotation.z) > 0.75f)
                if (_moveLeft)
                    return;
                else
                {
                    DoFlip();
                    _moveLeft = true;
                }

            if (Mathf.Abs(transform.rotation.z) < 0.75f)
                if (!_moveLeft)
                    return;
                else
                {
                    DoFlip();
                    _moveLeft = false;
                }
        }

        private void DoFlip()
        {
            if (c_flip != null)
            {
                StopCoroutine(c_flip);
                ResetFlip();
                c_flip = null;
            }

            c_flip = StartCoroutine(Flip());
        }

        private IEnumerator Flip()
        {
            var step = (float) 2 / flipTime * (_moveLeft ? 1f : -1f);
            var scale = transform.localScale;
            var i = 0;
            //_moveLeft = !_moveLeft;
            while (i < flipTime)
            {
                i++;
                transform.localScale = new Vector3(scale.x, scale.y + step * i * _normalScaleY, scale.z);
                yield return null;
            }

            c_flip = null;
        }

        private void ResetFlip()
        {
            transform.localScale = _moveLeft
                ? new Vector3(_normalScaleY, -_normalScaleY, _normalScaleY)
                : new Vector3(_normalScaleY, _normalScaleY, _normalScaleY);
        }

        #endregion

        // ----- if border crossing -----
        private void InvertPlayerPositionX(Vector2 pos)
        {
            transform.position =
                pos.x > 0
                    ? new Vector2((pos.x - DistanceColliderFix) * -1, pos.y)
                    : new Vector2((pos.x + DistanceColliderFix) * -1, pos.y);
        }
    }
}