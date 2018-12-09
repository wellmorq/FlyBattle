using System.Collections;
using FlyBattle.Interface;
using FlyBattle.UI;
using Gamelogic.Extensions;
using UnityEngine;

namespace FlyBattle.Player
{
    public class SkyDriverController : MonoBehaviour, IControllerReceiver
    {
        public bool IsLanding => _isLanding;

        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float angleRotate = 30f;
        [SerializeField] private float secondsDelayFullRotate = 2f;
        [SerializeField] private float maxVelocityDown = -2f;

        private bool _isLanding = false;
        private bool _isGoing = false;
        private Rigidbody2D _rb2D;
        private SpriteRenderer _sprite;
        private Coroutine c_Run;

        private void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _isLanding = false;
            _isGoing = false;
        }

        public void Shoot()
        {
            // Reaction on click Fire1 or Fire2
        }

        public void Move(float h_Input)
        {
            if (_isLanding) return;
            if (_rb2D == null) return;

            transform.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0, 0, angleRotate * h_Input),
                secondsDelayFullRotate);

            var velocity = _rb2D.velocity;
            var velocityY = velocity.y >= maxVelocityDown ? velocity.y : maxVelocityDown;
            _rb2D.velocity = new Vector2(h_Input * moveSpeed, velocityY);

            _sprite.flipX = h_Input < 0;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var colLayer = other.gameObject.layer;
            var groundLayer = LayerMask.NameToLayer("Ground"); // 12

            if (colLayer != groundLayer) return;

            _isLanding = true;
            transform.rotation = Quaternion.identity;
            AutoPilot();
        }

        /// <summary>
        /// Gets SkyDriver off the screen
        /// </summary>
        private void AutoPilot()
        {
            var middle = (BorderSetup.BottomLeftCorner.x + BorderSetup.TopRightCorner.x) / 2;
            StartCoroutine(MovingCharacter(transform.position.x <= middle));
        }

        private IEnumerator MovingCharacter(bool left)
        {
            if (!_isLanding) yield break;
            _isGoing = true;

            var direction = left ? Vector2.left : Vector2.right;

            while (_isGoing)
            {
                transform.Translate(direction * Time.deltaTime * moveSpeed);
                yield return null;
            }
        }

        private void OnDisable()
        {
            if (c_Run != null)
            {
                StopCoroutine(c_Run);
                c_Run = null;
            }

            StopAllCoroutines();
        }
    }
}