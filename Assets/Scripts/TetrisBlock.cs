using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    // Where the last block has fallen down once
    float prevTime;
    // How long it takes until it takes the block to fall down
    float fallTime = 1f;

    // Start is called before the first frame update
    void Start() 
    {
        ButtonInputs.instance.SetActiveBlock(gameObject, this);
        transform.position += Vector3.down;
        if (!CheckValidMove())
            GameManager.instance.setGameOver();
        transform.position += Vector3.up;
    }

    // Update is called once per frame
    void Update() 
    {
        if (UIHandler.instance.GetIsPaused())
            return;

        if (UIHandler.instance.GetDrop())
            fallTime = 0.2f;

        if(Time.time - prevTime > fallTime) 
        {
            transform.position += Vector3.down;
            if (CheckValidMove())
            {
                // Update the grid
                Playfield.instance.UpdateGrid(this);
            }
            else
            {
                transform.position += Vector3.up;
                Playfield.instance.DeleteLayer();
                enabled = false;

                fallTime = 1f;
                UIHandler.instance.Drop(false);

                if(!GameManager.instance.ReadGameOver())
                    Playfield.instance.SpawnNewBlock();
            }
            prevTime = Time.time;
        }

    }

    public void SetInput(Vector3 direction)
    {
        transform.position += direction;
        if (!CheckValidMove())
            transform.position -= direction;
        else
            Playfield.instance.UpdateGrid(this);
    }

    public void SetRotationInput(Vector3 rotation)
    {
        transform.Rotate(rotation, Space.World);
        if (!CheckValidMove())
            transform.Rotate(-rotation, Space.World);
        else
            Playfield.instance.UpdateGrid(this);
    }

    bool CheckValidMove()
    {
        foreach(Transform child in transform) 
        {
            Vector3 pos = child.position;
            if (!Playfield.instance.CheckIfInsidePlayfield(pos))
                return false;
        }
        
        foreach(Transform child in transform)
        {
            Vector3 pos = child.position;
            Transform t = Playfield.instance.GetTransformOnGridPosition(pos);
            if (t != null && t.parent != transform)
                return false;
        }
        return true;
    }
}
