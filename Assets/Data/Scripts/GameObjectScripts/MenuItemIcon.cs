using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class MenuItemIcon : MonoBehaviour
{
    public string linkedItem = null;
    private Cursor _cursor;
    // Start is called before the first frame update
    void Start()
    {
        _cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(()=>{
           
            GameObject itemToSpawn = (GameObject)AssetDatabase.LoadAssetAtPath(linkedItem, typeof(GameObject));
            _cursor.setPrefabHeldInCursor(itemToSpawn);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setLinkedItem(string pathToGameObject){
        linkedItem = pathToGameObject;
    }


}
