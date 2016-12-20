using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public enum SnakeDirection
{
    RIGHT, LEFT, UP, DOWN
}

public class GameLoop : MonoBehaviour 
{
    public Board Board;
    private Point _snakeHead;
    public SnakeDirection Direction;
    private bool _isFoodExists = false;

    public Text ScoreText;
    private int _score = 0;
    public int Score { get { return _score; } }

    public float StepTime = 0.5F;
    private float _timePassed = 0.0F;
	// Use this for initialization
	void Start () 
    {
        _snakeHead = new Point(Board.BoardSizeX / 2, Board.BoardSizeY / 2);
        Time.timeScale = 0.0F;
	}
	
	// Update is called once per frame
	void Update () 
    {
        updateTexts();

        _timePassed -= Time.deltaTime;
        if (_timePassed <= 0.0F)
        {
            _timePassed = StepTime;

            // DO THINGS!
            if (Direction == SnakeDirection.DOWN)
            {
                _snakeHead.Y--;
            }
            else if (Direction == SnakeDirection.UP)
            {
                _snakeHead.Y++;
            }
            else if (Direction == SnakeDirection.RIGHT)
            {
                _snakeHead.X++;
            }
            else if (Direction == SnakeDirection.LEFT)
            {
                _snakeHead.X--;
            }

            Tile nextTile = Board.GetTile(_snakeHead.X, _snakeHead.Y);

            if (nextTile == null)
            {
                if (this.Score > PlayerPrefs.GetInt("BestScore"))
                    PlayerPrefs.SetInt("BestScore", this.Score);
                SceneManager.LoadScene("mainScene");
                return;
            }

            if (nextTile.IsFull)
            {
                if (nextTile.IsContainFood)
                {
                    _score++;
                    _isFoodExists = false;
                }
                else
                {
                    if (this.Score > PlayerPrefs.GetInt("BestScore"))
                        PlayerPrefs.SetInt("BestScore", this.Score);
                    SceneManager.LoadScene("mainScene");
                    return;
                }
            }

            nextTile.Color = Color.black;
            nextTile.FillCounter = _score + 4;
            Board.ClearFillCounterZeroTiles();

            if (!_isFoodExists)
            {
                Tile foodTile = Board.GetRandomFreeTile();
                foodTile.Color = Color.red;
                _isFoodExists = true;
            }
        }
	}

    private void updateTexts()
    {
        ScoreText.text = "Score: " + _score.ToString();
    }
}
