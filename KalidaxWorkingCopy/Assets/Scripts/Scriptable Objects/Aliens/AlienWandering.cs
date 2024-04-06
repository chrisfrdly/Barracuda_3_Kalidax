using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienWandering : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f; // how fast the alien is moving
    private float currentSpeed;
    [SerializeField] private float waitTime = 5f; //how long before the aliens stop standing still or moving
    private WorldAlien alienScript;
    private SO_Alien alien;
    private AlienBorders borders;

    //borders that will be put grabbed from the alienBorders script
    private float m_leftBorders;
    private float m_rightBorders;
    private float m_topBorders;
    private float m_bottomBorders;

    private void Awake()
    {
        WorldAlien alienScript = GetComponent<WorldAlien>();
        SO_Alien alien = alienScript.m_AlienContainer;
        ReturnBordersScripts(alien);
        GetBorders(borders);
    }
    // Update is called once per frame
    void Update()
    {
        if(alienScript.isBeingSold)
        {
            StopMoving();
        }
    }

    //grabs the borders from the gameObjects in the scene
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

    private void GetBorders(AlienBorders borders)
    {
        m_leftBorders = borders.leftBorders;
        m_rightBorders = borders.rightBorders;
        m_bottomBorders = borders.bottomBorders;
        m_topBorders = borders.topBorders;
    }

    private void StopMoving()
    {
        currentSpeed = 0;
    }
}
