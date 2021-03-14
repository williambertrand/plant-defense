using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GridDirection {
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public class TopDownCharacterController : GridObject
{

    public float speed;
    public GridDirection direction;
    public Rigidbody2D rb;

    public GameObject focusPrefab;
    private SpriteRenderer focusSpriteRenderer;
    public Point? focusPoint;

    private Animator animator;
    private SpriteRenderer renderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        focusSpriteRenderer = focusPrefab.gameObject.GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            animator.SetInteger("Direction", 3);
            direction = GridDirection.LEFT;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            animator.SetInteger("Direction", 2);
            direction = GridDirection.RIGHT;
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            animator.SetInteger("Direction", 1);
            direction = GridDirection.UP;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            animator.SetInteger("Direction", 0);
            direction = GridDirection.DOWN;
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        rb.velocity = speed * dir;

        // Grid Object method
        UpdatePosition();
        UpdateFocusPosition();

        // Higher y = lower sorting order

        if (transform.position.y > 0)
        {
            renderer.sortingOrder = - (int) (2 * transform.position.y);
        }
        else
        {
            renderer.sortingOrder = 5 - (int)transform.position.y;
        }
    }

    private void UpdateFocusPosition()
    {
        Point pos = new Point(gridPositon);
        switch (direction)
        {
            case GridDirection.LEFT:
                pos = new Point(gridPositon.x - 1, gridPositon.y);
                break;
            case GridDirection.RIGHT:
                pos = new Point(gridPositon.x + 1, gridPositon.y);
                break;
            case GridDirection.UP:
                pos = new Point(gridPositon.x, gridPositon.y + 1);
                break;
            case GridDirection.DOWN:
                pos = new Point(gridPositon.x, gridPositon.y - 1);
                break;
        }

        if(GameGrid.Instance.CheckBounds(pos))
        {
            focusPrefab.SetActive(true);
            focusPrefab.transform.position = GameGrid.Instance.PosToWorldLocation(pos);
            focusPoint = pos;

            // Update info view with plant at this location
            if (PlantManager.Instance.PlantExistis(pos))
            {
                Plant focused = PlantManager.Instance.plants[pos];
                PlantMenuSystem.Instance.OnPlantHighlighted(focused);
                focusSpriteRenderer.color = new Color(0.8f, 0.5f, 0.9f);
            }
            else
            {
                PlantMenuSystem.Instance.OnPlantHighlighted(null);
                focusSpriteRenderer.color = Color.white;
            }
        }
        else
        {
            PlantMenuSystem.Instance.OnPlantHighlighted(null);  
            focusPrefab.SetActive(false);
            focusPoint = null;
        }
    }
}

