using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> resurrectionPoints;
    int currentResurrectionIndex = 0;
    private PlayerMoveManager pmm;
    // Start is called before the first frame update
    void Start()
    {
        pmm = player.GetComponent<PlayerMoveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pmm.Hp <= 0)
        {
            player.SetActive(false);
        }
        if (player.activeSelf == false) // Á×Àº »óÅÂ
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                player.transform.position = resurrectionPoints[currentResurrectionIndex].transform.position;
                pmm.ResetVariable();
                player.SetActive(true);
            }
        }
    }
}
