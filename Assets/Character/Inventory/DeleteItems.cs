using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteItems : MonoBehaviour
{
  public Cursor _cursor;
  public UISlotContainer _uiSlotContainer;

  public void onclick()
  {
    if (_cursor.isItemOnCursor) {
      _cursor.removeItem();
    }
  }
}