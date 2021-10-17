using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Grid
{
    public Slot[,] Slots;
}

public class Board : MonoBehaviour
{
    public Action<bool> OnGameEnd;

    private LevelController _levelController;

    private Grid _grid;
    
    private GameObject _board;
    private GameObject _tilesRoot;
    
    [SerializeField] private Vector2Int _draggingObjectIndex = new Vector2Int(-1, -1);
    [SerializeField] private int _ingredientsAmount;

    public void SetupDependency(LevelController levelController)
    {
        _levelController = levelController;
    }
    public void Initialize(List<GameObject> ingredientsList)
    {
        _grid = new Grid();

        var levelProperties = _levelController.Level;

        var boardProperties = levelProperties.BoardProperties;
        var tileProperties = levelProperties.TileProperties;

        _board = Instantiate(boardProperties.Background);
        _tilesRoot = new GameObject("Slots");

        var tileSize = tileProperties.Size + tileProperties.Spacing;
        Vector3 startPosition = new Vector3(
            x: (Mathf.Floor(boardProperties.Dimensions.x / 2) * tileSize.x) * -1, 
            y: 0f,
            z: Mathf.Floor(boardProperties.Dimensions.y / 2) * tileSize.z);
        _grid.Slots = new Slot[boardProperties.Dimensions.x, boardProperties.Dimensions.y];

        _ingredientsAmount = ingredientsList.Count;

        var slotPrefab = levelProperties.Slot;
        var slotCount = 0;

        for (int y = 0; y < boardProperties.Dimensions.y; y++)
        {
            for (int x = 0; x < boardProperties.Dimensions.x; x++)
            {
                var position = new Vector3(startPosition.x + tileSize.x * x, 0f, startPosition.z - tileSize.z * y);
                var slotGO = Instantiate(slotPrefab, position, Quaternion.identity, _tilesRoot.transform);
                slotGO.name = $"{slotPrefab.name}_{slotCount}";
                slotCount += 1;

                Slot slot = slotGO.GetComponent<Slot>();
                slot.SetupDependency(tileProperties, this);
                _grid.Slots[y, x] = slot;

                if (ingredientsList.Count > 0)
                {
                    var ingredient = ingredientsList[0];
                    var ingredientGO = Instantiate(ingredient, position, Quaternion.identity, _tilesRoot.transform);
                    ingredientGO.name = ingredient.name;
                    ingredientGO.transform.localScale = tileProperties.Size;

                    slot.AddIngredientToSlot(ingredientGO);
                    ingredientsList.Remove(ingredient);

                    //Debug.Log($"GRID TILE: " +
                    //$"\n INGREDIENT {_grid.Tiles[x, y].Ingredient}" +
                    //$"\n POSITION {_grid.Tiles[x, y].Position}" +
                    //$"\n ISOCCUPIED {_grid.Tiles[x, y].IsOccupied}");
                }

                ////Debug.Log($"GRID TILE: " +
                ////    $"\n INGREDIENT {_grid.Tiles[x,y].Ingredient}" +
                ////    $"\n POSITION {_grid.Tiles[x, y].Position}" +
                ////    $"\n ISOCCUPIED {_grid.Tiles[x, y].IsOccupied}");
            }
        }
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
            Slot baseSlot = _grid.Slots[baseIngredientIndex.x, baseIngredientIndex.y];
            Slot draggingSlot = _grid.Slots[draggingIngredientIndex.x, draggingIngredientIndex.y];

            baseSlot.AddIngredientToStack(draggingSlot);
            draggingSlot.ClearStack();

            _draggingObjectIndex = new Vector2Int(-1, -1);

            CheckEndGame();
        }
        else
        {
            _draggingObjectIndex = new Vector2Int(-1, -1);
        }
    }
    private void CheckEndGame()
    {
        var boardProperties = _levelController.Level.BoardProperties;

        for (int y = 0; y < boardProperties.Dimensions.y; y++)
        {
            for (int x = 0; x < boardProperties.Dimensions.x; x++)
            {
                var tile = _grid.Slots[y, x];

                if (tile.IsOccupied == true)
                {
                    var rightTileIndex = x + 1;
                    if (rightTileIndex < boardProperties.Dimensions.x)
                        if (_grid.Slots[y, rightTileIndex].IsOccupied)
                            continue;

                    var leftTileIndex = x - 1;
                    if (leftTileIndex >= 0)
                        if (_grid.Slots[y, leftTileIndex].IsOccupied)
                            continue;

                    var topTileIndex = y - 1;
                    if (topTileIndex >= 0)
                        if (_grid.Slots[topTileIndex, x].IsOccupied)
                            continue;

                    var downTileIndex = y + 1;
                    if (downTileIndex < boardProperties.Dimensions.y)
                        if (_grid.Slots[downTileIndex, x].IsOccupied)
                            continue;

                    var hasWin = HasWin();
                    OnGameEnd?.Invoke(hasWin);
                    Debug.Log($"Game End Event Called! WIN:{hasWin}");
                    return;
                }
            }
        }
    }
    private bool HasWin()
    {
        var boardProperties = _levelController.Level.BoardProperties;

        for (int y = 0; y < boardProperties.Dimensions.y; y++)
        {
            for (int x = 0; x < boardProperties.Dimensions.x; x++)
            {
                var slot = _grid.Slots[y, x];

                if (slot.IsOccupied)
                {
                    var firstLayer = slot.Stack[0];
                    var lastLayer = slot.GetLastStackedItem();

                    var layers = "LAYERS : ";
                    for (int i = 0; i < slot.Stack.Count; i++)
                    {
                        layers += $"\n {slot.Stack[i].name}";
                    }

                    Debug.Log(layers);
                    if(slot.Stack.Count > 1 && (slot.Stack.Count == _ingredientsAmount))
                    {
                        if (firstLayer.name == "Bread" && lastLayer.name == "Bread")
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    private Vector2Int GetIngredientIndex(Slot slot)
    {
        for (int x = 0; x < _grid.Slots.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.Slots.GetLength(1); y++)
            {
                var item = _grid.Slots[x, y].GetLastStackedItem();
                if(item != null && item == slot.GetLastStackedItem())
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }
    
    #region CALLBACK EVENTS
    public void OnPointerDown(Slot slot)
    {
        if (slot == null)
        {
            _draggingObjectIndex = new Vector2Int(-1, -1);
            return;
        }

        _draggingObjectIndex = GetIngredientIndex(slot);
    }
    public void OnPointerUp(Slot slot)
    {
        if (slot == null)
        {
            _draggingObjectIndex = new Vector2Int(-1, -1);
            return;
        }

        Vector2Int baseIngredientIndex = GetIngredientIndex(slot);
        MoveIngredient(baseIngredientIndex, _draggingObjectIndex);
    }
    #endregion
}
