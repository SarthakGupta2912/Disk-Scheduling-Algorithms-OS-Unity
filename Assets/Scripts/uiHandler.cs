using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiHandler : MonoBehaviour
{
    [SerializeField] GameObject StartPanel, customizePanel;
    [SerializeField] LineManager lineManager;
    [SerializeField] Text headPositionText;
    public Text[] inGameSquenceTextBox = new Text[9];

    public GameObject inGamePanel, inGameCanvas, panel1, panel2, GameOverPanel;
    public int squenceCounter = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        StartPanel.SetActive(true);
        customizePanel.SetActive(false);

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3);
        Destroy(StartPanel.gameObject);
        customizePanel.SetActive(true);
    }

    public void DestroyPanel1()
    {
        Destroy(panel1);
    }

    public void GetHeadPosition(GameObject _this)
    {
        lineManager.headPosition = int.Parse(_this.transform.GetChild(0).GetComponent<Text>().text);
        headPositionText.text += lineManager.headPosition.ToString();

        // Remove head position from the sequence
        for (int i = 0; i < panel2.transform.GetChild(3).childCount; i++)
        {
            if (lineManager.headPosition.ToString() == panel2.transform.GetChild(3).GetChild(i).GetChild(0).GetComponent<Text>().text)
                panel2.transform.GetChild(3).GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}

