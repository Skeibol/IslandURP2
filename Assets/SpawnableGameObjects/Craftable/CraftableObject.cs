using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CraftableObject
{
    public string Name;
    public GameObject ObjectToSpawn;
    public GameObject RecipeIcon;
    public List<GameObject> ObjectRequirements;

}
