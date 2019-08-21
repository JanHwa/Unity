using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    #region Singleton
    private static Player _instance;
    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Player();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    #endregion

    public delegate void ChangeEnemyColor(Color color);
    public static event ChangeEnemyColor onEnemyHit;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(onEnemyHit != null)
            {

                onEnemyHit(Color.red);
            }
        }
        
    }
}
