using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private int initialBodySegments = 3;
    [SerializeField] private float fruitSpawnRange = 10f;
    [SerializeField] private int Gap;

    private List<GameObject> BodyList = new List<GameObject>();
    private List<Vector3> positionSnake = new List<Vector3>();

    private void Update()
    {
        MoveSnake();
    }

    private void MoveSnake()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        float rotation = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * rotation * rotationSpeed * Time.deltaTime);

        positionSnake.Insert(0, transform.position);

        int i = 0;

        foreach (GameObject prefab in BodyList)
        {
            int positionIndex = Mathf.Min(i * Gap, positionSnake.Count - 1);
            Vector3 position = positionSnake[positionIndex];

            
            Vector3 oldPosition = prefab.transform.position;
            Quaternion oldRotation = prefab.transform.rotation;

           
            Vector3 newPosition = position;
            Quaternion newRotation = Quaternion.LookRotation(position - oldPosition);

            
            prefab.transform.position = Vector3.Lerp(oldPosition, newPosition, Time.deltaTime * moveSpeed);
            prefab.transform.rotation = Quaternion.Slerp(oldRotation, newRotation, Time.deltaTime * rotationSpeed);

            i++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            Debug.Log("Fruit eaten");
            Destroy(other.gameObject);
            MySnakePrefab();
            StartCoroutine(SpawnFruit());
        }
    }

    private void Start()
    {
        // Змейка создается в Start
        for (int i = 0; i < initialBodySegments; i++)
        {
            MySnakePrefab();
        }
        // Фрукт спавнится в Start
        SpawnFruit();
    }

    private void MySnakePrefab()
    {
        GameObject prefab = Instantiate(bodyPrefab);
        BodyList.Add(prefab);
    }

    private IEnumerator SpawnFruit()
    {
        
        yield return new WaitForSeconds(1.0f);
        float x = Random.Range(-fruitSpawnRange, fruitSpawnRange);
        float z = Random.Range(-fruitSpawnRange, fruitSpawnRange);
        Vector3 spawnPosition = new Vector3(x, 0f, z);
        Instantiate(fruitPrefab, spawnPosition, Quaternion.identity);
    }
}
