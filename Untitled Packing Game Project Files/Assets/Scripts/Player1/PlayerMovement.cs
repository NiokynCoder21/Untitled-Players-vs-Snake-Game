using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 move;
    public Rigidbody2D rb;
    public Rigidbody2D enemyRb;
    public Transform orientation;
    public float moveForce;
    public bool isRight = false;
    public bool isMoving = false;
    public float dashForce;
    public float pushForce;

    public int addItems;
    public int addKitchen;
    public int addLiving;
    public int addDining;

    public int loseDash; //cost of dash
    public int loseDashItems;
    public int loseTeleport; //cost of teleport
    public int loseTeleportItems;
    public int losePush; //cost of touch
    public int losePushItems;

    public GameObject door;
    public bool isDoor = false;
    public bool isDoorDestroyed = false;
    public bool isCar;

    public ItemManager itemManager;
    public ScoreManager scoreManager;
    public int scoreKitchen;
    public int scoreLiving;
    public int scoreDining;
    public bool isGrocery = false;
    public bool kitchenFood = false; //this is to check whether the player is touching kitchen groceries or not
    public bool diningFood = false; // this is to check whether the player is touching dining groceries or not
    public bool livingFood = false; //this is to check whether the player is touching living groceries or not

    public Image pinkGroceryUI;
    public Image blueGroceryUI;
    public Image purpleGroceryUI;

    public int loseKitchen;
    public int loseLiving;
    public int loseDining;

    public bool selectedKitchen = false;
    public bool selectedDining = false;
    public bool selectedLiving = false;

    public bool hasPicked = false; //this is to track whether the player has picked up a something or not
    public bool canPush = false; //check whether the player can push

   // public Transform teleportInside;
    public Transform teleportOutside;

    public GameObject teleportInside;
    public GameObject player;

    public bool hasTeleported = false;
    public GameObject playerTwo;
    public bool canTouch = false;
    public bool canMove = true;
    public float cooldownTime;

    public enum GroceryType
    {
        KitchenStuff,
        LivingRoomStuff,
        DiningRoomStuff,
    }

    public GroceryType selectedGrocery = GroceryType.KitchenStuff;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>(); //this detects input along the vector and allows movement 
    }

    public void onDoor(InputAction.CallbackContext context)
    {
        if(isDoor == true)
        {
            if (context.performed)
            {
                if (hasTeleported == false)
                {
                    TeleportIn();
                }

                else if (hasTeleported == true)
                {
                    TeleportOut();
                }
            }
        }
        
    }

    public void onPickUp(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            if (kitchenFood == true)
            {
                PickUp();

                if (itemManager != null)
                {
                    if (itemManager.currentItems < 10)
                    {
                        hasPicked = true;
                    }
                }

                hasPicked = true;
            }

            if (livingFood == true)
            {
                PickUp();

                if (itemManager != null)
                {
                    if (itemManager.currentItems < 10)
                    {
                        hasPicked = true;
                    }
                }
            }

            if (diningFood == true)
            {
                PickUp();

                if (itemManager != null)
                {
                    if (itemManager.currentItems < 10)
                    {
                        hasPicked = true;
                    }
                }
            }
        }

    }

    public void onDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DropItem();
        }
    }

    public void onLeftSelection(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwitchSelection(-1);
        }
    }

    public void onRightSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwitchSelection(1);
        }
    }

    public void onDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Dash();
        }
    }

    public void onTouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Touch();
        }
    }

    public void onTeleport(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Teleport();
        }
    }
    void FixedUpdate()
    {
        if (canMove == true)
        {
            Move();
        }
      
    }

    private void Update()
    {
        if (playerTwo == null)
        {
            playerTwo = GameObject.FindWithTag("PlayerTwo"); //get reference on start of game since the object becomes a clone

            if (playerTwo != null)
            {
                PlayerMovement1 player2 = playerTwo.GetComponent<PlayerMovement1>();
            }
        }

    }

    public void Move()
    {
       
        if (move.x > 0)
        {
            rb.AddForce(orientation.right * moveForce, ForceMode2D.Impulse);
        }

        if (move.x < 0)
        {
            rb.AddForce(-orientation.right * moveForce, ForceMode2D.Impulse);
        }


        if (move.y > 0)
        {
            rb.AddForce(orientation.up * moveForce, ForceMode2D.Impulse);
        }

        if (move.y < 0)
        {
            rb.AddForce(-orientation.up * moveForce, ForceMode2D.Impulse);
        }

        if (move.x == 0 && move.y == 0)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void TeleportIn()
    {
        Vector3 newPosition = new Vector3(6.1f, -118.3f, 0.1383239f); // Replace with your desired position
        Quaternion newRotation = Quaternion.identity; // Default rotation (no rotation)

        player.transform.SetPositionAndRotation(newPosition, newRotation);
        hasTeleported = true;
    }

    public void TeleportOut()
    {
        Vector3 newPosition = new Vector3(1.21f, 4.66f, 0f); // Replace with your desired position
        Quaternion newRotation = Quaternion.identity; // Default rotation (no rotation)

        player.transform.SetPositionAndRotation(newPosition, newRotation);
        hasTeleported = false;
    }


    public void Dash() //lowest cost ability
    {
        if (selectedDining == true)
        {
            if (itemManager != null)
            {
                if (itemManager.currentDiningStuff > 2)
                {
                    if (move.x > 0)
                    {
                        rb.AddForce(orientation.right * dashForce, ForceMode2D.Impulse);

                        if (itemManager != null)
                        {
                            itemManager.LossDash(loseDashItems, loseDash);
                        }
                    }

                    else if (move.x < 0)
                    {
                        rb.AddForce(-orientation.right * dashForce, ForceMode2D.Impulse);

                        if (itemManager != null)
                        {
                            itemManager.LossDash(loseDashItems, loseDash);
                        }
                    }


                    else if (move.y > 0)
                    {
                        rb.AddForce(orientation.up * dashForce, ForceMode2D.Impulse);

                        if (itemManager != null)
                        {
                            itemManager.LossDash(loseDashItems, loseDash);
                        }
                    }

                    else if (move.y < 0)
                    {
                        rb.AddForce(-orientation.up * dashForce, ForceMode2D.Impulse);

                        if (itemManager != null)
                        {
                            itemManager.LossDash(loseDashItems, loseDash);
                        }
                    }
                }
            }
        }


    }

    public void Touch() //second lowest cost ability
    {
        if (selectedLiving == true)
        {
            if (canTouch == true)
            {
                if (playerTwo != null)
                {
                    // Get the PlayerMovement script from the playerTwo object
                    PlayerMovement1 player2 = playerTwo.GetComponent<PlayerMovement1>();

                    // Disable movement by setting canMove to false
                    if (player2 != null)
                    {

                        if (itemManager != null)
                        {
                            if (itemManager.currentLivingStuff > 2)
                            {
                                player2.SetCanMove(false);
                                itemManager.LossTouch(losePushItems, losePush);
                            }
                        }

                    }
                }
            }
        } 

    }

    public void PickUp()
    {
        if (canMove)
        {
            if (itemManager != null)
            {

                if (itemManager.currentItems < 10)
                {
                    itemManager.AwardItems(addItems, addKitchen, addLiving, addDining);
                }
            }
        }
    }

    public void DropItem()
    {
         if (selectedKitchen == true)
         {
            scoreManager.AwardKitchenPoints(scoreKitchen);
            itemManager.LossKitchenStuff(addItems, loseKitchen);
         }


         if (selectedDining == true)
         {
             scoreManager.AwardDiningPoints(scoreDining);
             itemManager.LossDiningStuff(addItems, loseDining);
         }


         if (selectedLiving == true)
         {
             scoreManager.AwardLivingPoints(scoreLiving);
             itemManager.LossLivingStuff(addItems, loseLiving);
         }    

    }

    public void Teleport() //most expensive ability
    {
        if (selectedKitchen == true)
        {
            if (itemManager != null)
            {
                if (itemManager.currentKitchenStuff > 4)
                {
                    Vector3 newPosition = new Vector3(6.1f, -118.3f, 0.1383239f); // Replace with your desired position
                    Quaternion newRotation = Quaternion.identity; // Default rotation (no rotation)
                    player.transform.SetPositionAndRotation(newPosition, newRotation);
                    itemManager.LossTeleport(loseTeleportItems, loseTeleport);
                    hasTeleported = true;
                } 
            }
        }
        
    }
    public void SwitchSelection(int direction)
    {
        int newSelection = ((int)selectedGrocery + direction) % 3;
        if (newSelection < 0) newSelection += 3;
        selectedGrocery = (GroceryType)newSelection;

        if (itemManager != null && scoreManager != null)
        {
            switch (selectedGrocery)
            {
                case GroceryType.KitchenStuff:
                    selectedKitchen = true;
                    selectedLiving = false;
                    selectedDining = false;
                    break;

                case GroceryType.DiningRoomStuff:

                    selectedKitchen = false;
                    selectedLiving = false;
                    selectedDining = true;
                    break;

                case GroceryType.LivingRoomStuff:

                    selectedKitchen = false;
                    selectedLiving = true;
                    selectedDining = false;
                    break;
            }

        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        pinkGroceryUI.GetComponent< Image>().color = Color.black; // Default color for kitchen
        purpleGroceryUI.GetComponent<Image>().color = Color.black; // Default color for living room
        blueGroceryUI.GetComponent<Image>().color = Color.black; // Default color for dining room

        // Highlight the selected grocery
        switch (selectedGrocery)
        {
            case GroceryType.KitchenStuff:
                pinkGroceryUI.GetComponent<Image>().color = Color.red; // Highlight kitchen
                break;
            case GroceryType.LivingRoomStuff:
                purpleGroceryUI.GetComponent<Image>().color = Color.red; // Highlight living room
                break;
            case GroceryType.DiningRoomStuff:
                blueGroceryUI.GetComponent<Image>().color = Color.red; // Highlight dining room
                break;
        }
    }

    public void SetIsDoor(bool state) //this is used to allow me to check for leftwall in a collsion script
    {
        isDoor = state;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerTwo"))
        {
            canTouch = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerTwo"))
        {
            canTouch = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerTwo"))
        {
            canTouch = false;
        }
    }

    public void SetKitchenFood(bool state)
    {
        kitchenFood = state;
    }

    public void SetDiningFood(bool state)
    {
        diningFood = state;
    }

    public void SetLivingFood(bool state)
    {
        livingFood = state;
    }

    public void SetCanMove(bool state)
    {
        canMove = state;

        if (!state)
        {
            StartCoroutine(CooldownCoroutine());
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(cooldownTime); // Wait for the cooldown time
        canMove = true; // After the wait, allow movement again
    }

}
