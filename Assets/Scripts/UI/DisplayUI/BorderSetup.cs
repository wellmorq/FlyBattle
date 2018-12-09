using UnityEngine;

namespace FlyBattle.UI
{
    public class BorderSetup : MonoBehaviour
    {
        public static Vector2 BottomLeftCorner;
        public static Vector2 TopRightCorner;
        [SerializeField] private GameObject ceiling, rightWall, leftwall;


        private void Awake()
        {
            var camera = GetComponent<Camera>();
            BottomLeftCorner = camera.ScreenToWorldPoint(new Vector2(0, 0));
            TopRightCorner = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            ceiling.transform.position = new Vector3(0, Mathf.Ceil(TopRightCorner.y), 0);
            leftwall.transform.position = new Vector3(BottomLeftCorner.x - 0.5f, 0, 0);
            rightWall.transform.position = new Vector3(TopRightCorner.x + 0.5f, 0, 0);
        }
    }
}