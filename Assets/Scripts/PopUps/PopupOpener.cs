// Copyright (C) 2015 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;


    // This class is responsible for creating and opening a popup of the given prefab and add
    // it to the UI canvas of the current scene.
    public class PopupOpener : MonoBehaviour
    {
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject özelYetenekPaneli;
        [SerializeField] private GameObject soruPaneli;
        [SerializeField] private GameObject sürePaneli;


        public GameObject popupPrefab;

        protected Canvas m_canvas;

        protected void Start()
        {
            m_canvas = GetComponentInParent<Canvas>();
        }

        public virtual void OpenPopup()
        {
            var popup = Instantiate(popupPrefab) as GameObject;
            popup.SetActive(true);
            popup.transform.localScale = Vector3.zero;
            popup.transform.SetParent(m_canvas.transform, false);
            popup.GetComponent<Popup>().Open();
        }

        public virtual void OpenSettingsPopup()
        {
            settingsPanel.SetActive(true);
            settingsPanel.transform.localScale = Vector3.zero;
            settingsPanel.transform.SetParent(m_canvas.transform, false);
            settingsPanel.GetComponent<Popup>().Open();
        }
        public virtual void OpenTimePopup()
        {
            sürePaneli.SetActive(true);
            sürePaneli.transform.localScale = Vector3.zero;
            sürePaneli.transform.SetParent(m_canvas.transform, false);
            sürePaneli.GetComponent<Popup>().Open();
        }
        public virtual void OpenAbilityPopup()
        {
            özelYetenekPaneli.SetActive(true);
            özelYetenekPaneli.transform.localScale = Vector3.zero;
            özelYetenekPaneli.transform.SetParent(m_canvas.transform, false);
            özelYetenekPaneli.GetComponent<Popup>().Open();
        }
        public virtual void OpenQuestionSettingsPopup()
        {
            soruPaneli.SetActive(true);
            soruPaneli.transform.localScale = Vector3.zero;
            soruPaneli.transform.SetParent(m_canvas.transform, false);
            soruPaneli.GetComponent<Popup>().Open();
        }
    }
