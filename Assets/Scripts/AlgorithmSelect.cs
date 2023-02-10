using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlgorithmSelect : MonoBehaviour
{
    public string selectedAlgo;
    [SerializeField] uiHandler uiHandler;
    [SerializeField] LineManager lineManager;
    public void GetAlgo()
    {
        selectedAlgo = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
        uiHandler.customizePanel.SetActive(true);

        if (selectedAlgo == "SCAN" || selectedAlgo == "C-SCAN" || selectedAlgo == "LOOK")
        {
            uiHandler.scanAlgopanel.SetActive(true);

            uiHandler.panel2.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(false);
            uiHandler.panel2.transform.GetChild(3).transform.GetChild(9).gameObject.SetActive(false);

            uiHandler.panel1.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);
            uiHandler.panel1.transform.GetChild(1).transform.GetChild(9).gameObject.SetActive(false);          
        }
        else
            uiHandler.panel1.SetActive(true);

        if (selectedAlgo == "FCFS")
        {
            uiHandler.inGamePanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(lineManager.FCFS);
            uiHandler.headerTxt.GetComponent<Text>().text = "FCFS (First-Come First-Server) Algorithm";
        }
        else if (selectedAlgo == "SSTF")
        {
            uiHandler.inGamePanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(lineManager.SSTF);
            uiHandler.headerTxt.GetComponent<Text>().text = "SSTF (Shortest Seek Time First) Algorithm";

        }
        else if (selectedAlgo == "SCAN")
        {
            uiHandler.inGamePanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate {lineManager.SCAN();});
            uiHandler.headerTxt.GetComponent<Text>().text = "SCAN (Elevator) Algorithm";
        }
        else if (selectedAlgo == "C-SCAN")
        {
           uiHandler.inGamePanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate {lineManager.C_SCAN();});
            uiHandler.headerTxt.GetComponent<Text>().text = "C-SCAN Algorithm";
        }
        else if (selectedAlgo == "LOOK")
        {
            uiHandler.inGamePanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate {lineManager.Look(); });
            uiHandler.headerTxt.GetComponent<Text>().text = "LOOK Algorithm";
        }

        uiHandler.AlgorithmSelectPanel.SetActive(false);
    }

    public void Print()
    {
        Debug.Log(selectedAlgo);        
    }
}
