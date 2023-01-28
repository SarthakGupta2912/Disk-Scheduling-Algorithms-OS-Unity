using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineManager : MonoBehaviour
{
    // Line related components
    string tempSCANString;
    [SerializeField] GameObject[] requestsTxt;
    public int headPosition = 50;
    int requestsProcessed = 0, lineRendPosition = 1, pos_y = 0, pos_x = 0, requestArrayElement = 0;
    int[] requestArray = new int[7];
    LineRenderer lineRenderer;

    // UI related components
    [SerializeField] uiHandler uiHandler;
    [SerializeField] GameObject button;
    [SerializeField] GameObject imageHighlighter;
    int textBoxSelect = 0, tempHeadValueStore, tempHeadElementStore;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < requestArray.Length; i++)
            requestArray[i] = int.Parse(uiHandler.inGameSquenceTextBox[i].text);

        lineRenderer = this.GetComponent<LineRenderer>();
        tempHeadValueStore = headPosition;

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
                tempHeadElementStore = i;
                break;
            }
        }
        sortRequestArrayInAscendingOrder();
    }

    // ---------------------------------------------- Algorithms ---------------------------------------------- \\

    // FCFS Algorithm
    public void FCFS()
    {
        //Check if all the requests are processed
        if (requestsProcessed <= requestsTxt.Length - 2)
        {
            //moving request highlighter
            imageHighlighter.SetActive(true);
            imageHighlighter.transform.localPosition = uiHandler.inGameSquenceTextBox[textBoxSelect].transform.localPosition;
            textBoxSelect++;

            // destroy the click here button so that user can not press it again and then call seek time fcfs algorithm
            if (requestsProcessed == requestsTxt.Length - 2)
            {
                Destroy(button);
                SeekTimeFCFS();
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
                        MoveRight(i);
                        break;
                    }

                    else
                    {
                        //calculate number of elements to move to the left
                        MoveLeft(i);
                        break;
                    }
                }
            }
        }

        else
            Debug.Log("FCFS algorithm completed successfully!");
    }

    // Seek time calculation for FCFS
    public void SeekTimeFCFS()
    {
        int temp = 0;
        uiHandler.inGamePanel.transform.GetChild(2).gameObject.SetActive(true);

        if (tempHeadValueStore > requestArray[0])
        {
            temp += tempHeadValueStore - requestArray[0];
            uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + tempHeadValueStore + "-" + requestArray[0] + ")+";
        }
        else
        {
            temp += requestArray[0] - tempHeadValueStore;
            uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestArray[0] + "-" + tempHeadValueStore + ")+";
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
        uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += temp + "+";

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

    // SSTF Algorithm
    public void SSTF()
    {
        //Check if all the elements or requests are processed or not
        if (requestsProcessed <= requestsTxt.Length - 2)
        {
            if (requestsProcessed == requestsTxt.Length - 2)
                Destroy(button);
            // move towards right only, headposition = 0 or at 1st element
            if (tempHeadElementStore == 0)
            {
                for (int i = 0; i <= (requestsTxt.Length - 1); i++)
                {
                    // check if the element matches the request array element value
                    if (requestArray[requestArrayElement].ToString() == requestsTxt[i].GetComponent<Text>().text)
                    {
                        ImageHighlighterMove(requestArrayElement);
                        MoveRight(i);
                        break;
                    }
                }

            }

            // move towards left only, headposition = 9 or at 10th element
            else if (tempHeadElementStore == requestsTxt.Length - 1)
            {
                for (int i = 0; i <= (requestsTxt.Length - 1); i++)
                {
                    // check if the element matches the request array element value
                    if (requestArray[requestArrayElement].ToString() == requestsTxt[i].GetComponent<Text>().text)
                    {
                        ImageHighlighterMove(requestArrayElement);
                        MoveLeft(i);
                        break;
                    }
                }
            }

            // move in left if both the above condions are false and then moving to right in case the tempHeadElement reaches 0
            else
            {
                // Check if distance of right side is greater or left
                // Right side element distance                                                                  // Left side element distance 
                if ((int.Parse(requestsTxt[headPosition + 1].GetComponent<Text>().text) - tempHeadValueStore) >= (tempHeadValueStore - int.Parse(requestsTxt[headPosition - 1].GetComponent<Text>().text)) || headPosition != 0)
                {
                    tempHeadElementStore = headPosition - 1;

                    for (int i = tempHeadElementStore; i >= 0; i--)
                    {
                        // check if the element matches the request array element value
                        if (requestArray[tempHeadElementStore].ToString() == requestsTxt[i].GetComponent<Text>().text)
                        {
                            ImageHighlighterMove(tempHeadElementStore);
                            MoveLeft(i);
                            break;
                        }
                    }
                }
            }
        }
        else
            Debug.Log("SSTF algorithm completed successfully!");
    }

    // Seek time calculation for SSTF
    public void SeekTimeSSTF(int storeBeginHeadPos)
    {
        //uiHandler.inGamePanel.transform.GetChild(2).gameObject.SetActive(true);
        int temp = 0;

        // move towards right only, headposition = 0 or at 1st element
        if (storeBeginHeadPos == 0)
        {
            sortRequestArrayInAscendingOrder();

            for (int i = 0; i <= requestsTxt.Length - 2; i++)
            {
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[i + 1].GetComponent<Text>().text + "-" + requestsTxt[i].GetComponent<Text>().text + ")";
                if (i <= requestsTxt.Length - 3)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
            }

            uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "\n=\n";

            for (int i = 0; i <= requestsTxt.Length - 2; i++)
            {
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[i].GetComponent<Text>().text);
                temp += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[i].GetComponent<Text>().text);
                if (i <= requestsTxt.Length - 3)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
            }
        }

        // move towards left only, headposition = 9 or at 10th element
        else if (storeBeginHeadPos == requestsTxt.Length - 1)
        {
            //sorting element in the request array in descending order
            sortRequestArrayInDescdingOrder();

            for (int i = requestsTxt.Length - 1; i >= 1; i--)
            {
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[i].GetComponent<Text>().text + "-" + requestsTxt[i - 1].GetComponent<Text>().text + ")";
                if (i >= 2)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
            }

            uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "\n=\n";

            for (int i = requestsTxt.Length - 1; i >= 1; i--)
            {
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += int.Parse(requestsTxt[i].GetComponent<Text>().text) - int.Parse(requestsTxt[i - 1].GetComponent<Text>().text);
                temp += int.Parse(requestsTxt[i].GetComponent<Text>().text) - int.Parse(requestsTxt[i - 1].GetComponent<Text>().text);
                if (i >= 2)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
            }
        }

        // move in left if both the above condions are false 
        else
        {
            sortRequestArrayInAscendingOrder();

            for (int i = storeBeginHeadPos; i > 0; i--)
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[i].GetComponent<Text>().text + "-" + requestsTxt[i - 1].GetComponent<Text>().text + ")+";


            for (int i = storeBeginHeadPos; i <= requestsTxt.Length - 2; i++)
            {
                if (i == storeBeginHeadPos)
                {
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[i + 1].GetComponent<Text>().text + "-" + requestsTxt[0].GetComponent<Text>().text + ")";
                }
                else
                {
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[i + 1].GetComponent<Text>().text + "-" + requestsTxt[i].GetComponent<Text>().text + ")";
                }
                if (i <= requestsTxt.Length - 3)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
            }

            uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "\n=\n";

            for (int i = storeBeginHeadPos; i > 0; i--)
            {
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += int.Parse(requestsTxt[i].GetComponent<Text>().text) - int.Parse(requestsTxt[i - 1].GetComponent<Text>().text);
                temp += int.Parse(requestsTxt[i].GetComponent<Text>().text) - int.Parse(requestsTxt[i - 1].GetComponent<Text>().text);
                if (i >= 1)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
            }

            for (int i = storeBeginHeadPos; i <= requestsTxt.Length - 2; i++)
            {
                if (i == storeBeginHeadPos)
                {
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[0].GetComponent<Text>().text);
                    temp += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[0].GetComponent<Text>().text);
                }
                else
                {
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[i].GetComponent<Text>().text);
                    temp += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[i].GetComponent<Text>().text);
                }
                if (i <= requestsTxt.Length - 3)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
            }
        }

        uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "\n=\nThe seek time for your algorithm is: " + temp;
    }

    //SCAN Algorithm
    public void SCAN(string _chooseWhichDirSCAN)
    {
        _chooseWhichDirSCAN = uiHandler._chooseWhichDirSCAN;
        //Check if all the elements or requests are processed or not
        if (requestsProcessed <= requestsTxt.Length - 2)
        {
            if (requestsProcessed == 0)
                SeekTimeSCAN(headPosition, _chooseWhichDirSCAN);
            if (requestsProcessed == requestsTxt.Length - 2)
            {
                Destroy(button);
                uiHandler.inGamePanel.transform.GetChild(2).gameObject.SetActive(true);
            }
            // move towards right only, headposition = 0 or at 1st element
            /*if (tempHeadElementStore == 0 && _chooseWhichDirSCAN == "")
            {
                for (int i = 0; i <= (requestsTxt.Length - 1); i++)
                {
                    // check if the element matches the request array element value
                    if (requestArray[requestArrayElement].ToString() == requestsTxt[i].GetComponent<Text>().text)
                    {
                        ImageHighlighterMove(requestArrayElement);
                        MoveRight(i);
                        break;
                    }
                }

            }*/

            // move towards left only, headposition = 9 or at 10th element
            /* else if (tempHeadElementStore == requestsTxt.Length - 1 && _chooseWhichDirSCAN == "")
             {
                 for (int i = 0; i <= (requestsTxt.Length - 1); i++)
                 {
                     // check if the element matches the request array element value
                     if (requestArray[requestArrayElement].ToString() == requestsTxt[i].GetComponent<Text>().text)
                     {
                         ImageHighlighterMove(requestArrayElement);
                         MoveLeft(i);
                         break;
                     }
                 }
             }*/

            // move in left if both the above condions are false and then moving to right in case the tempHeadElement reaches 0
            else
            {
                // Check if left SCAN or right SCAN is chosen

                // move line left side and then right
                if (_chooseWhichDirSCAN == "left" && requestArrayElement <= requestsTxt.Length - 1)
                {
                    Debug.Log(_chooseWhichDirSCAN); ;

                    //moving to right side then
                    if (tempHeadElementStore == 0)
                    {
                        for (int i = 0; i <= (requestsTxt.Length - 2); i++)
                        {
                            // check if the element matches the request array element value
                            if (requestArray[requestArrayElement - 1].ToString() == requestsTxt[i].GetComponent<Text>().text)
                            {
                                ImageHighlighterMove(requestArrayElement - 1);
                                MoveRight(i);
                                break;
                            }
                        }
                    }

                    else
                    {
                        //moving to left side first
                        tempHeadElementStore = headPosition - 1;
                        for (int i = tempHeadElementStore; i >= 0; i--)
                        {
                            // check if the element matches the request array element value
                            if (tempHeadElementStore != 0 && requestArray[tempHeadElementStore - 1].ToString() == requestsTxt[i].GetComponent<Text>().text)
                            {
                                ImageHighlighterMove(tempHeadElementStore - 1);
                                MoveLeft(i);
                                break;
                            }

                            else if (i == 0)
                                MoveLeft(i);

                        }
                    }
                }

                // move line right side and then left
                if (_chooseWhichDirSCAN == "right" && requestArrayElement <= requestsTxt.Length - 1)
                {
                    //moving to right side 
                    if (tempHeadElementStore <= requestsTxt.Length - 2 && headPosition <= requestsTxt.Length - 2)
                    {
                        tempHeadElementStore = headPosition + 1;

                        for (int i = tempHeadElementStore; i <= (requestsTxt.Length - 2); i++)
                        {
                            // check if the element matches the request array element value
                            if (requestArray[tempHeadElementStore - 2].ToString() == requestsTxt[i].GetComponent<Text>().text)
                            {
                                ImageHighlighterMove(tempHeadElementStore - 2);
                                MoveRight(i);
                                break;
                            }
                        }

                        if (tempHeadElementStore == requestsTxt.Length - 1)
                        {
                            MoveRight(tempHeadElementStore);
                            requestArrayElement = (headPosition - requestArrayElement) - 1;
                        }

                    }

                    //moving to left side then
                    else
                    {
                        for (int i = requestArrayElement; i >= 0; i--)
                        {
                            // check if the element matches the request array element value
                            if (requestArray[requestArrayElement - 1].ToString() == requestsTxt[i].GetComponent<Text>().text)
                            {
                                ImageHighlighterMove(requestArrayElement - 1);
                                MoveLeft(i);
                                requestArrayElement = headPosition - 1;
                                break;
                            }
                        }
                    }
                }
            }
        }
        else
            Debug.Log("SCAN algorithm completed successfully!");
    }

    // Seek time calculation for SCAN
    public void SeekTimeSCAN(int storeBeginHeadPos, string _chooseWhichDirSCAN)
    {
        //uiHandler.inGamePanel.transform.GetChild(2).gameObject.SetActive(true);
        int temp = 0;

        // move towards right only, headposition = 0 or at 1st element
        /* if (storeBeginHeadPos == 0)
         {
             sortRequestArrayInAscendingOrder();

             for (int i = 0; i <= requestsTxt.Length - 2; i++)
             {
                 uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[i + 1].GetComponent<Text>().text + "-" + requestsTxt[i].GetComponent<Text>().text + ")";
                 if (i <= requestsTxt.Length - 3)
                     uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
             }

             uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "\n=\n";

             for (int i = 0; i <= requestsTxt.Length - 2; i++)
             {
                 uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[i].GetComponent<Text>().text);
                 temp += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[i].GetComponent<Text>().text);
                 if (i <= requestsTxt.Length - 3)
                     uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
             }
         }

         // move towards left only, headposition = 9 or at 10th element
         else if (storeBeginHeadPos == requestsTxt.Length - 1)
         {
             //sorting element in the request array in descending order
             sortRequestArrayInDescdingOrder();

             for (int i = requestsTxt.Length - 1; i >= 1; i--)
             {
                 uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[i].GetComponent<Text>().text + "-" + requestsTxt[i - 1].GetComponent<Text>().text + ")";
                 if (i >= 2)
                     uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
             }

             uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "\n=\n";

             for (int i = requestsTxt.Length - 1; i >= 1; i--)
             {
                 uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += int.Parse(requestsTxt[i].GetComponent<Text>().text) - int.Parse(requestsTxt[i - 1].GetComponent<Text>().text);
                 temp += int.Parse(requestsTxt[i].GetComponent<Text>().text) - int.Parse(requestsTxt[i - 1].GetComponent<Text>().text);
                 if (i >= 2)
                     uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
             }
         }*/

        // Left side move first calculations
        if (_chooseWhichDirSCAN == "left")
        {
            //left calculation
            for (int i = storeBeginHeadPos; i > 0; i--)
            {
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[i].GetComponent<Text>().text + "-" + requestsTxt[i - 1].GetComponent<Text>().text + ")";
                if (i >= 2)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
            }


            //right calculation
            for (int i = storeBeginHeadPos; i <= requestsTxt.Length - 3; i++)
            {
                if (i <= requestsTxt.Length - 3)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";

                if (i == storeBeginHeadPos)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[i + 1].GetComponent<Text>().text + "-" + requestsTxt[0].GetComponent<Text>().text + ")";

                else
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[i + 1].GetComponent<Text>().text + "-" + requestsTxt[i].GetComponent<Text>().text + ")";              
            }

            uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "\n=\n";

            //left calculation
            for (int i = storeBeginHeadPos; i > 0; i--)
            {           
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += int.Parse(requestsTxt[i].GetComponent<Text>().text) - int.Parse(requestsTxt[i - 1].GetComponent<Text>().text);
                temp+= int.Parse(requestsTxt[i].GetComponent<Text>().text) - int.Parse(requestsTxt[i - 1].GetComponent<Text>().text);
                if (i >= 2)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";

            }

            //right calculation
            for (int i = storeBeginHeadPos; i <= requestsTxt.Length - 3; i++)
            {

                if (i <= requestsTxt.Length - 3)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";

                if (i == storeBeginHeadPos)
                {
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[0].GetComponent<Text>().text);
                    temp += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[0].GetComponent<Text>().text);
                }

                else
                {
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text)-int.Parse(requestsTxt[i].GetComponent<Text>().text);
                    temp+= int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[i].GetComponent<Text>().text);
                }

            }
        }

        // Right side move first calculations
        if (_chooseWhichDirSCAN == "right")
        {
            //right calculation
            for (int i = storeBeginHeadPos; i < requestsTxt.Length - 1; i++)
            {
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[i + 1].GetComponent<Text>().text + "-" + requestsTxt[i].GetComponent<Text>().text + ")";
                if (i <= requestsTxt.Length - 3)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
            }

            //left calculation after right is complete
            for (int i = storeBeginHeadPos; i > 1; i--)
            {
                if (i >= 2)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";

                if (i == storeBeginHeadPos)
                     uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[requestsTxt.Length - 1].GetComponent<Text>().text + "-" + requestsTxt[i-1].GetComponent<Text>().text + ")";
                else
                     uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "(" + requestsTxt[i].GetComponent<Text>().text + "-" + requestsTxt[i - 1].GetComponent<Text>().text + ")";                
            }

            uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "\n=\n";

            //right calculation
            for (int i = storeBeginHeadPos; i < requestsTxt.Length - 1; i++)
            {
                uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[i].GetComponent<Text>().text);
                temp += int.Parse(requestsTxt[i + 1].GetComponent<Text>().text) - int.Parse(requestsTxt[i].GetComponent<Text>().text);

                if (i <= requestsTxt.Length - 3)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";
            }

            //left calculation after right is complete
            for (int i = storeBeginHeadPos; i > 1; i--)
            {
                if (i >= 2)
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "+";

                if (i == storeBeginHeadPos)
                {
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += int.Parse(requestsTxt[requestsTxt.Length - 1].GetComponent<Text>().text) - int.Parse(requestsTxt[i - 1].GetComponent<Text>().text);
                    temp += int.Parse(requestsTxt[requestsTxt.Length - 1].GetComponent<Text>().text) - int.Parse(requestsTxt[i - 1].GetComponent<Text>().text);
                }
                else
                {
                    uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text +=int.Parse( requestsTxt[i].GetComponent<Text>().text) - int.Parse(requestsTxt[i - 1].GetComponent<Text>().text);
                    temp += int.Parse(requestsTxt[i].GetComponent<Text>().text) - int.Parse(requestsTxt[i - 1].GetComponent<Text>().text);
                }
            }
        }
        uiHandler.GameOverPanel.transform.GetChild(2).GetComponent<Text>().text += "\n=\nThe seek time for your algorithm is: " + temp;
    }

    // --------------------------------------------- Common Code --------------------------------------------- \\
    //sorting elements in the request array in ascending order
    void sortRequestArrayInAscendingOrder()
    {

        for (int i = 0; i < requestArray.Length; i++)
        {
            for (int j = i; j < requestArray.Length; j++)
            {
                if (requestArray[i] > requestArray[j])
                {
                    int _temp = requestArray[i];
                    requestArray[i] = requestArray[j];
                    requestArray[j] = _temp;
                }
            }
        }
    }

    //sorting elements in the request array in descending order
    void sortRequestArrayInDescdingOrder()
    {

        for (int i = 0; i < requestArray.Length; i++)
        {
            for (int j = i; j < requestArray.Length; j++)
            {
                if (requestArray[i] < requestArray[j])
                {
                    int _temp = requestArray[i];
                    requestArray[i] = requestArray[j];
                    requestArray[j] = _temp;
                }
            }
        }
    }

    //moving line left and right
    public void MoveLeft(int moveToElement)
    {
        //calculate number of elements to move to the left
        pos_y += (3 * (headPosition - moveToElement));
        pos_x -= 1;

        lineRenderer.SetPosition(lineRendPosition, new Vector3(pos_x, pos_y, 0));

        //Incrementing certain required values
        requestArrayElement++;
        requestsProcessed++;
        lineRendPosition++;

        if (requestsProcessed <= (requestsTxt.Length - 2))
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRendPosition, new Vector3(pos_x, pos_y, 0));
        }

        headPosition = moveToElement;
    }
    public void MoveRight(int moveToElement)
    {
        //calculate number of elements to move to the right
        pos_y -= (3 * (moveToElement - headPosition));
        pos_x -= 1;

        lineRenderer.SetPosition(lineRendPosition, new Vector3(pos_x, pos_y, 0));

        //Incrementing certain required values
        requestArrayElement++;
        requestsProcessed++;
        lineRendPosition++;

        if (requestsProcessed <= (requestsTxt.Length - 2))
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRendPosition, new Vector3(pos_x, pos_y, 0));
        }

        headPosition = moveToElement;
    }

    //moving request highlighter  
    public void ImageHighlighterMove(int moveAccordingToElement)
    {
        imageHighlighter.SetActive(true);
        for (int i = 0; i < uiHandler.inGameSquenceTextBox.Length; i++)
        {
            if (uiHandler.inGameSquenceTextBox[i].text == requestArray[moveAccordingToElement].ToString())
            {
                imageHighlighter.transform.localPosition = uiHandler.inGameSquenceTextBox[i].transform.localPosition;
                break;
            }
        }
    }
}
