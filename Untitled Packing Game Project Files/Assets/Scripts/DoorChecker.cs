using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorChecker : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door")) //if the object does have wall tag, this to ensure it is not wall running
        {
            playerMovement.SetIsDoor(true); //this is set grounded to true meaning the player is grounded
        }

        if (collision.gameObject.CompareTag("Car"))
        {
            playerMovement.SetIsCar(true); //this is set grounded to true meaning the player is grounded
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door")) //if the object does have wall tag, this to ensure it is not wall running
        {
            playerMovement.SetIsDoor(false); //this is set grounded to false meaning the player is grounded
        }

        if (collision.gameObject.CompareTag("Car"))
        {
            playerMovement.SetIsCar(false); //this is set grounded to true meaning the player is grounded
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door")) //if the object does have wall tag, this to ensure it is not wall running
        {
            playerMovement.SetIsDoor(true);  //this is set grounded to true meaning the player is grounded
        }

        if (collision.gameObject.CompareTag("Car"))
        {
            playerMovement.SetIsCar(true); //this is set grounded to true meaning the player is grounded
        }
    }
}
