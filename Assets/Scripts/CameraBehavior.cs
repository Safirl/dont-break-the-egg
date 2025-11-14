using UnityEngine;
using UnityEngine.UIElements;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 previousPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player || !gameObject)
        {
            print("player or gameObject is null!");
            return;
        }
        var newPlayerPosition = player.transform.position;
        var translation = newPlayerPosition - previousPosition;
        previousPosition = newPlayerPosition;
        // print(new Vector3(translation.x * .8f, translation.y * .8f, translation.z * .8f));
        gameObject.transform.position += new Vector3(translation.x, translation.y, translation.z);
    }
}
