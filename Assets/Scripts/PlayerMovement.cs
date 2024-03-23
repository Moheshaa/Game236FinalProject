using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpPower = 10f;
    public float secondJumpPower = 10f;
    public Transform groundCheckPosition;
    public float radius = 0.3f;
    public LayerMask layerGround;

    private Rigidbody myBody;

    private bool isGrounded;
    private bool playerJumped = false;
    private bool canDoubleJump;

    public GameObject smokePosition;

    private bool GameStarted;

    private BGScroller bgScroller;

    private PlayerAnimation playerAnim;

    private Button JumpBTN;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        playerAnim = GetComponent<PlayerAnimation>();
        bgScroller = GameObject.Find(Tags.BACKGROUND_GAME_OBJ).GetComponent<BGScroller>();

        JumpBTN = GameObject.Find(Tags.JUMP_BUTTON).GetComponent<Button>();
        JumpBTN.onClick.AddListener(() => JumpTouch());
    }


    private void Start()
    {
        StartCoroutine(StartGame());
    }
    // Update is called once per frame
    void Update()
    {
        if (GameStarted)
        {
            PlayerMove();
            PlayerGrounded();
            PlayerJump();
        }

    }


    void PlayerMove()
    {
        myBody.velocity = new Vector3(movementSpeed, myBody.velocity.y, 0);
    }

    void PlayerGrounded()
    {
        isGrounded = Physics.OverlapSphere(groundCheckPosition.position, radius, layerGround).Length > 0;

    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && canDoubleJump)
        {
            canDoubleJump = false;
            myBody.AddForce(new Vector3(0, secondJumpPower, 0));
        }
        else if (Input.GetKeyUp(KeyCode.Space) && isGrounded)
        {
            playerAnim.DidJump();
            myBody.AddForce(new Vector3(0, jumpPower, 0));
            StartCoroutine(Jumping());
            canDoubleJump = true;
            playerJumped = true;

        }


    }

    public void JumpTouch()
    {
        if (!isGrounded && canDoubleJump)
        {
            canDoubleJump = false;
            myBody.AddForce(new Vector3(0, secondJumpPower, 0));
        }
        else if (isGrounded)
        {
            playerAnim.DidJump();
            myBody.AddForce(new Vector3(0, jumpPower, 0));
            StartCoroutine(Jumping());
            canDoubleJump = true;
            playerJumped = true;


        }
    }

    IEnumerator Jumping()
    {
        yield return new WaitForSeconds(0.1f);
        playerJumped = true;

    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
        GameStarted = true;
        bgScroller.canScroll = true;

        GameplayController.instance.canCountScore = true;
        smokePosition.SetActive(true);
        playerAnim.PlayerRun();

    }

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag == Tags.PLATFORM_TAG)
        {
            if (playerJumped)
            {
                playerJumped = false;
                playerAnim.DidLand();

            }
        }
    }
}
