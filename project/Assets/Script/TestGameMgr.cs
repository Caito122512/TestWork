using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGameMgr : MonoBehaviour
{
    public Button[] buttons;
    public Text resultText;  // 提示文本
    public Button restart_btn;  // 重来按钮
    private string COMPUTER = "O";
    private string PLAYER = "X";
    private string currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = PLAYER;
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => MakeMove(button));
        }
        restart_btn.onClick.AddListener(RestartGame);
        UpdateStatusTips();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeMove(Button button)
    {
        if (button.GetComponentInChildren<Text>().text == "" && currentPlayer == PLAYER)
        {
            button.GetComponentInChildren<Text>().text = PLAYER;
            if (CheckWin(PLAYER))
            {
                resultText.text = "Player Wins!";
                EndGame();
            }
            else if (IsBoardFull())
            {
                resultText.text = "It's a Draw!";
                EndGame();
            }
            else
            {
                currentPlayer = COMPUTER;
                UpdateStatusTips();
                StartCoroutine(AIMove());
            }
        }
    }

    IEnumerator AIMove()
    {
        yield return new WaitForSeconds(1);
        int bestScore = int.MinValue;
        Button bestMove = null;

        foreach (Button button in buttons)
        {
            if (button.GetComponentInChildren<Text>().text == "")
            {
                button.GetComponentInChildren<Text>().text = COMPUTER;
                int score = Minimax(buttons, false);
                button.GetComponentInChildren<Text>().text = "";

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = button;
                }
            }
        }

        if (bestMove != null)
        {
            bestMove.GetComponentInChildren<Text>().text = COMPUTER;
            if (CheckWin(COMPUTER))
            {
                resultText.text = "AI Wins!";
                EndGame();
            }
            else if (IsBoardFull())
            {
                resultText.text = "It's a Draw!";
                EndGame();
            }
            else
            {
                currentPlayer = PLAYER;
                UpdateStatusTips();
            }
        }
    }

    int Minimax(Button[] newBoard, bool isMaximizing)
    {
        if (CheckWin(COMPUTER))
        {
            return 1;
        }
        else if (CheckWin(PLAYER))
        {
            return -1;
        }
        else if (IsBoardFull())
        {
            return 0;
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            foreach (Button button in newBoard)
            {
                if (button.GetComponentInChildren<Text>().text == "")
                {
                    button.GetComponentInChildren<Text>().text = COMPUTER;
                    int score = Minimax(newBoard, false);
                    button.GetComponentInChildren<Text>().text = "";
                    bestScore = Mathf.Max(score, bestScore);
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            foreach (Button button in newBoard)
            {
                if (button.GetComponentInChildren<Text>().text == "")
                {
                    button.GetComponentInChildren<Text>().text = PLAYER;
                    int score = Minimax(newBoard, true);
                    button.GetComponentInChildren<Text>().text = "";
                    bestScore = Mathf.Min(score, bestScore);
                }
            }
            return bestScore;
        }
    }

    bool CheckWin(string player)
    {
        string[,] board = new string[3, 3];
        for (int i = 0; i < buttons.Length; i++)
        {
            board[i / 3, i % 3] = buttons[i].GetComponentInChildren<Text>().text;
        }

        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player)
            {
                return true;
            }
            if (board[0, i] == player && board[1, i] == player && board[2, i] == player)
            {
                return true;
            }
        }

        if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
        {
            return true;
        }

        if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
        {
            return true;
        }

        return false;
    }

    void UpdateStatusTips()
    {
        resultText.text = currentPlayer == PLAYER ? "Tips:玩家回合" : "Tips:电脑回合";
    }

    bool IsBoardFull()
    {
        foreach (Button button in buttons)
        {
            if (button.GetComponentInChildren<Text>().text == "")
            {
                return false;
            }
        }
        return true;
    }

    void EndGame()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    public void RestartGame()
    {
        foreach (Button button in buttons)
        {
            button.GetComponentInChildren<Text>().text = "";
            button.interactable = true;
        }
        resultText.text = "";
        currentPlayer = PLAYER;
        UpdateStatusTips();
    }

}
