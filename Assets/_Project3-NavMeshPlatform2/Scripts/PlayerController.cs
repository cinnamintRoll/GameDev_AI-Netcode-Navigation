using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private NavMeshAgent agent;

    [Header("Collection Settings")]
    public int collectedItems = 0;
    public int requiredItems = 3;
    public TextMeshProUGUI collectionCounterText;

    [Header("Game State")]
    public bool allItemsCollected = false;
    public GameObject winPanel;

    private void Start()
    {
        // Make sure we have references to required components
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (cam == null)
            cam = Camera.main;

        UpdateCollectionUI();

        // Ensure win panel is hidden at start
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Click-to-move using NavMeshAgent
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if player collided with a collectible
        if (other.CompareTag("Collectible"))
        {
            CollectItem(other.gameObject);
        }

        // Check if player reached extraction point and has all items
        if (other.CompareTag("ExtractionPoint") && allItemsCollected)
        {
            WinGame();
        }
    }

    private void CollectItem(GameObject item)
    {
        collectedItems++;
        UpdateCollectionUI();

        // Disable the collectible
        item.SetActive(false);

        // Check if all items are collected
        if (collectedItems >= requiredItems)
        {
            allItemsCollected = true;
            Debug.Log("All items collected! Head to the extraction point!");
        }
    }

    private void UpdateCollectionUI()
    {
        if (collectionCounterText != null)
        {
            collectionCounterText.text = "Items: " + collectedItems + " / " + requiredItems;
        }
    }

    private void WinGame()
    {
        Debug.Log("Game Won!");

        // Show win panel
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        // Disable player movement
        if (agent != null)
        {
            agent.isStopped = true;
        }
    }
}