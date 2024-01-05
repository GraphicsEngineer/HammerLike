using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMapGenerator : MonoBehaviour
{
    public GameObject startRoomPrefab;
    public GameObject eliteRoomPrefab;
    public GameObject bossRoomPrefab;

    public List<GameObject> normalRoomPrefabs; // NormalRoom ������ ����Ʈ

    public int normalRoomCount = 5; // ������ NormalRoom�� ��
    private int remainingNormalRooms; // ���� �������� ���� NormalRoom�� ��

    private List<GameObject> roomObjects; // �� GameObject ����Ʈ

    public LineRenderer lineRenderer; // LineRenderer ������Ʈ

    public int maxXSize = 1000;
    public int maxYSize = 1000;
    public int divisionCount = 5;
    private bool hasPlacedStartRoom = false;
    private bool hasPlacedEliteRoom = false;
    private bool hasPlacedBossRoom = false;
    private List<BSPNode> leafNodes;

    private class BSPNode
    {
        public BSPNode left;
        public BSPNode right;
        public Rect room;
    }

    void Start()
    {
        remainingNormalRooms = normalRoomCount;
        leafNodes = new List<BSPNode>();
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        BSPNode rootNode = new BSPNode
        {
            room = new Rect(0, 0, maxXSize, maxYSize) // ���� ũ��, ���� ���ӿ� �°� ���� �ʿ�
        };

        SplitNode(rootNode, divisionCount);
        PlaceRooms();
    }

    void SplitNode(BSPNode node, int depth)
    {
        if (depth <= 0)
        {
            leafNodes.Add(node); // ���� ��带 ����Ʈ�� �߰�
            return;
        }

        // ���� �Ǵ� ���� ������ �������� ����
        bool splitH = Random.Range(0, 2) == 0;

        if (splitH)
        {
            // ���� ����
            float splitPos = Random.Range(0.4f, 0.6f); // ���� ��ġ ����
            node.left = new BSPNode
            {
                room = new Rect(node.room.x, node.room.y, node.room.width, node.room.height * splitPos)
            };
            node.right = new BSPNode
            {
                room = new Rect(node.room.x, node.room.y + node.room.height * splitPos, node.room.width, node.room.height * (1 - splitPos))
            };
        }
        else
        {
            // ���� ����
            float splitPos = Random.Range(0.4f, 0.6f); // ���� ��ġ ����
            node.left = new BSPNode
            {
                room = new Rect(node.room.x, node.room.y, node.room.width * splitPos, node.room.height)
            };
            node.right = new BSPNode
            {
                room = new Rect(node.room.x + node.room.width * splitPos, node.room.y, node.room.width * (1 - splitPos), node.room.height)
            };
        }

        SplitNode(node.left, depth - 1);
        SplitNode(node.right, depth - 1);
    }

    void PlaceRooms()
    {
        roomObjects = new List<GameObject>();

        GameObject startRoom = null;
        GameObject bossRoom = null;
        List<GameObject> otherRooms = new List<GameObject>();

        foreach (var node in leafNodes)
        {
            GameObject roomPrefab = ChooseRoomPrefab();
            if (roomPrefab != null) // null�� �ƴ� ���� ���� ����
            {
                GameObject room = Instantiate(roomPrefab, new Vector3(node.room.x, 0, node.room.y), Quaternion.identity);

                if (roomPrefab == startRoomPrefab)
                    startRoom = room;
                else if (roomPrefab == bossRoomPrefab)
                    bossRoom = room;
                else
                    otherRooms.Add(room);
            }
        }

        // ������� �� ��ġ: StartRoom -> ������ ��� -> BossRoom
        if (startRoom != null)
            roomObjects.Add(startRoom);
        roomObjects.AddRange(otherRooms);
        if (bossRoom != null)
            roomObjects.Add(bossRoom);

        CreateCorridors();
    }

    void CreateCorridors()
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < roomObjects.Count - 1; i++)
        {
            Vector3 start = roomObjects[i].transform.position;
            Vector3 end = roomObjects[i + 1].transform.position;

            // L�� ��� ��� ����
            points.Add(start);
            points.Add(new Vector3(start.x, start.y, end.z));
            points.Add(end);
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }


    GameObject ChooseRoomPrefab()
    {
        if (!hasPlacedStartRoom)
        {
            hasPlacedStartRoom = true;
            return startRoomPrefab;
        }
        else if (remainingNormalRooms > 0)
        {
            remainingNormalRooms--;
            return normalRoomPrefabs[Random.Range(0, normalRoomPrefabs.Count)];
        }
        else if (!hasPlacedEliteRoom)
        {
            hasPlacedEliteRoom = true;
            return eliteRoomPrefab;
        }
        else if (!hasPlacedBossRoom)
        {
            hasPlacedBossRoom = true;
            return bossRoomPrefab;
        }
        else
        {
            // ��� Ư�� ��� �ʿ��� ���� NormalRoom�� ��ġ�� ��
            // ���⼭�� null�� ��ȯ�ϰų� �ٸ� ������ ���� ��ȯ�� �� �ֽ��ϴ�.
            return null; // �Ǵ� �ٸ� �� ������ ������ ��ȯ
        }
    }

}
