using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Level Level => _level;
    [SerializeField] private Level _level;
    [SerializeField] private Board _board;


    private void Awake()
    {
        _board.SetupDependency(this);
    }
    private void Start()
    {
        _board.Initialize();
    }
}
