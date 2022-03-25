using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronQueue : MonoBehaviour
{
    public static PatronQueue instance { get; private set; }

    //public static Queue<Patron> queue = new Queue<Patron>();
    public static Queue<GameObject> queue = new Queue<GameObject>();

    public GameObject patronPrefab;

    public GameObject door;

    public int patronCount;
    void Start()
    {
        patronCount = 0;
    }

    void Awake()
    {
        instance = this;
        //gameObject.transform.position = new Vector3(0, 0, 0);
        //gameObject.transform.position = door.transform.position;

        /*
        GameObject patronObject = Instantiate(patronPrefab, this.transform);
        Patron patron = patronObject.GetComponent<Patron>();
        patron.PrintPatron();
        queue.Enqueue(patron);
        */
    }

    public int Size()
    {
        return queue.Count;
        //return queue.patronCount;
    }

    public GameObject NextPatron()
    {
        return queue.Dequeue();
    }

    public void AddPatron(GameObject newPatron)
    {
        queue.Enqueue(newPatron);
        patronCount++;
    }
}
