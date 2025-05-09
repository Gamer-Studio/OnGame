using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OnGame
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] GameObject mainMenuPanel;
        [SerializeField] GameObject modeSelectPanel;
        [SerializeField] GameObject settingsPanel;
        
        [Header("MainMenu Components")]
        [SerializeField] Button startButton;
        [SerializeField] Button modeSelectButton;
        [SerializeField] Button settingsButton;
        
        [Header("ModeSelect Components")]
        [SerializeField] Button classicModeButton;
        [SerializeField] Button infinityModeButton;
        
        [SerializeField] Button[] backtomainButtons;
        
        private void Start()
        {
            BackToMain();
        }
        //메인 화면
        private void ShowPanel(GameObject panelToShow)
        {
            mainMenuPanel.SetActive(false);
            modeSelectPanel.SetActive(false);
            settingsPanel.SetActive(false);

            panelToShow.SetActive(true);
        }
        public void OnClickedStart()
        {
            Debug.Log("게임 시작");
        }
        public void OnClickedModeSelect()
        {
            ShowPanel(modeSelectPanel);
        }
        public void OnClickedSettings()
        {
            ShowPanel(settingsPanel);
        }
        public void BackToMain()
        {
            ShowPanel(mainMenuPanel);
        }
        //모드 선택 화면
        public void OnClickedClassicMode()
        {
            Debug.Log("스토리 모드 선택");
        }
        public void OnClickedInfinityMode()
        {
            Debug.Log("무한 모드 선택");
        }
    }
}
