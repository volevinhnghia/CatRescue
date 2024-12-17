using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    private int _catRescueCnt = 0;
    private List<GameObject> list;

    public void resetInventory() { 
        list = new List<GameObject>();
    }
    public void addCat(GameObject cat) 
    {
        list.Add(cat);
    }
    public int getCat() { return list.Count; }


}
    
