using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;

    private Rigidbody rb;
    private int count;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

    }
    private void Update()
    {
        GameObject cow = transform.Find("Cow").gameObject;
        if (rb.GetPointVelocity(Vector3.zero) == Vector3.zero)
        {
            cow.GetComponent<Animator>().SetBool("isMoving", false);
        }
        else
        {
            cow.GetComponent<Animator>().SetBool("isMoving", true);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            ObjectPool.Instance.DespawnObject("pickup",other.gameObject.GetComponent<PooledObject>());
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        /*if (count >= 2)
        {
            winText.text = "You Win!";
        }*/
    }
}