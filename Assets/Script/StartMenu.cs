using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject setting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonOnClick()
    {
        levelSelect.SetActive(true);
    }

    public void QuitButtonOnClick()
    {
        Application.Quit();
    }

    public void SettingButtonOnClick()
    {
        setting.SetActive(true);
    }

    public void ExitLevelSelectButtonOnClick()
    {
        levelSelect.SetActive(false);
        setting.SetActive(false);
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        loadScene();
    }

    void loadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void SetVolume(float sliderVolume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderVolume) * 20);
    }

    public void LevelOnClick(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void CableCutLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void StripOuterLevel()
    {
        SceneManager.LoadScene(3);
    }


    public void CrimpTerLevel()
    {
        SceneManager.LoadScene(5);
    }


    //public void NametubeInsertLevel()
    //{
    //    SceneManager.LoadScene(4);
    //}

    //public void TinningLevel()
    //{
    //    SceneManager.LoadScene(5);
    //}

    //public void CableAssyLevel()
    //{
    //    SceneManager.LoadScene(6);
    //}
}
