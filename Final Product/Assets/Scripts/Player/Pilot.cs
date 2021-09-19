using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pilot : MonoBehaviour
{
    public float speed = 100f;
    private float staticSpeed = 10f;
    public float speedRot = 10f;

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;    

    public Camera playerCamera;
    public Transform barrel = null;
    public Rigidbody2D rb;
    public Animator animator;
    public InventoryObject inventory;

    public LineRenderer lineRenderer;
    public LineRenderer laser;

    int stamina = 100;

    private float zoomSpeed = 0.8f;
    private Vector3 offsetCameraPosition = new Vector3(0, 0, -10);

    private Vector2 moveAmount;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        speed = staticSpeed;
        checkInput();
        fireInput();

        //Test HP
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHealth -= 20;
            healthBar.setHealth(currentHealth);
        }
    }
    private void FixedUpdate()
    {
        updateCamera();
        movementInput();
        mouseLook();

        
    }

    private void checkInput()
    {
        if (Input.GetKeyDown(KeyBinds.INVENTORY_KEY))
        {
            MenuController.menuOn = !MenuController.menuOn;
            print(MenuController.menuOn.ToString());
        }

        if (Input.GetKeyDown(KeyBinds.QUICK_SAVE_KEY))
        {
            inventory.Save();
        }

        if (Input.GetKeyDown(KeyBinds.QUICK_LOAD_KEY))
        {
            inventory.Load();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            animator.SetInteger("weaponId", 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetInteger("weaponId", 1);
        }
    }

    



    private void updateCamera()
    {
        Vector3 relativeMousePosition = transform.position + offsetCameraPosition + (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
        if (!MenuController.menuOn)
            playerCamera.transform.position = Vector3.Lerp(transform.position + offsetCameraPosition, relativeMousePosition + offsetCameraPosition, 0.02f);

        if (playerCamera.orthographicSize >= 7 && !Input.GetKey(KeyCode.Mouse1))
        {
            playerCamera.orthographicSize = Mathf.Lerp(playerCamera.orthographicSize, 7, Time.deltaTime * 2);
        }
    }

    private void mouseLook()
    {
        Vector2 mousePosition = playerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (!MenuController.menuOn)
            rb.SetRotation(Quaternion.Slerp(transform.rotation, rotation, speedRot * Time.deltaTime));
    }

    private void movementInput()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        Vector3 direction = input.normalized;
        Vector3 velocoty = direction * speed;


        moveAmount = (velocoty * Time.fixedDeltaTime) * 5;

        animator.SetFloat("speed", moveAmount.sqrMagnitude);

        

        if (Input.GetKey(KeyBinds.SPRINT_KEY) && stamina > 0)
        {
            stamina = stamina - (int)(1 * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + moveAmount * speed * 2 * Time.fixedDeltaTime);
            //print("Sprinting " + stamina);
        }
        else
        {
            stamina = stamina + (int)(1 * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + moveAmount * speed * Time.fixedDeltaTime);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //print("Lootable");
        var item = collision.GetComponent<GroundItem>();
        if (item)  
        {
            Vector3 mousePosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            if (collision.bounds.Contains(mousePosition))
            {
                
                //Cursor.SetCursor(Texture2D.blackTexture, mousePosition, CursorMode.Auto);
                if (Input.GetKey(KeyBinds.USE_KEY))
                {
                    inventory.AddItem(new Item(item.item), 1);
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    private float fireRate = 0.4f, lastShot = 0;
    private float spread = 0.05f;
    private void fireInput()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            laserRender();
            playerCamera.orthographicSize = Mathf.Lerp(playerCamera.orthographicSize, 9f, Time.deltaTime * zoomSpeed);
            laser.enabled = true;
            speed = 7f;
            if (Input.GetButton("Fire1") && Time.time > fireRate + lastShot)
            {
                animator.SetTrigger("shoot");
                StartCoroutine(Shoot());
                lastShot = Time.time;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            laser.enabled = false;
        }
    }

    IEnumerator Shoot()
    {
        Vector3 direction = new Vector2();
        direction.x = Random.Range(-spread, spread);
        direction.y = Random.Range(-spread, spread);
        direction.z = 0;


        RaycastHit2D hitInfo = Physics2D.Raycast(barrel.position, barrel.right + direction);


        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            lineRenderer.SetPosition(0, barrel.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            lineRenderer.SetPosition(0, barrel.position);
            lineRenderer.SetPosition(1, barrel.position + barrel.right * 100);
        }
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.05f);

        lineRenderer.enabled = false;
    }

    private void laserRender()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(barrel.transform.position, barrel.transform.right);

        if (hitInfo)
        {
            laser.SetPosition(0, barrel.transform.position);
            laser.SetPosition(1, hitInfo.point);
        }
        else
        {
            laser.SetPosition(0, barrel.transform.position);
            laser.SetPosition(1, barrel.transform.position + barrel.transform.right * 100);
        }

    }


    private void OnApplicationQuit()
    {
        inventory.container.Items = new InventorySlot[24];
    }

}
