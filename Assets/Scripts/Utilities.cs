using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static GameObject GetRandomIndexInList(List<GameObject> listOfObjects)
    {
        return listOfObjects[Random.Range(0, listOfObjects.Count - 1)];
    }
    public static List<T> ShuffleList<T>(List<T> original)
    {
        List<T> shuffledList = new List<T>(original.Count);
        List<T> originalList = new List<T>(original.Count);

        for (int i = 0; i < original.Count; i++)
        {
            originalList.Add(original[i]);
        }

        while (originalList.Count > 0)
        {
            int index = Random.Range(0, originalList.Count);
            shuffledList.Add(originalList[index]);
            originalList.RemoveAt(index);
        }

        return shuffledList;
    }

    public static List<GameObject> CreateIngredientList(BoardProperties boardProperties)
    {
        var list = new List<GameObject>();

        for (int i = 0; i < boardProperties.IngredientsAmount; i++)
        {
            list.Add(GetRandomIndexInList(boardProperties.Ingredients));
        }

        list = ShuffleList(list);
        list.Add(boardProperties.Bread);
        list.Add(boardProperties.Bread);

        return list;
    }
}
