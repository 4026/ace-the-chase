using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragAndDroppable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Vector3 oldPos;
    public Transform dragSpace;
    public Transform oldParent;

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPos = transform.position;
        oldParent = transform.parent;
        this.transform.parent = dragSpace;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (oldParent)
        {
            this.transform.parent = oldParent;
            oldParent = null;
        }
        transform.position = oldPos;
        oldPos = Vector3.zero;
    }
}
