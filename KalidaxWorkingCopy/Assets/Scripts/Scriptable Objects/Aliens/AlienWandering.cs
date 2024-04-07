using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AlienWandering : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f; // how fast the alien is moving
    private float currentSpeed;
    [SerializeField] private float waitTime = 5f; //how long before the aliens stop standing still or moving
    [SerializeField] private float changeDestinationRange = 0.5f;
    float timer;
    private WorldAlien alienScript;
    private AlienBorders borders;

    private Vector2 borderMidpointCoordinates;
    private Vector2 destination;

    private bool canMove = true;

    //borders that will be put grabbed from the alienBorders script
    private float m_leftBorders;
    private float m_rightBorders;
    private float m_topBorders;
    private float m_bottomBorders;

    //Animating Movement
    [SerializeField] string previouslyDirection;
    
    private int tweenScaleX = 0;
    private string directionFacing = "";


    //reference to the dialogue script
    private DS_InteractableObject_InteractPointConversation conversation;

    private void Awake()
    {
        alienScript = GetComponent<WorldAlien>();
        conversation = GetComponent<DS_InteractableObject_InteractPointConversation>();
        SO_Alien alien = alienScript.m_AlienContainer;
        ReturnBordersScripts(alien);
        GetBorders(borders);
        borderMidpointCoordinates = GetMidpoint();
        destination = borderMidpointCoordinates;
        StartMoving();
        timer = waitTime;

    }

    private void Start()
    {
        transform.GetChild(0).LeanScaleY(0.95f, 0.8f).setEaseInOutSine().setLoopPingPong();
        transform.GetChild(0).LeanMoveLocalY(-0.05f, 0.8f).setEaseInOutSine().setLoopPingPong();
    }
    private void FixedUpdate()
    {
        if (alienScript.isBeingSold)
        {
            canMove = false;
        }
        if(conversation.m_IsTalkingTo)
        {
            StopMoving();
        }
        else if(!conversation.m_IsTalkingTo && canMove)
        {
            StartMoving();
        }
        if (!IsWithinBorders())
        {
            destination = borderMidpointCoordinates;
        }

        MoveAlien();
    }

    //grabs the borders from the gameObjects in the scene
    //this is based on which scriptable object is attatched to the gameObject
    private void ReturnBordersScripts(SO_Alien alien)
    {

        if(alien.m_AlienFamily == AlienFamilyType.Sprogs)
        {
            GameObject go = GameObject.Find("Sprog Borders");
            borders = go.GetComponent<AlienBorders>();
        }
        else if(alien.m_AlienFamily == AlienFamilyType.LongStriders)
        {
            GameObject go = GameObject.Find("Strider Borders");
            borders = go.GetComponent<AlienBorders>(); 
        }
        else
        {
            Debug.Log("No Aliens Available");
        }
   
    }

    //this sets a random desit
    private void SetRandomDestination()
    {
        destination = new Vector2(Random.Range(m_leftBorders,m_rightBorders), Random.Range(m_bottomBorders, m_topBorders));
        StartMoving();
    }

    //this moves the alien around
    private void MoveAlien()
    {
        
        transform.position = Vector2.MoveTowards(transform.position,destination, currentSpeed *Time.deltaTime);
        if (Vector2.Distance(transform.position, destination) < changeDestinationRange)
        {
            StopMoving();
            timer -= Time.deltaTime; //scuffed ass delay, get ready for some spaghetti code 
            if (timer <= 0)
            {
                SetRandomDestination();
                timer = waitTime;
            }
                
                
        }
        
       

        FlipAlienSprite();
    }

    //returns the conditions on when the alien is allowed to move
    public bool IsWithinBorders()
    {
        if(transform.position.x > m_leftBorders && transform.position.x < m_rightBorders)
        {
            if(transform.position.y > m_bottomBorders && transform.position.y < m_topBorders)
                return true;
        }

        return false;
    }

    //grabs the borders from the retrieved borders script
    private void GetBorders(AlienBorders borders)
    {
        m_leftBorders = borders.leftBorders;
        m_rightBorders = borders.rightBorders;
        m_bottomBorders = borders.bottomBorders;
        m_topBorders = borders.topBorders;
    }

    //this finds the midpoint of the rectangle. This destination will be called if the
    private Vector2 GetMidpoint()
    {
        float xMidpoint = (m_leftBorders + m_rightBorders)/2;
        float yMidpoint = (m_bottomBorders + m_topBorders)/2;

        Vector2 midpoint = new Vector3(xMidpoint, yMidpoint,1f);

        return midpoint;
    }
    //this will keep our alien from moving around
    private void StopMoving()
    {
        currentSpeed = 0;
      
    }
    //this will be called whenever our alien can move again
    private void StartMoving()
    {
        currentSpeed = maxSpeed;
       
    }

    private void FlipAlienSprite()
    {
       
        //if the alien's x position is less than the target's x
        //Ik it's spaghetti code :(

        //Getting the direction facing
        if(transform.position.x < destination.x)
        {
            directionFacing = "right";
            
        }
        else if(transform.position.x > destination.x)
        {
            directionFacing = "left";
        }

        //Setting which X we should scale to
        if (directionFacing == "right")
        {
            if (previouslyDirection == "right") return;
            tweenScaleX = -1;
        }
        else if(directionFacing == "left")
        {
            if (previouslyDirection == "left") return;
            tweenScaleX = 1;
        }

        //Lerping the scale
        transform.GetChild(0).transform.LeanScaleX(tweenScaleX, 0.3f);
        
        //Setting previous direction
        if (directionFacing == "right" || directionFacing == "left")
        {
            previouslyDirection = directionFacing;

        }
      
    }
}
