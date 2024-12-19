using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject[] _cat;
    [SerializeField] private GameObject[] _block;

    [SerializeField] private GameObject _CatsContainer;
    [SerializeField] private GameObject _BlockContainer;

    [SerializeField] private GameObject _RoadObject;
    [SerializeField] private GameObject _RoadContainer;

    [SerializeField] private GameObject _Border;

    private Vector3 _initPos;
    void Start()
    {
        CharacterMovement.OnStartSpawn += TriggerSpawn;

        //Get player init position
        _initPos = transform.position;

        //Spawn Init object at begin
        InitSpawn(_cat, _CatsContainer, _RoadContainer);
        InitSpawn(_block, _BlockContainer, _RoadContainer);

    }
    private void OnDestroy()
    {
        CharacterMovement.OnStartSpawn -= TriggerSpawn;
    }

    //Spawn object Cat or Block
    public void SpawnObject(GameObject[] arrObj, GameObject objContainer, Vector3 spawnPos)
    {
        //newSpawnPos position will be around vector spawnPos
        float xPos = Random.Range(spawnPos.x - 10, spawnPos.x + 10);
        float yPos = Random.Range(spawnPos.z - 10, spawnPos.z + 10);
        Vector3 newSpawnPos = new Vector3 (xPos, 0, yPos);
        GameObject newObj = Instantiate(arrObj[Random.Range(0, arrObj.Length - 1)], newSpawnPos, Quaternion.identity);
        
        //Random scale block object for more random map generate 
        if (objContainer.transform.tag == "Block") 
        {
            float scale = Random.Range(1, 2f);
            newObj.transform.localScale = new Vector3(scale, scale, scale);
        }
        newObj.transform.parent = objContainer.transform; 
    }

    //Init spawn depending on how many road in current Init Map
    public void InitSpawn(GameObject[] arrObj, GameObject objContainer, GameObject RoadContainer) 
    {
        for (int i = 2; i < RoadContainer.transform.childCount; i++) 
        {
            Vector3 spawnPos = RoadContainer.transform.GetChild(i).position;
            SpawnObject(arrObj, objContainer, spawnPos);
        }
    }

    //Road Spawn function, each road spawn position gab is 20
    public void roadSpawn(GameObject RoadContainer, GameObject RoadObj)
    {
        int roadGap = 20;

        //Get last road in container then calculate next road post
        GameObject lastRoad = RoadContainer.transform.GetChild(RoadContainer.transform.childCount - 1).gameObject;
        Vector3 spawnPos = lastRoad.transform.position + new Vector3(roadGap, 0, 0);
        GameObject newObj = Instantiate(RoadObj, spawnPos, Quaternion.identity);
        newObj.transform.parent = RoadContainer.transform;
    }

    //Delete object for reduce map render
    public void ObjDelete(GameObject CatContainer, GameObject BlockContainer) 
    {
        GameObject firstCat = CatContainer.transform.GetChild(0).gameObject;
        GameObject firstBlock = BlockContainer.transform.GetChild(0).gameObject;
        if (CatContainer.transform.childCount > 8) 
        {
            Destroy(firstCat);
        }
        Destroy(firstBlock);
    }
    public void roadDelete(GameObject RoadContainer)
    {
        GameObject firstRoad = RoadContainer.transform.GetChild(0).gameObject;
        Destroy(firstRoad);
    }
    public void borderChange(GameObject border)
    {
        Vector3 scale = new Vector3(20, 0, 0);
        border.transform.Translate(scale);
    }    

    //Call all spawn function everytime Player reach some certain position
    private void TriggerSpawn()
    {
        roadSpawn(_RoadContainer,_RoadObject);

        
        //Get last Road just spawn position
        Vector3 spawnPos = _RoadContainer.transform.GetChild(_RoadContainer.transform.childCount - 1).gameObject.transform.position;
        SpawnObject(_cat, _CatsContainer, spawnPos);
        SpawnObject(_block, _BlockContainer, spawnPos);
        

        //Delete Object
        roadDelete(_RoadContainer);
        ObjDelete(_CatsContainer, _BlockContainer);
        borderChange(_Border);
    }
}
