using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Dimension
{
    public int X, Y;
}

[Serializable]
public struct TileProperties
{
    public Vector3 Size;
    public Vector3 Spacing;
    public float LayerOffset;
}

[Serializable]
public struct BoardProperties
{
    public Vector2Int Dimensions;
    public int IngredientsAmount;
    public int BreadsAmount;

    [Header("Prefabs")]
    public GameObject Background;
    public GameObject Bread;
    public List<GameObject> Ingredients;
}

[CreateAssetMenu(fileName = "Level", menuName = "Levels/Create", order = 1)]
public class Level : ScriptableObject
{
    public BoardProperties BoardProperties;
    public TileProperties TileProperties;
}
