using UnityEngine;

public class weaponsHandler : MonoBehaviour
{



    //weapons handler controllers
    [HideInInspector]
    public GameObject[] weapons;
    [HideInInspector]
    public int numberOfUsableWeaponsInGame;
    [HideInInspector]
    public string[] weaponsLayersNames;


    //Weapons animations controllers
    private Animator[] anim;
    private int numberOfWeaponsWithAnimatorsOnIt;
    [HideInInspector]
    public bool attack;
    private bool weaponIsIdle;
    [HideInInspector]
    public bool theAnimationHasEnded;

    private MainController mC;
  

    private void Awake()
    {
        mC = GetComponentInParent<MainController>();
        //Setting the weapons layers names to a string array
        numberOfUsableWeaponsInGame = transform.childCount;
        weaponsLayersNames = new string[numberOfUsableWeaponsInGame];
        weaponsLayersNames[0] = "arms@hatchet";
        weaponsLayersNames[1] = "arms@bow";
        weaponsLayersNames[2] = "arms@DBshotgun";
        weaponsLayersNames[3] = "arms@dualRevolvers";
        weaponsLayersNames[4] = "arms@knife";
        weaponsLayersNames[5] = "arms@longrifle";
        weaponsLayersNames[6] = "arms@revolver2";
        weaponsLayersNames[7] = "arms@sharps";
        weaponsLayersNames[8] = "arms@throwing";

        //Setting the number of usable weapons in the game
        weapons = new GameObject[numberOfUsableWeaponsInGame];

        //Setting active all game weapons to work on with the script
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);            
        }


        //Set the weapons game object in the array of weapons using the layer names in the string weaponsLayersNames
        for (int i = 0; i < transform.childCount; i++)
        {               
            weapons[i] = GameObject.Find(weaponsLayersNames[i]);
        }
        
        
        //Animations components setter
        numberOfWeaponsWithAnimatorsOnIt = numberOfUsableWeaponsInGame;
        anim = new Animator[numberOfWeaponsWithAnimatorsOnIt];
        for (int i = 0; i < numberOfWeaponsWithAnimatorsOnIt; i++)
        {
            anim[i] = GameObject.Find(weaponsLayersNames[i]).GetComponent<Animator>();
        }
        //End of animation components



        //Deactivate all weapons
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);            
        }
        //Test logs
        /*
        for(int i =0; i < numberOfWeaponsWithAnimatorsOnIt; i++)
        {
            Debug.Log("The animator in the " + weaponsLayersNames[i] + " layer is (" + anim[i]+")");
        }*/
       


    }
    //Áll the code below is temporary and it MUST BE CHANGED ONCE THE INVENTORY IS READY TO USE
    void Update()
    {
        hatchetAnimController();
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            weapons[0].SetActive(true);
            weapons[1].SetActive(false);
           
          
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapons[0].SetActive(false);
            weapons[1].SetActive(true);


        }
    }



    //Animations Array Index Codes
    //Code 0: Hatchet
    //Code 1: Bow
    //Code 2: DBshotgun
    //Code 3: DualRevolvers
    //Code 4: Knife
    //Code 5: Long Rifle
    //Code 6: Revolver 2
    //Code 7: Sharps
    //Code 8: Dinamite

    //Code 0: Hatchet
    void hatchetAnimController() {
        // Debug.Log("The weapons that the players is holding is "+mC.whichWeaponsIsTheplayerHolding+" that weapon layer name is "+weaponsLayersNames[0]);
        if(mC.whichWeaponsIsTheplayerHolding == weaponsLayersNames[0])
        {
            //If the player has requested attack          
                if (attack == true && weaponIsIdle == true && theAnimationHasEnded)
                {
                    //Attacks              
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1)
                    {
                        if (!anim[0].GetCurrentAnimatorStateInfo(0).IsName("hatchetfire2"))
                        {
                            anim[0].Play("hatchetfire1");
                        }
                    }
                    else
                    {
                        if (!anim[0].GetCurrentAnimatorStateInfo(0).IsName("hatchetfire1"))
                        {
                            anim[0].Play("hatchetfire2");
                        }
                    }
                    if (theAnimationHasEnded)
                    {
                        weaponIsIdle = false;                        
                    }                
                }
                else 
                {
                    weaponIsIdle = true;                    
                }           
            if (weaponIsIdle && attack == false)
            {     //Once the hatchet all atack animations has ended enter here                
                anim[0].Play("hatchetIdle");
                theAnimationHasEnded = true;
                Debug.Log("Weapons is Idle!!");
            }
            
        }           
    }




}










