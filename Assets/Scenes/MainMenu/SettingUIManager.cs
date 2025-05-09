using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OnGame
{
    public class SettingUIManager : MonoBehaviour
    {
        [Header("Setting Components")]
        [SerializeField] Dropdown resolution;
        [SerializeField] Toggle fullscreen;
        [SerializeField] Toggle windowscreen;
        [SerializeField] Slider masterVolume;
        [SerializeField] Slider backgroundVolume;
        [SerializeField] Slider effectVolume;
        
        private Resolution[] resolutions;

        public void Start()
        {
            InitializeResolution();
            resolution.onValueChanged.AddListener(OnResolution);
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
        
        //설정 화면
        public void OnResolution(int index)
        {
            if (resolutions == null || resolutions.Length == 0) return;

            Resolution selectedResolution = resolutions[index];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
            Debug.Log($"해상도 변경: {selectedResolution.width} x {selectedResolution.height}");
        }
        public void OnFullscreen(bool isOn)
        {
            Screen.fullScreen = isOn;
            fullscreen.interactable = false;
            windowscreen.interactable = true;
            Debug.Log("전체화면");
         
        }

        public void OnWindowScreen(bool isOn)
        {
            Screen.fullScreen = !isOn;
            windowscreen.interactable = false;
            fullscreen.interactable = true;
            Debug.Log("창모드");
        }

        public void OnMasterVolume(float value)
        {
            Debug.Log($"마스터 볼륨: {value:F2}");
        }

        public void OnBackgroundVolume(float value)
        {
            Debug.Log($"배경음 볼륨: {value:F2}");
        }

        public void OnEffectVolume(float value)
        {
            Debug.Log($"효과음 볼륨: {value:F2}");
        }
    }
}
