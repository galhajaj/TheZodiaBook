using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }
}

public class Board : MonoBehaviour 
{
	public int BoardSizeX;
	public int BoardSizeY;
	public Transform TilePrefab;

    public List<Transform> Tiles = new List<Transform>();
    private Dictionary<string, Tile> _tilesDictioanry = new Dictionary<string, Tile>();

    void Start () 
	{
		generateTiles();
	}

    private void generateTiles()
	{
		float boardOriginX = this.transform.position.x;
		float boardOriginY = this.transform.position.y;
		float tileWidth = TilePrefab.GetComponent<SpriteRenderer> ().bounds.size.x;
		float tileHeight = TilePrefab.GetComponent<SpriteRenderer> ().bounds.size.y;
		
		for ( int x = 0; x < BoardSizeX; x++ ) 
		{
			for ( int y = 0; y < BoardSizeY; y++ ) 
			{
				Vector3 tilePosition = new Vector3(boardOriginX + (x * tileWidth), boardOriginY + (y * tileHeight), 0);
				Transform tile = (Transform)Instantiate(TilePrefab, tilePosition, Quaternion.identity);
				tile.name = "Tile" + x + y;
				tile.parent = this.transform;

                Tile tileScript = tile.GetComponent<Tile>();

                tileScript.PosX = x;
                tileScript.PosY = y;

                Tiles.Add(tile);
                _tilesDictioanry[x.ToString() + "-" + y.ToString()] = tileScript;
			}
		}
	}

    public Tile GetTile(int x, int y)
    {
        if (_tilesDictioanry.ContainsKey(x.ToString() + "-" + y.ToString()))
            return _tilesDictioanry[x.ToString() + "-" + y.ToString()];
        return null;
    }

    public bool IsPointInsideBoard(Point p)
    {
        if (p.X < 0 || p.X >= BoardSizeX)
            return false;
        if (p.Y < 0 || p.X >= BoardSizeY)
            return false;
        return true;
    }

    public bool IsPointsInsideBoard(List<Point> points)
    {
        foreach (Point p in points)
        {
            if (p.X < 0 || p.X >= BoardSizeX)
                return false;
            if (p.Y < 0 || p.X >= BoardSizeY)
                return false;
        }
        return true;
    }

    public Tile GetRandomFreeTile()
    {
        List<Tile> tilesList = new List<Tile>();

        foreach (Transform t in Tiles)
        {
            Tile tileScript = t.gameObject.GetComponent<Tile>();
            if (!tileScript.IsFull)
                tilesList.Add(tileScript);
        }

        if (tilesList.Count == 0)
            return null;

        return tilesList[Random.Range(0,tilesList.Count)];
    }

    public void ClearFillCounterZeroTiles()
    {
        foreach (Transform t in Tiles)
        {
            Tile tileScript = t.gameObject.GetComponent<Tile>();
            if (tileScript.FillCounter > 0)
                tileScript.FillCounter--;
            if (tileScript.FillCounter <= 0)
            {
                if (tileScript.IsFull && !tileScript.IsContainFood)
                    tileScript.Color = Color.white;
            }
        }
    }
}
