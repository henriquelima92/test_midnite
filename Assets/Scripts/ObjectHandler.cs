using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Board board;

    public void SetupDepdency(Board board)
    {
        this.board = board;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        board.OnPointerDown(eventData.hovered.Count > 0 ? eventData.hovered[0] : null);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        board.OnPointerUp(eventData.hovered.Count > 0 ? eventData.hovered[0] : null);
    }
}
