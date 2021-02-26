using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control_personaje_proto : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    public Animator anim;
    bool izquierda = false;
    bool derecha = false;
    bool caminar = false;
    bool atras = false;
    

    

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();   
        controller.center = new Vector3(0,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if(groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3 (Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if(move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        //cambia la posicion de altura del jugador.
        if(Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        

        if(Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("caminar",true);
        }
        else
        {
            anim.SetBool("caminar", false);
        }

       

        if(Input.GetKeyDown(KeyCode.S))
        {
            anim.SetBool("atras",true);
        }
        else
        {
            anim.SetBool("atras", false);
        }

     
    }
   
}
