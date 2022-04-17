using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private GameObject player;
    public GameObject winCoin;
    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private Rigidbody rigidComp;
    private int superJump;
    private Vector3 begin;
    private Vector3 rP1;
    private Vector3 rP2;
    private string game;
    private GameObject [] superJumpCoins;
    public Transform groundCheckTransform;
    public LayerMask playerMask;
    public static Player instance;
    public AudioSource jumpN;
    public AudioSource superJumpN;
    public AudioSource endGameN;
    public AudioSource superCoinN;
    public AudioSource deadN;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rigidComp = GetComponent<Rigidbody>();
        superJump = 0;
        winCoin.GetComponent<Rigidbody>().useGravity = false;
        rP1 = GameObject.Find("Lvl1").transform.position;
        rP2 = GameObject.Find("Lvl2").transform.position;
        begin = rP1;
        superJumpCoins = GameObject.FindGameObjectsWithTag("SuperJumpCoin");

    }

    // Update is called once per frame. Key presses & inputs checked here and appy forces in FixedUpdate.
    void Update()
    {

        //Check if space key is pressed down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;

        }

        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayer();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("SampleScene");
            UISystem.pointsNumber = 0;
            UISystem.deathNumber = 0;
            Time.timeScale = 1;
        }

    }


    // FixedUpdate is called once every physics update.
    void FixedUpdate()
    {

        rigidComp.velocity = new Vector3(horizontalInput * 3, rigidComp.velocity.y, 0);

        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }


        if (jumpKeyWasPressed == true)
        {

            int jumpPower = 5;

            if (superJump > 0)
            {
                jumpPower *= 3;
                superJump -= 1;
                superJumpN.Play();
            }

            else
            {
                jumpN.Play();
            }

            rigidComp.AddForce(new Vector3(0, jumpPower, 0), ForceMode.VelocityChange);
            jumpKeyWasPressed = false;



        }


    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            other.gameObject.SetActive(false);
            UISystem.instance.AddPoint();
        }

        if (other.gameObject.tag == ("Respawn"))
        {
            RespawnPlayer();
            UISystem.instance.AddDeath();
            deadN.Play();
        }

        if (other.gameObject.layer == 8)
        {
            Destroy(other.gameObject);
            UISystem.instance.Text();
            GameObject.Find("Wall1").SetActive(false);
            GameObject.Find("basket1").GetComponent<Collider>().isTrigger = false;
            begin = rP2;
        }

        if (other.gameObject.tag == "SuperJumpCoin")
        {
            superJump += 1;
            Debug.Log("SuperJump + 1");
            superCoinN.Play();
        }

        if (other.gameObject.layer == 10)
        {
            Debug.Log("Coin Hit");
            winCoin.GetComponent<Rigidbody>().useGravity = true;
            UISystem.instance.Add1MPoint();
            UISystem.instance.TheEnd();
            endGameN.Play();
        }

        if (other.gameObject.tag == "winhoop2")
        {
            other.GetComponent<Rigidbody>().velocity = new Vector3(3, 3, 0);
        }

    }
    void RespawnPlayer()
    {
        player.transform.position = begin;
        UISystem.instance.WinTextOff();
        foreach (GameObject sjc in superJumpCoins)
        {
            sjc.SetActive(true);
        }

    }
}
