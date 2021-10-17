using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private LevelController _levelController;

    [SerializeField] private UISerializerController _uiSerializerController;
        
    [SerializeField] private Button _newLevel;
    [SerializeField] private Button _resetLevel;

    

    [SerializeField] private TMPro.TextMeshProUGUI _victoryFeedback;

    private void OnDestroy()
    {
        _levelController.Board.OnGameEnd -= OnGameEnd;
    }

    public void SetupDependecy(LevelController levelController)
    {
        _uiSerializerController.SetupDependency(levelController);

        _levelController = levelController;

        _newLevel.onClick.RemoveAllListeners();
        _newLevel.onClick.AddListener(delegate
        {
            _levelController.NewLevel();
        });

        _resetLevel.onClick.RemoveAllListeners();
        _resetLevel.onClick.AddListener(delegate
        {
            _levelController.ResetLevel();
        });

        _levelController.Board.OnGameEnd += OnGameEnd;
    }

    public void Dispose()
    {
        _victoryFeedback.text = "";
    }

    private void OnGameEnd(bool hasWin)
    {
        var text = hasWin == true ? "You Win" : "You lose";
        _victoryFeedback.text = text;
    }
}
