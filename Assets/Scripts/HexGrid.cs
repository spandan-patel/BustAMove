using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public List<List<HexTile>> hexTileList = new List<List<HexTile>>();
    
    public int rowCount;
    public float bubbleInnerRadius = 0.2f;

    public GameObject[] bubblePrefab;

    private float bubbleOuterRadius;
    private int gridSizeX, gridSizeY;

    private float topLeftCornerX = -3.0f, topLeftCornerY = 5.0f;

    //possible max height of grid 
    private float maxHeight = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        bubbleOuterRadius = bubbleInnerRadius * 2 / Mathf.Sqrt(3);

        gridSizeX = Mathf.RoundToInt(-topLeftCornerX / bubbleInnerRadius);
        gridSizeY = Mathf.CeilToInt(maxHeight / (bubbleOuterRadius * 1.5f));

        CreateGrid();

        PopulateGridWithBubbles();
    }

    void PopulateGridWithBubbles()
    {
        for(int i = 0; i < rowCount; i++)
        {
            for(int j = 0; j < hexTileList[i].Count; j++)
            {
                List<HexTile> currentList = hexTileList[i];
                int randomBubbleIndex = Random.Range(0, bubblePrefab.Length);

                GameObject spawnedBubble = Instantiate(bubblePrefab[randomBubbleIndex], currentList[j].worldPosition, Quaternion.identity, this.transform);

                spawnedBubble.transform.localScale = Vector2.one * bubbleInnerRadius / 0.2f;
            }
        }
    }

    void CreateGrid()
    {
        Vector2 topLeft = new Vector2(topLeftCornerX + bubbleInnerRadius, topLeftCornerY - bubbleOuterRadius);
        Vector2 spawnLocation = Vector2.zero;

        for(int i = 0; i < gridSizeY; i++)
        {
            bool isEvenRow = (i == 0 || i % 2 == 0) ? true : false;

            gridSizeX = (isEvenRow) ? Mathf.RoundToInt(-topLeftCornerX / bubbleInnerRadius) : Mathf.RoundToInt(-topLeftCornerX / bubbleInnerRadius) - 1;

            List<HexTile> tempList = new List<HexTile>();

            for (int j = 0; j < gridSizeX; j++)
            {
                if (isEvenRow)
                {
                    spawnLocation = topLeft + (Vector2.right * j * bubbleInnerRadius * 2) + (Vector2.down * i * bubbleOuterRadius * 1.5f);
                }
                
                else
                {
                    spawnLocation = topLeft + (Vector2.right * i * bubbleInnerRadius) - (Vector2.right * (i - 1) * bubbleInnerRadius) + (Vector2.right * j * bubbleInnerRadius * 2) + (Vector2.down * i * bubbleOuterRadius * 1.5f);
                }

                HexTile newTile = new HexTile(j, i, spawnLocation);

                tempList.Add(newTile);
            }

            hexTileList.Add(tempList);
        }
    }

    public Vector3 GetLocationFromWorldPoint(Vector3 WorldPosition)
    {
        return Vector3.zero;
    }

    public List<Bubble> GetNeighbourBubbles(Bubble currentBubble)
    {
        return null;
    }
}
