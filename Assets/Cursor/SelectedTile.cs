using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedTile : MonoBehaviour
{
    public WorldGenerator _worldGenerator;
    public TMP_Text _text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = _worldGenerator.getHeight(transform.position.x, transform.position.y).ToString();
        
    }
}
