using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateSequence : MonoBehaviour
{
    public Text squenceTextBox;
    [SerializeField] uiHandler uiHandler;
    [SerializeField] AlgorithmSelect algorithmSelect;
    int sequenceCount = 8, inGameSquenceTextBoxIndexToReduce=0;
    public void Createsequence(GameObject _this)
    {
        squenceTextBox.text += _this.transform.GetChild(0).GetComponent<Text>().text + ' ';
        Destroy(_this);

        if (algorithmSelect.selectedAlgo == "SCAN" || algorithmSelect.selectedAlgo == "C-SCAN" || algorithmSelect.selectedAlgo == "LOOK" || algorithmSelect.selectedAlgo == "C-LOOK")
        {
            inGameSquenceTextBoxIndexToReduce = 2;
            sequenceCount = 6;
        }

        if (uiHandler.squenceCounter == sequenceCount)
        {
            string[] trimmedStr = new string[9];
            trimmedStr = squenceTextBox.text.Split(' ');

            for (int i = 0; i < uiHandler.inGameSquenceTextBox.Length-inGameSquenceTextBoxIndexToReduce; i++)
            {
                uiHandler.inGameSquenceTextBox[i].text += trimmedStr[i];
            }

            uiHandler.inGameCanvas.SetActive(true);
            uiHandler.inGamePanel.SetActive(true);
            Destroy(uiHandler.panel2);
        }
        uiHandler.squenceCounter++;
    }
}
