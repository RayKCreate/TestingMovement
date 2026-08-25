using UnityEngine;

public class LeanDetection : MonoBehaviour
{

    [SerializeField] Collider leanCollider;

    public bool leanToRight;
    public bool leanToLeft;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leanCollider = GetComponent<Collider>();
        leanToRight = false;
        leanToLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            if (gameObject.tag == "LeanDetectorR")
                leanToLeft = true;
            else if(gameObject.tag == "LeanDetectorL")
                leanToRight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            if (gameObject.tag == "LeanDetectorR" && leanToLeft == true)
                leanToLeft = false;
            else if (gameObject.tag == "LeanDetectorL" && leanToRight == true)
                leanToRight = false;            
        }
    }
}
