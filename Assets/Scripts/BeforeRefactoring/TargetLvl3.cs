using UnityEngine;

namespace BeforeRefactoring
{
	public class TargetLvl3 : MonoBehaviour {

		public GameObject end;

		void Start () {
		
		}
	
		void Update () {
		
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.tag == "Bullet")
			{
				Lvl3.cieck -= 1;
				GameObject newEnd = Instantiate(end, new Vector2(transform.position.x, transform.position.y), Quaternion.FromToRotation(transform.position, gameObject.transform.position)) as GameObject;
				Destroy(this.gameObject);
			}
		}
	}
}
