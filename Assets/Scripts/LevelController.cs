using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    public Level Level => _level;
    public Board Board => _board;

    [SerializeField] private UIController _uiController;
    [SerializeField] private Level _level;
    [SerializeField] private Board _board;

    [SerializeField] private List<GameObject> _ingredientsList;

    private void Awake()
    {
        _uiController.SetupDependecy(this);
        _board.SetupDependency(this);
    }
    private void Start()
    {
        _ingredientsList = Utilities.CreateIngredientList(_level.BoardProperties);
        _board.Initialize(_ingredientsList);
    }

    public void ResetLevel()
    {
        _uiController.Dispose();
        _board.Dispose();
        _board.Initialize(_ingredientsList);
    }

    public void NewLevel()
    {
        _uiController.Dispose();
        _board.Dispose();
        _ingredientsList.Clear();
        _ingredientsList = Utilities.CreateIngredientList(_level.BoardProperties);

        _board.Initialize(_ingredientsList);
    }
}
