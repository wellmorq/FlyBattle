using UnityEngine;

namespace BeforeRefactoring
{
    public class Lvl3 : MonoBehaviour {

        public GameObject[] checkPOint;
        public static int cieck;
        private int clickSpase;

        void Start()
        {
            clickSpase = 0;
            cieck = 1;
            Invoke("StartUI", 1f);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                clickSpase += 1;
                StartUI();
            }
            //if (cieck == 0) GameScript.lvlWin = true;
        }

        void StartUI()
        {
            //GameScript.pauseOn = true;
            if (clickSpase == 0)
            {
                //GameScript.pauseOn = true;
                checkPOint[0].SetActive(true);
                checkPOint[1].SetActive(false);
                checkPOint[2].SetActive(false);

            }
            if (clickSpase == 1)
            {
                //GameScript.pauseOn = true;
                checkPOint[0].SetActive(false);
                checkPOint[1].SetActive(true);
                checkPOint[2].SetActive(false);
                //Player1.lives -= 1;
            }
            if (clickSpase == 2)
            {
                //GameScript.pauseOn = true;
                checkPOint[0].SetActive(false);
                checkPOint[1].SetActive(false);
                checkPOint[2].SetActive(true);
                //Player1.lives -= 1;
            }
            if (clickSpase >= 3)
            {
                //GameScript.pauseOn = false;
                checkPOint[0].SetActive(false);
                checkPOint[1].SetActive(false);
                checkPOint[2].SetActive(false);
                //Player1.lives -= 1;
            }


        }
    }
}
