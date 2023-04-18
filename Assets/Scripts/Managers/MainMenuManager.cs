using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainTab, RecordsTab;

    private GameObject m_currentTab;

    private void Awake()
    {
        m_currentTab = MainTab;
    }

    public void QuitGame() => Application.Quit();

    public void NewGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void OpenTab(GameObject tabToOpen)
    {
        tabToOpen?.SetActive(true);
        m_currentTab = tabToOpen;
    }

    public void CloseTab(GameObject tabToClose) => tabToClose?.SetActive(false);

    public void RecordsButton()
    {
        OpenTab(RecordsTab);
        CloseTab(MainTab);
    }

    public void BackButton()
    {
        CloseTab(m_currentTab);
        OpenTab(MainTab);
    }
}
