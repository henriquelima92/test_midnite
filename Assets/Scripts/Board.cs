using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tile
{
    public Vector3 Position;
    public GameObject Ingredient;
    public bool IsOccupied 
    {  
        get { return Ingredient != null; }
    }
    public int Layers;
}

[Serializable]
public struct Grid
{
    public Tile[,] Tiles;
}

public class Board : MonoBehaviour
{
    private LevelController _levelController;

    private Grid _grid;
    
    private GameObject _board;
    private GameObject _tilesRoot;
    
    private Vector2Int _draggingObjectIndex = new Vector2Int(-1, -1);

    public void SetupDependency(LevelController levelController)
    {
        _levelController = levelController;
    }
    public void Initialize()
    {
        _grid = new Grid();

        var levelProperties = _levelController.Level;

        var boardProperties = levelProperties.BoardProperties;
        var tileProperties = levelProperties.TileProperties;

        _board = Instantiate(boardProperties.Background);
        _tilesRoot = new GameObject("Tiles");

        var tileSize = tileProperties.Size + tileProperties.Spacing;
        Vector3 startPosition = new Vector3(
            x: (Mathf.Floor(boardProperties.Dimensions.x / 2) * tileSize.x) * -1, 
            y: 0f,
            z: Mathf.Floor(boardProperties.Dimensions.y / 2) * tileSize.z);
        _grid.Tiles = new Tile[boardProperties.Dimensions.x, boardProperties.Dimensions.y];

        var ingredientsList = CreateIngredientList();
        for (int y = 0; y < boardProperties.Dimensions.y; y++)
        {
            for (int x = 0; x < boardProperties.Dimensions.x; x++)
            {
                var position = new Vector3(startPosition.x + tileSize.x * x, 0f, startPosition.z - tileSize.z * y);

                if (ingredientsList.Count > 0)
                {
                    var ingredient = ingredientsList[0];
                    var ingredientGO = Instantiate(ingredient, position, Quaternion.identity, _tilesRoot.transform);
                    ingredientGO.GetComponent<ObjectHandler>().SetupDepdency(this);
                    ingredientGO.name = $"Ingredient_{ingredientsList.Count - 1}";
                    ingredientGO.transform.localScale = tileProperties.Size;

                    _grid.Tiles[x, y] = new Tile()
                    {
                        Ingredient = ingredientGO,
                        Layers = 1,
                        Position = position
                    };
                    
                    ingredientsList.Remove(ingredient);

                    //Debug.Log($"GRID TILE: " +
                    //$"\n INGREDIENT {_grid.Tiles[x, y].Ingredient}" +
                    //$"\n POSITION {_grid.Tiles[x, y].Position}" +
                    //$"\n ISOCCUPIED {_grid.Tiles[x, y].IsOccupied}");
                    continue;
                }

                _grid.Tiles[x, y] = new Tile()
                {
                    Ingredient = null,
                    Layers = 0,
                    Position = position
                };

                //Debug.Log($"GRID TILE: " +
                //    $"\n INGREDIENT {_grid.Tiles[x,y].Ingredient}" +
                //    $"\n POSITION {_grid.Tiles[x, y].Position}" +
                //    $"\n ISOCCUPIED {_grid.Tiles[x, y].IsOccupied}");
            }
        }
    }

    public void OnPointerDown(GameObject ingredient)
    {
        if (ingredient == null)
        {
            _draggingObjectIndex = new Vector2Int(-1, -1);
            return;
        }

        _draggingObjectIndex = GetIngredientIndex(ingredient);

        //Debug.Log($"OnPointerDown {ingredient.name}");
    }
    public void OnPointerUp(GameObject ingredient)
    {
        if (ingredient == null)
        {
            _draggingObjectIndex = new Vector2Int(-1, -1);
            return;
        }

        //Debug.Log($"OnPointerUp {ingredient.name}");
        
        Vector2Int baseIngredientIndex = GetIngredientIndex(ingredient);
        MoveIngredient(baseIngredientIndex, _draggingObjectIndex);
    }

    private void MoveIngredient(Vector2Int baseIngredientIndex, Vector2Int draggingIngredientIndex)
    {
        bool isOnRightDirection = draggingIngredientIndex.x == (baseIngredientIndex.x - 1) && draggingIngredientIndex.y == baseIngredientIndex.y;
        bool isOnLeftDirection = draggingIngredientIndex.x == (baseIngredientIndex.x + 1) && draggingIngredientIndex.y == baseIngredientIndex.y;
        bool isOnUpDirection = draggingIngredientIndex.y == (baseIngredientIndex.y + 1) && draggingIngredientIndex.x == baseIngredientIndex.x;
        bool isOnDownDirection = draggingIngredientIndex.y == (baseIngredientIndex.y - 1) && draggingIngredientIndex.x == baseIngredientIndex.x;

        //Debug.Log($"isOnRightDirection {isOnRightDirection}");
        //Debug.Log($"isOnLeftDirection {isOnLeftDirection}");
        //Debug.Log($"isOnUpDirection {isOnUpDirection}");
        //Debug.Log($"isOnDownDirection {isOnDownDirection}");

        if (isOnRightDirection || isOnLeftDirection || isOnUpDirection || isOnDownDirection)
        {
            Tile baseIngredientTile = _grid.Tiles[baseIngredientIndex.x, baseIngredientIndex.y];
            Tile draggingIngredientTile = _grid.Tiles[draggingIngredientIndex.x, draggingIngredientIndex.y];
            baseIngredientTile.Layers += 1;

            var baseIngredient = baseIngredientTile.Ingredient;
            var draggingIngredient = _grid.Tiles[draggingIngredientIndex.x, draggingIngredientIndex.y].Ingredient;

            var layerOffset = _levelController.Level.TileProperties.LayerOffset;
            var layerHeight = baseIngredientTile.Layers * layerOffset;
            var position = baseIngredient.transform.position + new Vector3(0f, layerHeight, 0f);

            draggingIngredient.transform.SetParent(baseIngredient.transform);
            draggingIngredient.transform.position = position;

            draggingIngredientTile.Ingredient.GetComponent<Collider>().enabled = false;
            Destroy(draggingIngredientTile.Ingredient.GetComponent<ObjectHandler>());

            //Debug.Log($"BASE TILE: " +
            //$"\n INGREDIENT {baseIngredientTile.Ingredient}" +
            //$"\n POSITION {baseIngredientTile.Position}" +
            //$"\n ISOCCUPIED {baseIngredientTile.IsOccupied}" +
            //$"\n LAYERS {baseIngredientTile.Layers}");


            draggingIngredientTile.Ingredient = null;
            _draggingObjectIndex = new Vector2Int(-1, -1);
        }
        else
        {
            _draggingObjectIndex = new Vector2Int(-1, -1);
        }
    }

    private Vector2Int GetIngredientIndex(GameObject ingredient)
    {
        for (int x = 0; x < _grid.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.Tiles.GetLength(1); y++)
            {
                if(_grid.Tiles[x, y].Ingredient == ingredient)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }
    private List<GameObject> CreateIngredientList()
    {
        var boardProperties = _levelController.Level.BoardProperties;
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
