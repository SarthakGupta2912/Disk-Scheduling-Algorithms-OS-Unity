using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class uiHandler : MonoBehaviour
{
    [SerializeField] GameObject StartPanel;
    [SerializeField] LineManager lineManager;
    [SerializeField] Text headPositionText;
    public Text[] inGameSquenceTextBox = new Text[9];

    public GameObject inGamePanel, inGameCanvas, panel1, panel2, GameOverPanel, scanAlgopanel, AlgorithmSelectPanel, customizePanel, headerTxt;
    public int squenceCounter = 0;
    public string _chooseWhichDirSCAN;
    // Start is called before the first frame update
    void Start()
    {
        StartPanel.SetActive(true);
        //customizePanel.SetActive(false);

        StartCoroutine(StartGame());
        //GenerateRandomsNumberWithoutRepeat(10);
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3);
        Destroy(StartPanel.gameObject);
        AlgorithmSelectPanel.SetActive(true);
    }

    public void returnButtonName()
    {
        _chooseWhichDirSCAN = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text.ToLower();
    }

    //Generate random number without repeat
    /*public void GenerateRandomsNumberWithoutRepeat(int size)
    {
        int[] temp = new int[size];
        for (int i = 0; i < size; i++)
        {
            temp[i] = Random.Range(1, size+1);
            for (int j = 1; j <= i; j++)
            {
                while(temp[i] == temp[j-1])
                {
                    temp[i] = Random.Range(1, size+1);
                    j = 1;
                }
            }
        }
        for (int i = 0; i < size; i++)
            Debug.Log(temp[i]);
    }*/
    /*public void DestroyPanel1()
    {
        Destroy(panel1);
    }*/

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

