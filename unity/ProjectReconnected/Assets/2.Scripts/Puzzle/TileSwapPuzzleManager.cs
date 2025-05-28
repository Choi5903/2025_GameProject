using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSwapPuzzleManager : MiniGameBase
{
    public List<GameObject> tilePrefabs;
    public Transform gridParent;

    private List<GameObject> tiles = new List<GameObject>();
    private GameObject firstSelected = null;
    private Vector2[] tilePositions = new Vector2[9];

    private bool isCleared = false;

    void OnEnable()
    {
        ResetGame(); // 퍼즐 활성화 시 자동 초기화
    }

    void GenerateGridPositions()
    {
        float tileSize = 100f;
        int index = 0;
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                float posX = x * tileSize;
                float posY = -y * tileSize;
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

            RectTransform rect = tile.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchorMin = rect.anchorMax = new Vector2(0, 1);
                rect.pivot = new Vector2(0, 1);
            }

            TilePiece piece = tile.GetComponent<TilePiece>();
            piece.correctIndex = prefabIndex;

            Button btn = tile.GetComponent<Button>();
            btn.onClick.AddListener(() => OnTileClicked(tile));

            tiles.Add(tile);
        }
    }

    void OnTileClicked(GameObject selected)
    {
        if (isCleared) return;

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
        }
    }

    void HighlightTile(GameObject tile, bool highlight)
    {
        var img = tile.GetComponent<Image>();
        if (img != null)
            img.color = highlight ? Color.yellow : Color.white;
    }

    void SwapPositions(GameObject a, GameObject b)
    {
        Vector3 temp = a.transform.localPosition;
        a.transform.localPosition = b.transform.localPosition;
        b.transform.localPosition = temp;

        // 위치 바뀌었으니 클리어 체크
        if (!isCleared && CheckClear())
        {
            isCleared = true;
            Debug.Log("✅ 정답입니다! 퍼즐 클리어!");
            NotifyClear();
        }
    }


    bool CheckClear()
    {
        // 타일들을 localPosition 기준으로 정렬 (왼→오, 위→아래)
        List<GameObject> sortedTiles = new List<GameObject>(tiles);
        sortedTiles.Sort((a, b) =>
        {
            Vector2 posA = a.transform.localPosition;
            Vector2 posB = b.transform.localPosition;

            if (Mathf.Abs(posA.y - posB.y) > 1f)
                return posA.y > posB.y ? -1 : 1;
            else
                return posA.x < posB.x ? -1 : 1;
        });

        for (int i = 0; i < sortedTiles.Count; i++)
        {
            TilePiece piece = sortedTiles[i].GetComponent<TilePiece>();
            if (piece.correctIndex != i)
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
        isCleared = false;
        SpawnTiles();
    }

    public override void ResetGame()
    {
        GenerateGridPositions();
        ResetPuzzle();
    }

    void DebugTilePositions()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            var tile = tiles[i];
            Debug.Log($"[Tile {i}] Pos: {tile.transform.localPosition}, Correct: {tile.GetComponent<TilePiece>().correctIndex}");
        }
    }
}
