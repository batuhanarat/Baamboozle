using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreasureChest
{
    public class ChestDemoSceneManager : MonoBehaviour
    {
        public GameObject ChestLv1;
        public GameObject ChestLv3;

        void Start()
        {
            Invoke("OpenChest1", 0.01f);
            Invoke("OpenChest3", 0.01f);

        }

        public void OpenChest1()
        {
            ChestLv1.GetComponent<Chest>().Open();
        }

        public void OpenChest3()
        {
            ChestLv3.GetComponent<Chest>().Open();
        }

        public void Close()
        {
            ChestLv1.GetComponent<Chest>().Close();
            ChestLv3.GetComponent<Chest>().Close();
        }


        public void OnChestOpen(GameObject Chest)
        {
            Debug.Log("OnChestOpen");
            Debug.Log(Chest.GetComponent<Chest>().ChestLevel);
        }

        public void OnChestOpenedIdle(GameObject Chest)
        {
            Debug.Log("OnChestOpenedIdle");
            Debug.Log(Chest.GetComponent<Chest>().ChestLevel);
        }

        public void OnChestClosed(GameObject Chest)
        {
            Debug.Log("OnChestClosed");
            Debug.Log(Chest.GetComponent<Chest>().ChestLevel);
        }
    }

}
