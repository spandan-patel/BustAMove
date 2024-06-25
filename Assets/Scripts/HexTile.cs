using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile
{
    public Vector2 worldPosition;

    int gridXCord;
    int gridYCord;

    public HexTile(int xCord, int yCord, Vector2 worldPos)
    {
        gridXCord = xCord;
        gridYCord = yCord;

        worldPosition = worldPos;
    }
}
