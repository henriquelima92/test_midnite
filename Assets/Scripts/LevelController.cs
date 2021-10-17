using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Level Level => _level;
    [SerializeField] private Level _level;
    [SerializeField] private Board _board;

    [SerializeField] private List<GameObject> _ingredientsList;

    private void Awake()
    {
        _board.SetupDependency(this);
    }
    private void Start()
    {
        _ingredientsList = Utilities.CreateIngredientList(_level.BoardProperties);
        _board.Initialize(_ingredientsList);
    }
}
