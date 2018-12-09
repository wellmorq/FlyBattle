using UnityEngine;

namespace BeforeRefactoring
{
    public class CheckPoint : MonoBehaviour {

        public GameObject point;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                //point.Spawn(transform.position, Quaternion.Euler(0, 0, 0));
                GameObject newPoint = Instantiate(point, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0,0,0)) as GameObject;
                Destroy(this.gameObject);
            }
        }
    }
}
