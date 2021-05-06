using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MainController))]
public class PhysicsController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Rigidbody rb;

    private Vector3 move = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    [SerializeField]
    private float cameraRotationLimit = 85f;

    private CharacterController charController;


    //Variaveis de pulo e agachamento
    private CapsuleCollider player;
    public Vector3 jump;
    public float jumpForce = 2.0f;
    public bool isGrounded;
    public bool isCrouching;






    private void Awake()
    {
        player = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        //Movimenta o personagem
        performMovement();
        //Rotaticiona o personagem
        performRotation();

    }
    //Aplica o movimento caso o jogador de as entradas no teclado ou joystick a partir da função Move
    public void performMovement()
    {
        if (move != Vector3.zero)
        {
            rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
        }
    }
    //Aplica o movimento caso o jogador de as entradas no teclado ou joystick a partir da função Move
    public void performRotation()
    {
        if (rotation != Vector3.zero)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        }
        if (cam != null)
        {
            //Novo calculo de rotação
            currentCameraRotationX -= cameraRotationX;
            //Limita a rotação da camera entre o minimo e o maximo igualmente
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
            //Aplica a rotação a posição do transform de nossa camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX,0f,0f);
        }
    }


    //Aplica os vetores de movimento recebidos da classe MainController e os armazena dentro da variavel move
    public void Move(Vector3 _move)
    {
        move = _move;
    }


    //Aplica os vetores de rotação recebidos da classe MainController e os armazena dentro da variavel rotation
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }


    //Aplica os vetores de rotação recebidos da classe MainController e os armazena dentro da variavel rotation
    public void CameraRotate(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    public void performJump() {

        rb.AddForce(jump * jumpForce, ForceMode.Impulse);        
        isGrounded = false;

    }
    public void performCrouch(bool Crouch) {
        if (isCrouching == Crouch)
        {
            player.height = 2f;
        }
        else {
            player.height = 1f;
        }      
    }








}
