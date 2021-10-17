using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISerializerController : MonoBehaviour
{
    private LevelSerializer _levelSerializer;
    private LevelController _levelController;

    [SerializeField] private Button _saveLevel;
    [SerializeField] private Button _loadLevel;
    [SerializeField] private TMP_Dropdown _selectedLevel;

    [SerializeField] private string selectedFilename;

    public void SetupDependency(LevelController levelController)
    {
        _levelController = levelController;
        _levelSerializer = levelController.LevelSerializer;

        _selectedLevel.AddOptions(_levelSerializer.GetFilesInFolder());
        if(_selectedLevel.options.Count > 0)
        {
            selectedFilename = _selectedLevel.options[0].text;
        }

        _selectedLevel.onValueChanged.RemoveAllListeners();
        _selectedLevel.onValueChanged.AddListener(OnDropdownValueChange);

        _saveLevel.onClick.RemoveAllListeners();
        _saveLevel.onClick.AddListener(delegate
        {
            _levelSerializer.Serialize(levelController.Ingredients);
            ReloadFiles();
        });

        _loadLevel.onClick.RemoveAllListeners();
        _loadLevel.onClick.AddListener(delegate
        {
            if(string.IsNullOrEmpty(selectedFilename))
            {
                return;
            }

            var ingredients = _levelSerializer.Deserialize(selectedFilename);
            _levelController.NewLevel(ingredients);
        });
    }

    private void ReloadFiles()
    {
        _selectedLevel.ClearOptions();
        _selectedLevel.AddOptions(_levelSerializer.GetFilesInFolder());
        if (_selectedLevel.options.Count > 0)
        {
            selectedFilename = _selectedLevel.options[0].text;
        }
    }

    private void OnDropdownValueChange(int index)
    {
        selectedFilename = _selectedLevel.options[index].text;
    }
}
