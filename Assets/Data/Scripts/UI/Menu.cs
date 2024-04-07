using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Cursor _cursor;
    // Start is called before the first frame update

    public void OnPointerEnter(PointerEventData eventData)
    {
        _cursor.isOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _cursor.isOverUI = false;
    }

}
