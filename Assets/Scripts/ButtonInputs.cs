using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputs : MonoBehaviour
{
    public static ButtonInputs instance;
    public GameObject[] rotateCanvases;
    public GameObject[] moveCanvases;

    GameObject activeBlock;
    TetrisBlock activeTetris;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void SetActiveBlock(GameObject block, TetrisBlock tetris)
    {
        activeBlock = block;
        activeTetris = tetris;
    }

    public void MoveBlock(string direction)
    {
        if (activeBlock != null)
        {
            if (direction == "left")
                activeTetris.SetInput(Vector3.left);
            else if (direction == "right")
                activeTetris.SetInput(Vector3.right);
            else if (direction == "forward")
                activeTetris.SetInput(Vector3.forward);
            else if (direction == "back")
                activeTetris.SetInput(Vector3.back);
        }
    }

    public void RotateBlock(string rotation)
    {
        if (activeBlock != null)
        {
            // x rotation
            if (rotation == "posX")
                activeTetris.SetRotationInput(new Vector3(90, 0, 0));
            else if (rotation == "negX")
                activeTetris.SetRotationInput(new Vector3(-90, 0, 0));
            // y rotation
            else if (rotation == "posY")
                activeTetris.SetRotationInput(new Vector3(0, 90, 0));
            else if (rotation == "negY")
                activeTetris.SetRotationInput(new Vector3(0, -90, 0));
            // z rotation
            else if (rotation == "posZ")
                activeTetris.SetRotationInput(new Vector3(0, 0, 90));
            else if (rotation == "negZ")
                activeTetris.SetRotationInput(new Vector3(0, 0, -90));
        }
    }
}
