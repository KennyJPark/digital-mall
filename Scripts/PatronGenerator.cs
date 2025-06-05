using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatronGenerator : MonoBehaviour
{
    public static PatronGenerator instance { get; private set; }

    public List<Sprite> patronSprites;
    public List<GameObject> npcPrefabs;

    public GameObject door;

    public GameObject patronQueueObj;

    PatronQueue patronQueue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        instance = this;
        patronQueue = patronQueueObj.GetComponent<PatronQueue>();
    }

    // Make into coroutine? Make into Async?
    //public IEnumerator GeneratePatrons(int num)
    public void GeneratePatrons(int num)
    {
        
        Debug.Log("num patrons: " + num);
        for (int i = 0; i < num; ++i)
        {
            Debug.Log("GENERATING NEW PATRON " + i);
            int spriteID = Random.Range(0, patronSprites.Count);
            GameObject patronObject = Instantiate(patronQueue.patronPrefab, door.transform.position + new Vector3(Random.Range(door.transform.position.x-1, door.transform.position.x+1 * 1.5f), Random.Range(3.0f, 5.0f), 0), Quaternion.identity, patronQueueObj.transform);
            patronObject.GetComponent<SpriteRenderer>().sprite = patronSprites[spriteID];
            //patronObject.GetComponent<Image>().sprite = patronSprites[spriteID];

            //Debug.Log(patronObject.transform.position);

            patronQueue.AddPatron(patronObject);

        }

        //yield return null;
    }

    public void RemovePatron()
    {

    }

    // Generate Patron with modifiers
    Patron GeneratePatron()
    {
        Patron newPatron = new Patron();
        return newPatron;

    }
}
