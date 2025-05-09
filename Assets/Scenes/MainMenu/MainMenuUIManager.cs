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
        
        [Header("Setting Components")]
        [SerializeField] Dropdown resolution;
        [SerializeField] Toggle fullscreen;
        [SerializeField] Toggle windowscreen;
        [SerializeField] Slider masterVolume;
        [SerializeField] Slider backgroundVolume;
        [SerializeField] Slider effectVolume;
        
        [SerializeField] Button[] backtomainButtons;
        
        private Resolution[] resolutions;
        private void Start()
        {
            SetupMainMenu();
            SetupModeSelect();
            SetupSettings();
            //뒤로가기
            foreach (Button btn in backtomainButtons)
            {
                btn.onClick.AddListener(BackToMain);
            }
        }
        
        private void SetupMainMenu() //메인메뉴
        {
            startButton.onClick.AddListener(OnClickedStart);
            modeSelectButton.onClick.AddListener(OnClickedModeSelect);
            settingsButton.onClick.AddListener(OnClickedSettings);
        }

        private void SetupModeSelect() //모드선택
        {
            classicModeButton.onClick.AddListener(OnClickedClassicMode);
            infinityModeButton.onClick.AddListener(OnClickedInfinityMode);
        }

        private void SetupSettings() //설정
        {
            InitializeResolution();
            
            resolution.onValueChanged.AddListener(OnResolution);
            fullscreen.onValueChanged.AddListener(OnFullscreen);
            windowscreen.onValueChanged.AddListener(OnWindowScreen);
            masterVolume.onValueChanged.AddListener(OnMasterVolume);
            backgroundVolume.onValueChanged.AddListener(OnBackgroundVolume);
            effectVolume.onValueChanged.AddListener(OnEffectVolume);
        }
        
        private void InitializeResolution()//가능한 해상도 불러오기
        {
            resolutions = Screen.resolutions;
            resolution.ClearOptions();

            List<string> options = new List<string>();
            int curResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = $"{resolutions[i].width} x {resolutions[i].height}";
                if (!options.Contains(option)) //중복 제거
                {
                    options.Add(option);
                }

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    curResolutionIndex = i;
                }
            }

            resolution.AddOptions(options);
            resolution.value = curResolutionIndex;
            resolution.RefreshShownValue();
        }
        
        //메인 화면
        private void OnClickedStart()
        {
            Debug.Log("게임 시작");
        }
        private void OnClickedModeSelect()
        {
            ShowPanel(modeSelectPanel);
        }
        private void OnClickedSettings()
        {
            ShowPanel(settingsPanel);
        }
        private void BackToMain()
        {
            ShowPanel(mainMenuPanel);
        }
        private void ShowPanel(GameObject panelToShow)
        {
            mainMenuPanel.SetActive(false);
            modeSelectPanel.SetActive(false);
            settingsPanel.SetActive(false);

            panelToShow.SetActive(true);
        }
        //모드 선택 화면
        private void OnClickedClassicMode()
        {
            Debug.Log("스토리 모드 선택");
        }
        private void OnClickedInfinityMode()
        {
            Debug.Log("무한 모드 선택");
        }
        
        //설정 화면
        private void OnResolution(int index)
        {
            if (resolutions == null || resolutions.Length == 0) return;

            Resolution selectedResolution = resolutions[index];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
            Debug.Log($"해상도 변경: {selectedResolution.width} x {selectedResolution.height}");
        }
        private void OnFullscreen(bool isOn)
        {
            Debug.Log("전체화면");
            Screen.fullScreen = isOn;
        }
        private void OnWindowScreen(bool isOn)
        {
            Debug.Log("창모드");
            Screen.fullScreen = !isOn;
        }
        private void OnMasterVolume(float value)
        {
            Debug.Log($"마스터 볼륨: {value:F2}");
        }
        private void OnBackgroundVolume(float value)
        {
            Debug.Log($"배경음 볼륨: {value:F2}");
        }
        private void OnEffectVolume(float value)
        {
            Debug.Log($"효과음 볼륨: {value:F2}");
        }
    }
}
