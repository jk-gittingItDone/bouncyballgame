using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
//this note was created on david's computer

public class PlayerController : MonoBehaviour
{
    public AudioClip collisionSound; // Drag the sound clip here in the Inspector
   
    public AudioSource audioSource;

    [SerializeField] float speed = 2.0f;
    [SerializeField] float fallThreshold = -3f;
    private Rigidbody rb;
    private float forwardInput;
    private GameObject focalPoint;
    public bool hasPowerup = false;
    [SerializeField] float powerUpStrength = 2.0f;
    [SerializeField] GameObject indicator;
   
    //Audio objects here
   


    void Start()
    {
        Debug.Log("Player Controller Script Called");
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

    }

    // Update is called once per frame
    void Update()
    {
        
        forwardInput = Input.GetAxis("Vertical");
        rb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        indicator.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        

        
       
        if(gameObject.transform.position.y < fallThreshold){
            Debug.Log("Player fell below -10");
            //destroy player
            Destroy(gameObject);
            // gameObject.SetActive(false);
            SceneManager.LoadScene(2, LoadSceneMode.Single);

        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            indicator.gameObject.SetActive(true);
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        indicator.gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player bumped into enemy");
            
            Debug.Log("Sound should work");
            audioSource.PlayOneShot(collisionSound);
            
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            //moved powerup strength to next if statement

            if(hasPowerup){
                enemyRb.AddForce(awayFromPlayer * powerUpStrength);
            }
        }
    }
}
