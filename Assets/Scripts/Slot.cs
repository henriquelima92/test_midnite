using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private BoxCollider _collider;

    public bool IsOccupied => Stack.Count > 0;
    public List<GameObject> Stack;

    private float _layerOffset = 0f;
    private Board _board;

    public void SetupDependency(TileProperties tileProperties, Board board)
    {
        _board = board;
        _layerOffset = tileProperties.LayerOffset;
        _collider.size = tileProperties.Size;
    }

    public GameObject GetLastStackedItem()
    {
        if(Stack.Count > 0)
        {
            return Stack[Stack.Count - 1];
        }

        return null;
    }
    public void AddIngredientToSlot(GameObject ingredient)
    {
        Stack.Add(ingredient);
        ingredient.transform.SetParent(transform);
    }
    public void AddIngredientToStack(Slot slot)
    {
        slot.ReverseStack();

        Stack.AddRange(slot.Stack);
        
        for (int i = 0; i < slot.Stack.Count; i++)
        {
            var ingredient = slot.Stack[i];

            ingredient.transform.SetParent(transform);
            ingredient.transform.position = transform.position;
        }

        for (int i = 0; i < Stack.Count; i++)
        {
            var ingredient = Stack[i];
            ingredient.transform.position = new Vector3(transform.position.x, _layerOffset * i, transform.position.z);
        }
    }
    public void ClearStack()
    {
        Stack.Clear();
    }
    public void ReverseStack()
    {
        if(Stack.Count > 1)
        {
            Stack.Reverse();

            for (int i = 0; i < Stack.Count; i++)
            {
                var ingredient = Stack[i];
                ingredient.transform.position = new Vector3(transform.position.x, _layerOffset * i, transform.position.z);
            }
        }
    }

    #region CALLBACK EVENTS
    public void OnPointerDown(PointerEventData eventData)
    {
        _board.OnPointerDown(eventData.hovered.Count > 0 ? eventData.hovered[0].GetComponent<Slot>() : null);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _board.OnPointerUp(eventData.hovered.Count > 0 ? eventData.hovered[0].GetComponent<Slot>() : null);
    }
    #endregion
}
