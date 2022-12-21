using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    enum leaveSceneIndex {Restart, MainMenu};
    leaveSceneIndex currentLeaveIndex;
    static UIManager instance;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private GameObject completePanel;
    [SerializeField] private GameObject manualActionPanel, machineActionPanel, machineBoardPanel;
    [SerializeField] private Text errorCountText;
    [SerializeField] private Text timeCountTextObj;
    [SerializeField] private GameObject pauseMenu, leaveConfirmMenu, videoGuideMenu, hintMenu;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public static IEnumerator Error()
    {
        instance.errorPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        instance.errorPanel.SetActive(false);
    }

    public static void StartCompletePanel()
    {
        instance.completePanel.SetActive(true);
    }

    public static void SetError(string errorText)
    {
        instance.errorCountText.text = errorText;
    }

    public static void SetTime(string timeText)
    {
        instance.timeCountTextObj.text = timeText;
    }

    public static void ChangeMachine()
    {
        instance.manualActionPanel.SetActive(false);
        instance.machineActionPanel.SetActive(true);
    }

    public static void ChangeManual()
    {
        instance.machineActionPanel.SetActive(false);
        instance.manualActionPanel.SetActive(true);
    }

    public static void ChangeUIText(Text UIObject,string targetText)
    {
        //Text currentText = UIObject.GetComponent<Text>();
        UIObject.text = targetText;
    }

    public void PauseGame()
    {
        SoundManager.PlayClickClip();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        SoundManager.PlayClickClip();
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void OpenRestartWarning()
    {
        SoundManager.PlayClickClip();
        leaveConfirmMenu.SetActive(true);
        currentLeaveIndex = leaveSceneIndex.Restart;

    }

    public void OpenMainMenuWarning()
    {
        SoundManager.PlayClickClip();
        leaveConfirmMenu.SetActive(true);
        currentLeaveIndex = leaveSceneIndex.MainMenu;
    }

    public void CloseWarning()
    {
        leaveConfirmMenu.SetActive(false);
        ResumeGame();
    }

    public void ConfirmLeave()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        if (currentLeaveIndex == leaveSceneIndex.Restart) 
        {
            RestartLevel();
        }
        else if(currentLeaveIndex== leaveSceneIndex.MainMenu)
        {
            DirectBackMainMenu();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DirectBackMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void TutorialMenu()
    {
        SoundManager.PlayClickClip();
        videoGuideMenu.SetActive(true);
    }

    public void CloseTutorialMenu()
    {
        SoundManager.PlayClickClip();
        videoGuideMenu.SetActive(false);
    }

    public static void BoardPanel()
    {
        SoundManager.PlayClickClip();
        instance.machineBoardPanel.SetActive(true);
    }

    public static void CloseBoardPanel()
    {
        SoundManager.PlayClickClip();
        instance.machineBoardPanel.SetActive(false);
    }

    public void HintPanel()
    {
        SoundManager.PlayClickClip();
        hintMenu.SetActive(true);
    }

    public void CloseHintPanel()
    {
        SoundManager.PlayClickClip();
        hintMenu.SetActive(false);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
