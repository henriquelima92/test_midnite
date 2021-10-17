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
    [Range(0, 3)]
    public int BreadsAmount;

    [Header("Prefabs")]
    public GameObject BackgroundPrefab;
    public GameObject BreadPrefab;
    public List<GameObject> IngredientPrefabs;
}

[CreateAssetMenu(fileName = "GameProperties", menuName = "GameProperties/Create", order = 1)]
public class GameProperties : ScriptableObject
{
    public GameObject SlotPrefab;

    public BoardProperties BoardProperties;
    public TileProperties TileProperties;
}
