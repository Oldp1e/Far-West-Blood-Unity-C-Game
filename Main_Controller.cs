using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhysicsController))]
public class MainController : MonoBehaviour
{

    //MouseControllerView
    bool isCursorVisible;
    bool isCursorLocked;

    //Bool Verifications
    bool isInventoryOpen;

    [SerializeField]
    private float walkSpeed = 0.5f;
    [SerializeField]
    private float runSpeed = 1f;
    [SerializeField]
    private float bleeding = 0f;
    [SerializeField]
    private float thirst = 100f;
    [SerializeField]
    private float health = 100f;
    [SerializeField]
    private float hunger = 100f;
    [SerializeField]
    private bool hungerActivated = true;
    [SerializeField]
    private bool thirstActivated = true;


    float lastThirstUpdateTime = 0f;
    float lastHungerUpdateTime = 0f;
    float lastDamageUpdateTime = 0f;
    float currentTime;

    //Displays
    public Text healthDisplay;
    public Text hungerDisplay;

    //Footsteps sounds
    private AudioSource footSound;

    //Hurtsounds
    private AudioSource damagedSound;


    [SerializeField]
    private AudioClip[] feetSounds;
    [SerializeField]
    private AudioClip[] damageSounds;

    [Range(0f, 10f)]
    [SerializeField]
    private float mouseSensitivity = 3f;
    [HideInInspector]
    public bool isSprinting;
    [HideInInspector]
    public bool isWalking;

    //Items 
    public string whichWeaponsIsTheplayerHolding;

    private PhysicsController pC;
    private BloodScreen nb;
    private Inventory inv;
    private weaponsHandler wpHd;

    private void Awake()
    {
        wpHd = GetComponentInChildren<weaponsHandler>();
        inv = GetComponent<Inventory>();
        pC = GetComponent<PhysicsController>();
        footSound = GetComponent<AudioSource>();
        damagedSound = GetComponent<AudioSource>();
        nb = GameObject.Find("WeaponCamera").GetComponent<BloodScreen>();

    }

    private void Start()
    {
        isCursorVisible = false;
        isCursorLocked = true;
        isInventoryOpen = false;
    }


    //Entradas
    private void Update()
    {
        //Cursor options
        if (isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            Cursor.lockState = CursorLockMode.None;           
        }
        if (isCursorVisible == false)
        {
            Cursor.visible = false;
        }
        else {
            Cursor.visible = true;
        }



        //TEMPORARY LINES OF CODE

        if (Health <= 0f)
        {
            Health = 0f;
        }
        if (pC.isGrounded == false)
        {
            isWalking = false;
            isSprinting = false;
            pC.isCrouching = false;
        }
        //Temporary lines of code ends here







        //UI DISPLAY CONFIGS
        healthDisplay.text = "Health: " + Health;

        hungerDisplay.text = "Hunger: " + Hunger.ToString("F1");

        //Player Atributtes Functions
        healthRegen(); // Regenerador de vida
        hungerTimer(); //Contador da fome
        thirstTimer(); //Contador da sede

        //Sounds controller
        if (!footSound.isPlaying && isWalking == true)
        {
            footSound.clip = feetSounds[Random.Range(0, feetSounds.Length)];
            footSound.pitch = 1f;
            playFeetSound();
        }
        else if (!footSound.isPlaying && isSprinting == true)
        {
            footSound.clip = feetSounds[Random.Range(0, feetSounds.Length)];
            footSound.pitch = 1.5f;
            playFeetSound();
        }

        //Player Controllers

        //Weapons input
        weaponsHolderVerifier();
        if (Input.GetButtonDown("Attack") && wpHd.attack == false && !isInventoryOpen) {
            Attack(true);
            Debug.Log("Is Attacking");
        }
        else if(Input.GetButtonUp("Attack") && wpHd.attack == true)
        {
            if (whichWeaponsIsTheplayerHolding == "arms@hatchet") {
                if (wpHd.theAnimationHasEnded == true)
                {
                    Attack(false);
                }
            }
            else
            {
                Attack(false);
                Debug.Log("Is not Attacking");
            }
                                      
        }
        //Weapons input ends here

        if (Input.GetButtonDown("inventory"))
        {
            inv.inventoryEnabled = !inv.inventoryEnabled;
            isCursorVisible = !isCursorVisible;
            isCursorLocked = !isCursorLocked;
            isInventoryOpen = !isInventoryOpen;
        }
        //Run
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
        }//Stop Running
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
        }//Jump
        if (Input.GetButton("Jump") && pC.isGrounded)
        {
            pC.performJump();
        }//Crouch
        if (Input.GetButtonDown("Crouch"))
        {
            pC.performCrouch(true);
        }//Crouch
        else if (Input.GetButtonUp("Crouch"))
        {
            pC.performCrouch(false);
        }
        //Walks and sprint algorithm
        if (pC.isGrounded == true)
        {
            float movX = Input.GetAxisRaw("Horizontal");
            float movZ = Input.GetAxisRaw("Vertical");


            if (movX != 0 || movZ != 0 && isSprinting == false)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
            Vector3 _movHorizontal = transform.right * movX;
            Vector3 _movVertical = transform.forward * movZ;
            //Vetor de movimento final que aplica a soma do movimento horizontal mais o movimento vertical vezes a velocidade 
            if (isSprinting)
            {
                Vector3 velocity = (_movHorizontal + _movVertical).normalized * RunSpeed;
                pC.Move(velocity);
            }
            else
            {
                Vector3 velocity = (_movHorizontal + _movVertical).normalized * WalkSpeed;
                pC.Move(velocity);
            }
        }




        //Rotação da camera e do personagem no eixo X
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, yRot, 0f) * mouseSensitivity;

        //Aplica a rotação do personagem
        if (!isInventoryOpen) {
            pC.Rotate(_rotation);
        }
       





        //Rotação da camera no eixo Y
        float xRot = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = xRot * mouseSensitivity;

        //Aplica a rotação da camera
        if (!isInventoryOpen)
        {
            pC.CameraRotate(_cameraRotationX);
        }




    }



    //Encapsulamento da variavel de corrida
    public float RunSpeed
    {
        get
        {
            return runSpeed;
        }

        set
        {
            runSpeed = value;

        }
    }


    //Encapsulamento da variavel de movimento
    public float WalkSpeed
    {
        get
        {
            return walkSpeed;


        }
        set
        {
            walkSpeed = value;

        }
    }



    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public float Hunger
    {

        get
        {
            return hunger;

        }
        set
        {
            hunger = value;
        }


    }
    public float Thirst
    {

        get
        {
            return thirst;

        }
        set
        {
            thirst = value;
        }


    }
    public float Bleeding
    {
        get
        {
            return bleeding;

        }
        set
        {
            bleeding = value;
        }
    }




    //Funções de controle do player

    //Verifies wich weapons is the player holding
    void weaponsHolderVerifier()
    {
        //verifies if the weapons object array is active
        //Array weapons Index
        //Index 0: Hatchet
        //Index 1: Bow
        //Index 2: DBshotgun
        //Index 3: DualRevolvers
        //Index 4: Knife
        //Index 5: Long Rifle
        //Index 6: Revolver 2
        //Index 7: Sharps
        //Index 8: Dinamite     
        for (int i = 0; i < wpHd.numberOfUsableWeaponsInGame; i++){
            if (wpHd.weapons[i].activeSelf)
            {
                whichWeaponsIsTheplayerHolding = wpHd.weaponsLayersNames[i];
            }
        }
     
    }
        



    

        //Attacks
    void Attack(bool isAttacking){

        wpHd.attack = isAttacking;
    
    }
   

    




    //Leva dano
    private void takeDamage(float damagePoints)
    {
        Health = Health - damagePoints;
        damagedSound.clip = damageSounds[Random.Range(0, damageSounds.Length)];
        damagedSound.pitch = 1f;
        damagedSound.Play();
        takeDamageBlend();
    }
    //Regenera a vida do jogador se for diferente de 100
    private void healthRegen()
    {

        if (Health < 100f)
        {
            StartCoroutine(lifeRegen());
        }
        else
        {
            StopCoroutine(lifeRegen());
        }
    }
    //Regenera a vida a cada meio segundo (0.5f)
    IEnumerator lifeRegen()
    {

        Health += 1f;
        yield return new WaitForSeconds(5f);
    }



    //Algoritimo de Fome do qual desce a fome em um determinado tempo e de acordo com a intensidade do exercicio
    private void hungerTimer()
    {
        currentTime = Time.time;
        if (hungerActivated == true)
        {
            if (currentTime - lastHungerUpdateTime > 1f)
            {
                if (isSprinting) { Hunger = Hunger - 1.5f; } else { Hunger = Hunger - 0.05f; }
                lastHungerUpdateTime = currentTime;
            }

            if (Hunger <= 0f)
            {
                Hunger = 0f;
                if (currentTime - lastDamageUpdateTime > 3f)
                {
                    takeDamage(25f);
                    lastDamageUpdateTime = currentTime;
                }
            }
        }

    }

    private void thirstTimer()
    {
        currentTime = Time.time;
        if (thirstActivated == true)
        {
            if (currentTime - lastThirstUpdateTime > 1f)
            {
                if (isSprinting) { Thirst = Thirst - 0.8f; } else { Thirst = Thirst - 0.02f; }
                lastThirstUpdateTime = currentTime;
            }

            if (Thirst <= 0f)
            {
                Thirst = 0f;
                if (currentTime - lastDamageUpdateTime > 3f)
                {
                    takeDamage(10f);
                    lastDamageUpdateTime = currentTime;
                }
            }
        }

    }



    private void takeDamageBlend()
    {
        if (Health != 100f)
        {
            if (Health >= 50f)
            {
                nb.blendamount = 0.5f;
            }
            else if (Health <= 25f)
            {
                nb.blendamount = 1f;
            }
        }
    }

    private void playFeetSound()
    {
        footSound.Play();
    }
    private void stopFeetSound()
    {
        footSound.Stop();
    }
   
   


}



