using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    private int _catRescueCnt = 0;
    private List<GameObject> _list;

    public void resetInventory() {
        _list = new List<GameObject>();
    }
    public void addCat(GameObject cat) 
    {
        _list.Add(cat);
    }
    public int getCat() { return _list.Count; }


}
    
