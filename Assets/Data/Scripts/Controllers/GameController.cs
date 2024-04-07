using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine.UI;
using Unity.VisualScripting;


public class GameController : MonoBehaviour
{

    static bool DEBUG = false;
    public static bool EDIT_MODE_ON = false;
    private Canvas _canvas;
    private GameObject _player;
    public GameObject worldHolder;

    void Start()
    {

        _player = GameObject.Find("Player");
        UIPopulateIcons();
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

    }   

    // START I UPDATE NAMJERNO NISU TU, GLEDAJ DRUGDE
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O)){
            SaveScene();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EDIT_MODE_ON = !EDIT_MODE_ON;
        }

        if (EDIT_MODE_ON)
        {
            _canvas.enabled = true;
        }
        else
        {
            _canvas.enabled = false;
        }

    }

    public static bool spawnItemByName(string itemName)
    {
        DirectoryInfo dir = new DirectoryInfo("Assets/GameObjects");
        FileInfo[] info = dir.GetFiles($"{itemName}_object.prefab", SearchOption.AllDirectories);
        if (info.Length >= 2)
        {
            if (DEBUG) Debug.Log($"Duplicate items found : {itemName}");
            return false;
        }

        if (info.Length == 0)
        {
            if (DEBUG) Debug.Log($"No items found : {itemName}");
            return false;
        }

        string itemPath = info[0].FullName.Replace("C:\\Users\\Antonio\\Island3URP\\", "");
        if (DEBUG) Debug.Log($"Item {info[0].Name} found. Directory : {itemPath}");
        GameObject item = (GameObject)AssetDatabase.LoadAssetAtPath(itemPath, typeof(GameObject));
        Instantiate(item);

        return true;

    }

    public static bool SpawnAllItems()
    {
        DirectoryInfo dir = new DirectoryInfo("Assets\\Data\\GameObjects");
        FileInfo[] info = dir.GetFiles("*_object.prefab", SearchOption.AllDirectories);
        if (info.Length == 0)
        {
            if (DEBUG) Debug.Log($"No items found : All Items");
            return false;
        }
        var itemPath = "";
        foreach (FileInfo file in info)
        {
            itemPath = file.FullName.Replace("C:\\Users\\Antonio\\Island3URP\\", "");
            if (DEBUG) Debug.Log($"Item {file.Name} found. Directory : {itemPath}");
            GameObject item = (GameObject)AssetDatabase.LoadAssetAtPath(itemPath, typeof(GameObject));
            Instantiate(item);
        }
        return true;
    }

    public bool SaveScene(){

        PrefabUtility.SaveAsPrefabAsset(worldHolder , "Assets\\worl.prefab");
        return true;
    }

    public static bool UIPopulateIcons()
    {


        GameObject icon = (GameObject)AssetDatabase.LoadAssetAtPath("Assets\\Data\\UI\\UI_icon.prefab", typeof(GameObject));
        DirectoryInfo dir = new DirectoryInfo("Assets\\Data\\GameObjects");
        FileInfo[] info = dir.GetFiles("*_icon.png", SearchOption.AllDirectories);
        foreach (FileInfo file in info)
        {

            string itemClass = file.FullName.Split("\\")[7] + "Panel";
            if (DEBUG) Debug.Log($"Item Class : {itemClass}");
            GameObject linkedObject = Instantiate(icon, GameObject.Find(itemClass).transform);

            string _path = file.FullName.Replace("C:\\Users\\Antonio\\Island3URP\\", "");
            linkedObject.GetComponent<Image>().sprite = (Sprite)AssetDatabase.LoadAssetAtPath(_path, typeof(Sprite));
            _path = _path.Replace("_icon.png", "_object.prefab");

            linkedObject.GetComponent<MenuItemIcon>().setLinkedItem(_path);

            if (DEBUG) Debug.Log($"File : {file} found");

        }


        return true;
    }
}

