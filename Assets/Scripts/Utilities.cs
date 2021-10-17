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

        var slotsCount = boardProperties.Dimensions.x * boardProperties.Dimensions.y;
        var fillingsCount = boardProperties.IngredientsAmount;
        var breadsCount = boardProperties.BreadsAmount;

        var itemsCount = fillingsCount + breadsCount;

        for (int i = 0; i < itemsCount; i++)
        {
            if(fillingsCount > 0)
            {
                list.Add(GetRandomIndexInList(boardProperties.IngredientPrefabs));
                fillingsCount -= 1;
                continue;
            }

            if(breadsCount > 0)
            {
                list.Add(boardProperties.BreadPrefab);
                breadsCount -= 1;
            }
        }
        
        list = ShuffleList(list);

        var remainItems = slotsCount - itemsCount;

        if(remainItems > 0)
        {
            for (int i = 0; i < remainItems; i++)
            {
                list.Add(null);
            }
        }
        return list;
    }
}
