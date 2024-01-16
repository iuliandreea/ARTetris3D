using UnityEngine;
using UnityEngine.Windows;

public class Playfield : MonoBehaviour
{
    public static Playfield instance;

    public int gridSizeX, gridSizeY, gridSizeZ;

    [Header("Blocks")]
    public GameObject[] blockList;
    
    [Header("Playfield visuals")]
    public GameObject bottomPlane;
    public GameObject nPlane, sPlane, wPlane, ePlane;

    int randomIndex;


    public Transform[,,] grid;

    void Awake()
    {
        Debug.Log("Instantiated");
        instance = this;
    }

    void Start()
    {
        grid = new Transform[gridSizeX, gridSizeY, gridSizeZ];
        CalculatePreview();
        Previewer.instance.ShowPreview(randomIndex);
        //SpawnFixedBlock();
        //Debug.Log("uwu");
        SpawnNewBlock(Previewer.detectedBlockIndex);
    }

    public Vector3 Round(Vector3 vec)
    {
        return new Vector3(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), Mathf.RoundToInt(vec.z));
    }

    int Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        float val = toMin + ((toMax - toMin) / (fromMax - fromMin)) * (from - fromMin);
        return Mathf.RoundToInt(val);
    }

    public bool CheckIfInsidePlayfield(Vector3 position)
    {
        return position.x <= ePlane.transform.position.x && position.x >= wPlane.transform.position.x &&
               position.y >= bottomPlane.transform.position.y &&
               position.z <= nPlane.transform.position.z && position.z >= sPlane.transform.position.z;
    }

    public void UpdateGrid(TetrisBlock tBlock)
    {
        for (int x = 0; x < gridSizeX; x++) 
            for (int z = 0; z < gridSizeZ; z++)
                for (int y = 0; y < gridSizeY; y++)
                    if (grid[x, y, z] != null)
                        if (grid[x, y, z].parent == tBlock.transform) 
                            grid[x, y, z] = null;

        foreach(Transform child in tBlock.transform) 
        {
            Vector3 pos = child.position;
            if (pos.y < transform.position.y + gridSizeY)
            {
                int x = Remap(pos.x, wPlane.transform.position.x + 0.5f, ePlane.transform.position.x - 0.5f, 0f, gridSizeX - 1);
                int y = Remap(pos.y, bottomPlane.transform.position.y + 0.5f, bottomPlane.transform.position.y + gridSizeY - 0.5f, 0f, gridSizeY - 1);
                int z = Remap(pos.z, sPlane.transform.position.z + 0.5f, nPlane.transform.position.z - 0.5f, 0f, gridSizeZ - 1);

                grid[x, y, z] = child;
            }
                
        }
    }

    public Transform GetTransformOnGridPosition(Vector3 pos) 
    {
        if (pos.y > transform.position.y + gridSizeY) 
            return null;
        int x = Remap(pos.x, wPlane.transform.position.x + 0.5f, ePlane.transform.position.x - 0.5f, 0f, gridSizeX - 1);
        int y = Remap(pos.y, bottomPlane.transform.position.y + 0.5f, bottomPlane.transform.position.y + gridSizeY - 0.5f, 0f, gridSizeY - 1);
        int z = Remap(pos.z, sPlane.transform.position.z + 0.5f, nPlane.transform.position.z - 0.5f, 0f, gridSizeZ - 1);
        return grid[x, y, z];
    }

    public void SpawnNewBlock(int index = -1) 
    {
        bool isRandom = (index == -1);
        if (isRandom)
            index = randomIndex;

        Vector3 spawnPoint = new Vector3(transform.position.x + 0.5f,
                                         transform.position.y + gridSizeY + 0.5f,
                                         transform.position.z + gridSizeZ - 0.5f);

        // spawn
        GameObject newBlock = Instantiate(blockList[index], spawnPoint, Quaternion.identity, transform.parent) as GameObject;
        newBlock.transform.rotation = transform.parent.rotation;
        if (isRandom)
        {
            CalculatePreview();
            Previewer.instance.ShowPreview(randomIndex);
        }
        // ghost
    }

    public void CalculatePreview()
    {
        randomIndex = Random.Range(0, blockList.Length);
    }

    public void DeleteLayer()
    {
        int layersCleared = 0;
        for (int y = 0; y < gridSizeY; y++)
        {
            if (CheckEmptyLayer(y))
                break;
            if (CheckFullLayer(y))
            {
                DeleteLayerAt(y);
                moveAllLayerDown(y + 1);
                y--;
                layersCleared++;
            }
        }

        if (layersCleared > 0)
            GameManager.instance.SetScore(layersCleared);
    }

    bool CheckFullLayer(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
            for (int z = 0; z < gridSizeZ; z++)
                if (grid[x, y, z] == null)
                    return false;
        return true;
    }

    bool CheckEmptyLayer(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
            for (int z = 0; z < gridSizeZ; z++)
                if (grid[x, y, z] != null)
                    return false;
        return true;
    }

    void DeleteLayerAt(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Destroy(grid[x, y, z].gameObject);
                grid[x, y, z] = null;
            }
        }
    }

    void moveAllLayerDown(int y)
    {
        for (int i = y; i < gridSizeY; i++)
        {
            moveOneLayerDown(i);
        }

    }

    void moveOneLayerDown(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if (grid[x, y, z] != null)
                {
                    grid[x, y - 1, z] = grid[x, y, z];
                    grid[x, y, z] = null;
                    grid[x, y - 1, z].position += Vector3.down; 
                }
            }
        }
    }

    void OnDrawGizmos() 
    {
        if(bottomPlane != null)
        {
            // Resize bottom plane
            Vector3 scaler = new Vector3((float) gridSizeX / 10, 1, (float) gridSizeZ / 10);
            bottomPlane.transform.localScale = scaler;
            // Reposition bottom plane
            bottomPlane.transform.position = new Vector3(transform.position.x + (float) gridSizeX / 2, 
                                                         transform.position.y, 
                                                         transform.position.z + (float) gridSizeZ / 2);
            // Retile material
            bottomPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeZ);
        }
        if(nPlane != null)
        {
            // Resize bottom plane
            Vector3 scaler = new Vector3((float) gridSizeX / 10, 1, (float) gridSizeY / 10);
            nPlane.transform.localScale = scaler;
            // Reposition bottom plane
            nPlane.transform.position = new Vector3(transform.position.x + (float) gridSizeX / 2, 
                                                    transform.position.y + (float) gridSizeY / 2, 
                                                    transform.position.z + gridSizeZ);
            // Retile material
            nPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }
        if(sPlane != null)
        {
            // Resize bottom plane
            Vector3 scaler = new Vector3((float) gridSizeX / 10, 1, (float) gridSizeY / 10);
            sPlane.transform.localScale = scaler;
            // Reposition bottom plane
            sPlane.transform.position = new Vector3(transform.position.x + (float) gridSizeX / 2, 
                                                    transform.position.y + (float) gridSizeY / 2, 
                                                    transform.position.z);
        }
        if(wPlane != null)
        {
            // Resize bottom plane
            Vector3 scaler = new Vector3((float) gridSizeZ / 10, 1, (float) gridSizeY / 10);
            wPlane.transform.localScale = scaler;
            // Reposition bottom plane
            wPlane.transform.position = new Vector3(transform.position.x, 
                                                    transform.position.y + (float) gridSizeY / 2, 
                                                    transform.position.z + (float) gridSizeZ / 2);
            // Retile material
            wPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }
        if(ePlane != null)
        {
            // Resize bottom plane
            Vector3 scaler = new Vector3((float) gridSizeZ / 10, 1, (float) gridSizeY / 10);
            ePlane.transform.localScale = scaler;
            // Reposition bottom plane
            ePlane.transform.position = new Vector3(transform.position.x + gridSizeX, 
                                                    transform.position.y + (float) gridSizeY / 2, 
                                                    transform.position.z + (float) gridSizeZ / 2);
        }
    }
}
