using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TileSwapPuzzleManager : MonoBehaviour
{
    public List<GameObject> tilePrefabs; // 0~8 순서대로
    public Transform gridParent;
    public GameObject clearPanel;

    private List<GameObject> tiles = new List<GameObject>();
    private GameObject firstSelected = null;

    private Vector2[] tilePositions = new Vector2[9]; // 3x3 그리드 위치 저장

    void Start()
    {
        GenerateGridPositions();
        SpawnTiles();
        clearPanel.SetActive(false);
    }

    void GenerateGridPositions()
    {
        float spacing = 110f;
        float startX = -spacing; // -110f
        float startY = spacing;  // 110f

        int index = 0;
        for (int y = 0; y < 3; y++) // row
        {
            for (int x = 0; x < 3; x++) // col
            {
                float posX = startX + x * spacing;
                float posY = startY - y * spacing;
                tilePositions[index] = new Vector2(posX, posY);
                index++;
            }
        }
    }

    void SpawnTiles()
    {
        List<int> spawnOrder = new List<int>();
        for (int i = 0; i < 9; i++) spawnOrder.Add(i);
        spawnOrder = ShuffleList(spawnOrder);

        for (int i = 0; i < 9; i++)
        {
            int prefabIndex = spawnOrder[i];
            GameObject tile = Instantiate(tilePrefabs[prefabIndex], gridParent);
            tile.transform.localPosition = tilePositions[i];
            tile.transform.localScale = Vector3.one;

            TilePiece piece = tile.GetComponent<TilePiece>();
            piece.correctIndex = prefabIndex; // ex: Tile_2.prefab => 정답 위치는 2번 인덱스

            // 현재 위치 인덱스를 태그처럼 저장해도 되고, 아래에서 위치로 판별할 거임

            Button btn = tile.GetComponent<Button>();
            btn.onClick.AddListener(() => OnTileClicked(tile));

            tiles.Add(tile);
        }
    }

    void OnTileClicked(GameObject selected)
    {
        if (firstSelected == null)
        {
            firstSelected = selected;
            HighlightTile(selected, true);
        }
        else if (firstSelected == selected)
        {
            HighlightTile(selected, false);
            firstSelected = null;
        }
        else
        {
            SwapPositions(firstSelected, selected);
            HighlightTile(firstSelected, false);
            firstSelected = null;

            if (CheckClear())
            {
                clearPanel.SetActive(true);
                Debug.Log("퍼즐 클리어!");

                // ✅ MiniGameManager에게 알리기
                MiniGameManager manager = FindObjectOfType<MiniGameManager>();
                if (manager != null)
                {
                    manager.OnMiniGameClear();
                }
            }
        }
    }

    void SwapPositions(GameObject a, GameObject b)
    {
        Vector3 temp = a.transform.localPosition;
        a.transform.localPosition = b.transform.localPosition;
        b.transform.localPosition = temp;
    }

    void HighlightTile(GameObject tile, bool highlight)
    {
        var img = tile.GetComponent<Image>();
        if (img != null)
            img.color = highlight ? Color.yellow : Color.white;
    }

    bool CheckClear()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            GameObject tile = tiles[i];
            TilePiece piece = tile.GetComponent<TilePiece>();

            // 현재 위치가 정답 위치 인덱스의 위치와 일치하는지 확인
            Vector2 correctPos = tilePositions[piece.correctIndex];
            if (Vector2.Distance(tile.transform.localPosition, correctPos) > 1f)
                return false;
        }
        return true;
    }

    List<int> ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
        return list;
    }
    public void ResetPuzzle()
    {
        foreach (var t in tiles)
            Destroy(t);
        tiles.Clear();

        firstSelected = null;
        clearPanel.SetActive(false);

        SpawnTiles();
    }
    public void ResetGame()
    {
        ResetPuzzle();
    }
}
