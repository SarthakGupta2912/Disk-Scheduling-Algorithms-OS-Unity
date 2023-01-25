using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineManager : MonoBehaviour
{
    // Line related components
    [SerializeField] GameObject[] requestsTxt;
    public int headPosition = 50;
    int requestsProcessed = 0, lineRendPosition = 1, pos_y = 0, pos_x = 0, requestArrayElement = 0;
    int[] requestArray = new int[9];

    LineRenderer lineRenderer;

    // UI related components
    [SerializeField] uiHandler uiHandler;
    [SerializeField] GameObject button;
    [SerializeField] GameObject imageHighlighter;
    int textBoxSelect = 0, tempHeadStore;
    // Start is called before the first frame update
    void Start()
    {
        string[] trimmedStr = new string[9];

        for (int i = 0; i < uiHandler.inGameSquenceTextBox.Length; i++)
            trimmedStr[i] = uiHandler.inGameSquenceTextBox[i].text;

        for (int i = 0; i < trimmedStr.Length; i++)
            requestArray[i] = int.Parse(trimmedStr[i]);

        lineRenderer = this.GetComponent<LineRenderer>();
        tempHeadStore = headPosition;
        StartPosition();
    }

    // ---------------------------------------------- HeadPosition ---------------------------------------------- \\
    public void StartPosition()
    {
        for (int i = 0; i < requestsTxt.Length; i++)
        {
            if (requestsTxt[i].GetComponent<Text>().text == headPosition.ToString())
            {
                lineRenderer.transform.localPosition = new Vector3(requestsTxt[i].transform.localPosition.x, lineRenderer.transform.localPosition.y, 0);
                headPosition = i;
                break;
            }
        }
    }

    // ---------------------------------------------- Algorithms ---------------------------------------------- \\
    public void FCFS()
    {
        //Check if all the requests are processed
        if (requestsProcessed <= requestsTxt.Length - 2)
        {
            imageHighlighter.SetActive(true);
            imageHighlighter.transform.localPosition = uiHandler.inGameSquenceTextBox[textBoxSelect].transform.localPosition;
            textBoxSelect++;

            if (requestsProcessed == requestsTxt.Length - 2)
            {
                Destroy(button);
                SeekTime();
            }

            for (int i = 0; i <= (requestsTxt.Length - 1); i++)
            {
                // check if the element matches the request array element value
                if (requestArray[requestArrayElement].ToString() == requestsTxt[i].GetComponent<Text>().text)
                {
                    //distance from the current position to the required position
                    if (headPosition > i)
                    {
                        //calculate number of elements to move to the right
                        pos_y += (3 * (headPosition - i));
                        pos_x -= 1;

                        lineRenderer.SetPosition(lineRendPosition, new Vector3(pos_x, pos_y, 0));
                    }

                    else
                    {
                        //calculate number of elements to move to the left
                        pos_y -= (3 * (i - headPosition));
                        pos_x -= 1;

                        lineRenderer.SetPosition(lineRendPosition, new Vector3(pos_x, pos_y, 0));
                    }
                    headPosition = i;
                }
            }
            lineRendPosition++;
            requestArrayElement++;
            requestsProcessed++;
            if (requestsProcessed <= (requestsTxt.Length - 2))
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRendPosition, new Vector3(pos_x, pos_y, 0));
            }
        }

        else
            Debug.Log("FCFS algorithm completed successfully!");
    }

    public void SeekTime()
    {
        int temp = 0;
        uiHandler.inGamePanel.transform.GetChild(2).gameObject.SetActive(true);

        if (tempHeadStore > requestArray[0])
        {
            temp += tempHeadStore - requestArray[0];
            uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + tempHeadStore + "-" + requestArray[0] + ")+";
        }
        else
        {
            temp += requestArray[0] - tempHeadStore;
            uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestArray[0] + "-" + tempHeadStore + ")+";
        }

        for (int i = 0; i < requestArray.Length - 1; i++)
        {
            if (requestArray[i] > requestArray[i + 1])
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestArray[i] + "-" + requestArray[i + 1] + ")";

            else
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestArray[i + 1] + "-" + requestArray[i] + ")";

            if (i < requestArray.Length - 2)
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";

        }

        uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "\n=\n";
        uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += temp+"+";

        for (int i = 0; i < requestArray.Length - 1; i++)
        {
            if (requestArray[i] > requestArray[i + 1])
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += requestArray[i] - requestArray[i + 1];

            else
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += requestArray[i + 1] - requestArray[i];

            if (i < requestArray.Length - 2)
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";

        }

        for (int i = 0; i < requestArray.Length - 1; i++)
        {
            if (requestArray[i] > requestArray[i + 1])
                temp += requestArray[i] - requestArray[i + 1];

            else
                temp += requestArray[i + 1] - requestArray[i];
        }
        uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "\n=\nThe seek time for your algorithm is: " + temp;
    }
}
