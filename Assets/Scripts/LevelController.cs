using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameProperties Level => _level;
    public Board Board => _board;
    public LevelSerializer LevelSerializer => _levelSerializer;
    public List<GameObject> Ingredients => _ingredientsList;


    [SerializeField] private LevelSerializer _levelSerializer;
    [SerializeField] private UIController _uiController;
    [SerializeField] private GameProperties _level;
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

    private void DisposeLevel()
    {
        _uiController.Dispose();
        _board.Dispose();
        _ingredientsList.Clear();
    }

    public void ResetLevel()
    {
        _uiController.Dispose();
        _board.Dispose();
        _board.Initialize(_ingredientsList);
    }

    public void NewLevel()
    {
        DisposeLevel();
        _ingredientsList = Utilities.CreateIngredientList(_level.BoardProperties);

        _board.Initialize(_ingredientsList);
    }

    public void NewLevel(List<GameObject> ingredients)
    {
        DisposeLevel();
        _ingredientsList = ingredients;
        _board.Initialize(_ingredientsList);
    }
}
