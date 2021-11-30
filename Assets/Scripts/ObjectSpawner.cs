using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    #region Variables
    public static ObjectSpawner instance;

    [SerializeField] private bool manuelSpawn;
    [SerializeField] private GameObject manuelSelectedObject;
    [SerializeField] private int spawnOfSelectedObject = 10;

    [SerializeField] private Transform spawnPoint1; //sets on inspector
    [SerializeField] private Transform spawnPoint2; //sets on inspector

    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnDelay = 0.1f;
    [SerializeField] private float objectSizeMultiplayer = 2f;
    [SerializeField] public string collactableObjectName;
    [SerializeField] private int themedOfSpawnableObjectCount = 6;
    //[SerializeField] private string interruptCollectableRandom;


    [SerializeField] private string choosedCollectableObjectName;

    [SerializeField] private List<GameObject> spawnableObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> themedSpawnableObjects = new List<GameObject>();
    #endregion
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }


    private void Start()
    {
        MakeThemeOfSpawnableObjects();
        if (manuelSpawn) //manuel spawn
            choosedCollectableObjectName = manuelSelectedObject.name;
        else //random spawn
            choosedCollectableObjectName = selectCollectable();
        collactableObjectName = choosedCollectableObjectName + "(Clone)";
        GameManager.instance.collectName = choosedCollectableObjectName;
    }

    private void MakeThemeOfSpawnableObjects() // Random select for example 6 objects not all objects, to make the game simple
    {
        List<GameObject> tempList = new List<GameObject>();
        tempList = spawnableObjects;
        for (int i = 0; i < themedOfSpawnableObjectCount; i++)
        {
            for (int a = 0; a < Mathf.Infinity; a++) //Mathf.Infinity because x might be null. If it's the case it must try again
            {
                int x = Random.Range(0, tempList.Count);
                if(tempList[x] != null)
                {
                    themedSpawnableObjects.Add(tempList[x]);
                    tempList.RemoveAt(x);
                    break;
                }
            }
        }
        if (manuelSpawn)
            SpawnObjectsManuel();
        else
            SpawnObjects();
    }
    #region Spawn Objects For The Settings That Sets on Inspector
    private async void SpawnObjectsManuel()
    {
        for (int i = 0; i < spawnOfSelectedObject; i++) // Spawn the selected object that selected on inspector for the amount of spawnOfSelectedObject
        {
            GameObject spawned = Instantiate(manuelSelectedObject, selectRandomPoint(), Quaternion.identity);
            spawned.transform.parent = this.transform;
            BiggerSizeEffect biggerSizeEffect = spawned.AddComponent<BiggerSizeEffect>();
            biggerSizeEffect.Effect(objectSizeMultiplayer, 10);
            await Task.Delay((int)(spawnDelay * 1000));
        }
        int restObjectsCount = spawnCount - spawnOfSelectedObject;
        for (int i = 0; i < restObjectsCount; i++) //Spawn the rest random for amount of restObjectsCount
        {
            GameObject spawned = Instantiate(selectRandomObject(), selectRandomPoint(), Quaternion.identity);
            spawned.transform.parent = this.transform;
            BiggerSizeEffect biggerSizeEffect = spawned.AddComponent<BiggerSizeEffect>();
            biggerSizeEffect.Effect(objectSizeMultiplayer, 10);
            await Task.Delay((int)(spawnDelay * 1000));
        }

        //UIManager.instance.ActivateStartGameButton();
        await Task.Delay((int)(2f * 1000));
        GameManager.instance.StartGame();
    }
    #endregion

    #region Spawn Objects Random
    private async void SpawnObjects()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject spawned = Instantiate(selectRandomObject(), selectRandomPoint(), Quaternion.identity);
            spawned.transform.parent = this.transform;
            BiggerSizeEffect biggerSizeEffect = spawned.AddComponent<BiggerSizeEffect>();
            biggerSizeEffect.Effect(objectSizeMultiplayer, 10);
            await Task.Delay((int)(spawnDelay * 1000));
        }


        //UIManager.instance.ActivateStartGameButton();
        await Task.Delay((int)(2f * 1000));
        GameManager.instance.StartGame();
    }

    
    private string selectCollectable()
    {
        return selectRandomObject().name;
    }

    private GameObject selectRandomObject()
    {
        int x = Random.Range(0, themedSpawnableObjects.Count);
        return themedSpawnableObjects[x];
    }
    private Vector3 selectRandomPoint()
    {
        float x = Random.Range(spawnPoint1.position.x, spawnPoint2.position.x);
        float y = Random.Range(spawnPoint1.position.y, spawnPoint2.position.y);
        float z = Random.Range(spawnPoint1.position.z, spawnPoint2.position.z);
        return new Vector3(x, y, z);
    }
    #endregion
}
