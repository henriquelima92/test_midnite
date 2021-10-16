using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Level _levelProperties;


    [SerializeField] private GameObject _board;
    [SerializeField] private GameObject _tilesRoot;
    
    private GameObject[,] _tiles;

    private void Start()
    {
        Initialize();
    }

    public void SetupDependency(Level levelProperties)
    {
        _levelProperties = levelProperties;
    }

    public void Initialize()
    {
        var boardProperties = _levelProperties.BoardProperties;
        var tileProperties = _levelProperties.TileProperties;

        _board = Instantiate(boardProperties.Background);
        _tilesRoot = new GameObject("Tiles");

        var tileSize = tileProperties.Size + tileProperties.Spacing;
        Vector3 startPosition = new Vector3(
            x: (Mathf.Floor(boardProperties.Dimensions.X / 2) * tileSize.x) * -1, 
            y: 0f,
            z: Mathf.Floor(boardProperties.Dimensions.Y / 2) * tileSize.z);

        _tiles = new GameObject[boardProperties.Dimensions.X, boardProperties.Dimensions.X];

        var ingredientsList = CreateIngredientList();
        for (int y = 0; y < boardProperties.Dimensions.Y; y++)
        {
            for (int x = 0; x < boardProperties.Dimensions.Y; x++)
            {
                var position = new Vector3(startPosition.x + tileSize.x * x, startPosition.y + tileSize.y, startPosition.z - tileSize.z * y);

                if(ingredientsList.Count > 0)
                {
                    var ingredient = ingredientsList[0];
                    var ingredientGO = Instantiate(ingredient, position, Quaternion.identity, _tilesRoot.transform);
                    ingredientGO.transform.localScale = tileProperties.Size;
                    _tiles[x, y] = ingredientGO;

                    ingredientsList.Remove(ingredient);
                    continue;
                }

                _tiles[x, y] = new GameObject();
            }
        }
    }

    private List<GameObject> CreateIngredientList()
    {
        var boardProperties = _levelProperties.BoardProperties;
        var list = new List<GameObject>();

        for (int i = 0; i < boardProperties.IngredientsAmount; i++)
        {
            list.Add(Utilities.GetRandomIndexInList(boardProperties.Ingredients));
        }

        list = Utilities.ShuffleList(list);
        list.Insert(0, boardProperties.Bread);
        list.Add(boardProperties.Bread);

        return list;
    }
}
