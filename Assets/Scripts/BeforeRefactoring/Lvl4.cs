using UnityEngine;

namespace BeforeRefactoring
{
    public class Lvl4 : MonoBehaviour {

        public GameObject[] checkPOint;
        public static int cieck;
        private int clickSpase;
        bool playerLoose;

        void Start()
        {
            clickSpase = 0;
            cieck = 3;
            //Invoke("StartUI", 1f);
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    clickSpase += 1;
            //    StartUI();
            //}
            //if (cieck == 0 && !playerLoose) GameScript.lvlWin = true;
            //if (playerLoose) GameScript.lvlLose = true;
            if (GameObject.FindGameObjectWithTag("Player") == null && GameObject.FindGameObjectWithTag("Characted1") == null && GameObject.FindGameObjectWithTag("Characted2") == null) playerLoose = true;
        }

        //void StartUI()
        //{
        //    GameScript.pauseOn = true;
        //    if (clickSpase == 0)
        //    {
        //        GameScript.pauseOn = true;
        //        checkPOint[0].SetActive(true);
        //        checkPOint[1].SetActive(false);
        //        checkPOint[2].SetActive(false);

        //    }
        //    if (clickSpase == 1)
        //    {
        //        GameScript.pauseOn = true;
        //        checkPOint[0].SetActive(false);
        //        checkPOint[1].SetActive(true);
        //        checkPOint[2].SetActive(false);
        //    }
        //    if (clickSpase == 2)
        //    {
        //        GameScript.pauseOn = true;
        //        checkPOint[0].SetActive(false);
        //        checkPOint[1].SetActive(false);
        //        checkPOint[2].SetActive(true);
        //    }
        //    if (clickSpase >= 3)
        //    {
        //        GameScript.pauseOn = false;
        //        checkPOint[0].SetActive(false);
        //        checkPOint[1].SetActive(false);
        //        checkPOint[2].SetActive(false);
        //    }


        //}
    }
}
