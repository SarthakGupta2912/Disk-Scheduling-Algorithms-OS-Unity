using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateSequence : MonoBehaviour
{
    public Text squenceTextBox;
    [SerializeField] uiHandler uiHandler;
    public void Createsequence(GameObject _this)
    {
        squenceTextBox.text += _this.transform.GetChild(0).GetComponent<Text>().text + ' ';
        Destroy(_this);

        if (uiHandler.squenceCounter == 6)
        {
            string[] trimmedStr = new string[9];
            trimmedStr = squenceTextBox.text.Split(' ');

            for (int i = 0; i < uiHandler.inGameSquenceTextBox.Length-2; i++)
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
