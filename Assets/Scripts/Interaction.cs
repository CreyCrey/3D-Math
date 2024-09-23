using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Transform player;  // Reference to the player object
    public Renderer plantRenderer;  // Renderer of the plant to change color

    private void Start()
    {
        // Get the Renderer component to change the plants color
        if (plantRenderer == null)
        {
            plantRenderer = GetComponent<Renderer>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            // Get the height of the objects collider
            Collider plantCollider = GetComponent<Collider>();
            float plantTopY = transform.position.y + (plantCollider.bounds.size.y / 2);

            // Check if the player is above the top of the plant
            if (player.position.y > plantTopY)
            {
                TurnGreen();  // Turn green when colliding from above
            }
            else
            {
                TurnBlue();
            }
        }
    }

    // Method to turn the plant green
    private void TurnGreen()
    {
        ChangeColor(gameObject, Color.green);

        // Change the color of all child objects
        foreach (Transform child in transform)
        {
            ChangeColor(child.gameObject, Color.green);
        }
    }
    private void TurnBlue()
    {
        ChangeColor(gameObject, Color.blue);

        foreach (Transform child in transform)
        {
            ChangeColor(child.gameObject, Color.blue);
        }
    }
    void ChangeColor(GameObject obj, Color color)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }
}
