using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TreasureChest
{
    public class Chest : MonoBehaviour
    {
        public CardUI cardUI;
        public AudioClip ChestOpenedSound;
        public AudioClip ChestOpenedEvilSound;
        [SerializeField] private GameObject questionPopupOpenerPrefab;
        [SerializeField] private Transform canvasTransform;
        private Animator m_ChestAnimator;
        public Animator ChestAnimator { get { return m_ChestAnimator; } }

        [SerializeField]
        private int m_ChestLevel;
        public int ChestLevel { get { return m_ChestLevel; } }

        [SerializeField]
        private GameObject ChestOpenVFX;
        [SerializeField]
        private GameObject ChestOpenedIdelVFX;

        public UnityEvent<GameObject> OnChestOpen;
        public UnityEvent<GameObject> OnChestOpenedIdle;
        public UnityEvent<GameObject> OnChestClose;

        private void Awake()
        {


            m_ChestAnimator = GetComponent<Animator>();
            ChestOpenVFX = transform.Find("ChestOpen_VFX").gameObject;
            ChestOpenedIdelVFX = transform.Find("Opened_Idel_VFX").gameObject;
        }


        public void Open()
        {
            m_ChestAnimator = GetComponent<Animator>();

            m_ChestAnimator.SetTrigger("Open");
        }

        public void Close()
        {
            m_ChestAnimator = GetComponent<Animator>();

            m_ChestAnimator.SetTrigger("Close");

        }

        protected void OnOpen()
        {
            ChestOpenVFX.SetActive(true);
            ChestOpenedIdelVFX.SetActive(true);
            OnChestOpen?.Invoke(gameObject);
            Debug.Log("OnOpen");
        }

        protected void OnOpenedIdle()
        {
            ChestOpenVFX.SetActive(false);
            OnChestOpenedIdle?.Invoke(gameObject);
            Debug.Log("OnOpenedIdle");

        }

        protected void OnClose()
        {
            ChestOpenVFX.SetActive(false);
            ChestOpenedIdelVFX.SetActive(false);
            OnChestClose?.Invoke(gameObject);
            Debug.Log("OnClose");
        }

        public void OpenPopUp()
        {
            if (cardUI != null)
            {
                cardUI.PopupOpened();
            }
            else
            {
                Debug.LogWarning("CardUI referansı atanmadı!");
            }
        }

        public void OpenChestSound(){
            GetComponent<AudioSource>().clip = ChestOpenedSound;
            GetComponent<AudioSource>().Play();
        }

        public void OpenChestEvilSound(){
            GetComponent<AudioSource>().clip = ChestOpenedEvilSound;
            GetComponent<AudioSource>().Play();
        }


    }


}
