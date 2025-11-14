using UnityEngine;
using UnityEngine.UIElements;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private GameObject m_gameObject;
    [SerializeField] private GameObject player;
    private Vector3 previousPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (play || m_gameObject == null)
        {
            print("player or gameObject is null!");
            return;
        }
        var newPlayerPosition = gameObject.transform.position;
        var translation = newPlayerPosition - previousPosition;
        m_gameObject.transform.position += new Vector3(translation.x * .8f, translation.y * .8f, translation.z * .8f);
    }
}
