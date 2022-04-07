using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput1 : MonoBehaviour
{
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        player.directionalInput=new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.OnJumpInputDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.OnJumpInputUp();
        }
    }
}
