using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector3 position;
    public enum RoomType { Start, End, MiddleBoss, Reward, Normal, Hidden }
    public RoomType roomType;
    public int roomNumber; // �� ��ȣ
    public List<Room> connectedRooms = new List<Room>();

    // ��� ������ ��Ÿ���� ������
    public enum Direction { North, East, West, South }
    public List<Direction> corridorDirections = new List<Direction>(); // ����� ����� �����
}

public class MapGenerator : MonoBehaviour
{
    public GameObject startRoomPrefab, endRoomPrefab, middleBossRoomPrefab, rewardRoomPrefab, normalRoomPrefab, hiddenRoomPrefab;
    public int numberOfNormalRooms = 5;
    public float hiddenRoomProbability = 0.2f;
    public Vector3 dungeonMaxSize = new Vector3(50, 0, 50);

    private LineRenderer lineRendererPrefab;
    private List<Room> rooms = new List<Room>();
    private List<LineRenderer> corridors = new List<LineRenderer>();
    private Dictionary<Room, GameObject> roomObjects = new Dictionary<Room, GameObject>();

    public float roomSeparationMargin = 2.0f;


    private void Awake()
    {
        lineRendererPrefab = new GameObject("Corridor").AddComponent<LineRenderer>();
        lineRendererPrefab.gameObject.SetActive(false);
    }

    public void GenerateMap()
    {
        bool isOverlapping;
        int maxAttempts = 1000;
        int attemptCount = 0;

        do
        {
            rooms.Clear();
            isOverlapping = false;

            // Start Room ����
            CreateRoom(startRoomPrefab, Room.RoomType.Start, Vector3.zero);

            // Normal Rooms ����
            for (int i = 0; i < numberOfNormalRooms; i++)
            {
                Vector3 newPosition = GetRandomNonOverlappingPosition(normalRoomPrefab);
                CreateRoom(normalRoomPrefab, Room.RoomType.Normal, newPosition);
                if (CheckOverlap(newPosition, normalRoomPrefab))
                {
                    isOverlapping = true;
                    break;
                }
            }

            if (!isOverlapping)
            {
                // MiddleBossRoom ����
                Vector3 middleBossPosition = GetRandomNonOverlappingPosition(middleBossRoomPrefab, true);
                CreateRoom(middleBossRoomPrefab, Room.RoomType.MiddleBoss, middleBossPosition);
                if (CheckOverlap(middleBossPosition, middleBossRoomPrefab))
                {
                    isOverlapping = true;
                }
            }

            if (!isOverlapping)
            {
                // RewardRoom ����
                Vector3 rewardPosition = GetRandomNonOverlappingPosition(rewardRoomPrefab, true);
                CreateRoom(rewardRoomPrefab, Room.RoomType.Reward, rewardPosition);
                if (CheckOverlap(rewardPosition, rewardRoomPrefab))
                {
                    isOverlapping = true;
                }
            }

            if (!isOverlapping)
            {
                // End Room ����
                Vector3 endRoomPosition = new Vector3(dungeonMaxSize.x - endRoomPrefab.GetComponent<Renderer>().bounds.size.x,
                                                      0,
                                                      dungeonMaxSize.z - endRoomPrefab.GetComponent<Renderer>().bounds.size.z);
                CreateRoom(endRoomPrefab, Room.RoomType.End, endRoomPosition);
                if (CheckOverlap(endRoomPosition, endRoomPrefab))
                {
                    isOverlapping = true;
                }
            }

            attemptCount++;
        } while (isOverlapping && attemptCount < maxAttempts);

        if (attemptCount == maxAttempts)
        {
            Debug.LogError("Failed to generate a non-overlapping map. Consider decreasing the number of rooms.");
        }
        else
        {
            CreateCorridors();
        }
    }

    private bool CheckOverlap(Vector3 position, GameObject roomPrefab)
    {
        Renderer newRoomRenderer = roomPrefab.GetComponent<Renderer>();
        Vector3 newRoomSize = newRoomRenderer != null ? newRoomRenderer.bounds.size : new Vector3(10, 0, 10);

        foreach (var room in rooms)
        {
            Renderer existingRoomRenderer = roomObjects[room].GetComponent<Renderer>();
            Vector3 existingRoomSize = existingRoomRenderer != null ? existingRoomRenderer.bounds.size : new Vector3(10, 0, 10);

            bool overlapX = position.x < room.position.x + existingRoomSize.x && position.x + newRoomSize.x > room.position.x;
            bool overlapZ = position.z < room.position.z + existingRoomSize.z && position.z + newRoomSize.z > room.position.z;

            if (overlapX && overlapZ)
            {
                return true;
            }
        }
        return false;
    }


    private void CreateRoom(GameObject roomPrefab, Room.RoomType roomType, Vector3 position)
    {
        Room room = new Room
        {
            position = position,
            roomType = roomType
        };

        GameObject roomObj = Instantiate(roomPrefab, room.position, Quaternion.identity);
        roomObjects.Add(room, roomObj); // ������ GameObject ����
        rooms.Add(room);
    }

    private Vector3 GetRandomNonOverlappingPosition(GameObject roomPrefab, bool isSpecialRoom = false)
    {
        Vector3 position;
        Renderer roomRenderer = roomPrefab.GetComponent<Renderer>();
        Vector3 roomSize = roomRenderer != null ? roomRenderer.bounds.size : new Vector3(10, 0, 10);

        int maxAttempts = 100;
        for (int attempts = 0; attempts < maxAttempts; attempts++)
        {
            position = new Vector3(
            Random.Range(0, dungeonMaxSize.x - roomSize.x - roomSeparationMargin),
            0,
            Random.Range(0, dungeonMaxSize.z - roomSize.z - roomSeparationMargin));

            if (!IsOverlapping(position, roomSize))
            {
                return position;
            }
        }

        throw new System.Exception("Failed to find non-overlapping position. Increase dungeon size or decrease number of rooms.");
    }

    private bool IsOverlapping(Vector3 position, Vector3 size)
    {
        foreach (var room in rooms)
        {
            Renderer existingRoomRenderer = roomObjects[room].GetComponent<Renderer>();
            Vector3 existingRoomSize = existingRoomRenderer != null ? existingRoomRenderer.bounds.size : new Vector3(10, 0, 10);

            // ������ ����Ͽ� ��ħ ���θ� Ȯ��
            bool overlapX = position.x < room.position.x + existingRoomSize.x + roomSeparationMargin &&
                            position.x + size.x + roomSeparationMargin > room.position.x;
            bool overlapZ = position.z < room.position.z + existingRoomSize.z + roomSeparationMargin &&
                            position.z + size.z + roomSeparationMargin > room.position.z;

            if (overlapX && overlapZ)
            {
                return true;
            }
        }
        return false;
    }

    public void CreateCorridors()
    {
        Room currentRoom = rooms.Find(room => room.roomType == Room.RoomType.Start);
        Room endRoom = rooms.Find(room => room.roomType == Room.RoomType.End);

        List<Room> unvisitedRooms = new List<Room>(rooms);
        unvisitedRooms.Remove(currentRoom);

        while (currentRoom != endRoom)
        {
            Room closestRoom = GetClosestRoom(currentRoom, unvisitedRooms);
            if (closestRoom != null)
            {
                CreateCorridor(currentRoom, closestRoom);
                currentRoom.connectedRooms.Add(closestRoom);
                closestRoom.connectedRooms.Add(currentRoom);

                currentRoom = closestRoom;
                unvisitedRooms.Remove(closestRoom);
            }
            else
            {
                break;
            }
        }
    }

    private Room GetClosestRoom(Room currentRoom, List<Room> otherRooms)
    {
        Room closestRoom = null;
        float minDistance = float.MaxValue;

        foreach (var room in otherRooms)
        {
            float distance = Vector3.Distance(currentRoom.position, room.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestRoom = room;
            }
        }

        return closestRoom;
    }
    public void CreateCorridor(Room roomA, Room roomB)
    {
        // ��� ���� ���� ����
        LineRenderer corridor = Instantiate(lineRendererPrefab, transform);
        corridor.gameObject.SetActive(true);

        // roomA�� roomB�� ũ��� �� ��ġ ���
        Renderer roomARenderer = roomObjects[roomA].GetComponent<Renderer>();
        Renderer roomBRenderer = roomObjects[roomB].GetComponent<Renderer>();

        // ���� ũ��� �� ��ġ�� �������� �߰� ���� ���
        Vector3 doorA = new Vector3(
            roomA.position.x + roomARenderer.bounds.size.x / 2, // roomA�� ���� �� ��ġ
            roomA.position.y,
            roomA.position.z
        );
        Vector3 doorB = new Vector3(
            roomB.position.x,
            roomB.position.y,
            roomB.position.z - roomBRenderer.bounds.size.z / 2 // roomB�� �Ʒ��� �� ��ġ
        );

        // �� ���� �Ա� ��ġ�� ã�� �Լ� ȣ�� (����)
        Vector3 entranceA = FindEntrancePosition(roomA);
        Vector3 entranceB = FindEntrancePosition(roomB);

        // ����� ��� ���
        Vector3 midPoint = new Vector3(doorB.x, doorA.y, doorA.z);

        corridor.positionCount = 3;
        //corridor.SetPosition(0, roomA.position);
        corridor.SetPosition(0, entranceA);
        corridor.SetPosition(1, midPoint);
        corridor.SetPosition(2, entranceB);
        //corridor.SetPosition(4, roomB.position);

        corridors.Add(corridor);

        // roomA���� ��ο� �Ա� Ȱ��ȭ/��Ȱ��ȭ
        ActivatePassage(roomObjects[roomA], doorB - doorA);

        // roomB�� ������ ����� ���� ���
        Vector3 directionToB;
        if (midPoint.x == doorB.x) // ���� ����
        {
            directionToB = new Vector3(0, 0, doorB.z > midPoint.z ? -1 : 1);
        }
        else // ���� ����
        {
            directionToB = new Vector3(doorB.x > midPoint.x ? -1 : 1, 0, 0);
        }

        // roomB���� ��ο� �Ա� Ȱ��ȭ/��Ȱ��ȭ
        ActivatePassage(roomObjects[roomB], directionToB);
    }
    private Vector3 FindEntrancePosition(Room room)
    {
        // �Ա��� �̸���
        string[] entranceNames = new string[] { "Entrance_N", "Entrance_E", "Entrance_S", "Entrance_W" };

        // �濡 �����ϴ� ���� ������Ʈ�� ã��
        GameObject roomObj = roomObjects[room];

        foreach (string entranceName in entranceNames)
        {
            Transform entranceTransform = roomObj.transform.Find(entranceName);
            if (entranceTransform != null && !entranceTransform.gameObject.activeSelf)
            {
                // ��Ȱ��ȭ�� �Ա� �߰�, ��ġ ��ȯ
                return entranceTransform.position;
            }
        }

        // �⺻�� ��ȯ, ���� ó���� ���� �α� ���
        Debug.LogError("Active entrance not found for room: " + room.roomNumber);
        return room.position; // ���� �߽� ��ġ�� �⺻������ ��ȯ
    }


    private void ActivatePassage(GameObject roomObj, Vector3 direction)
    {
        // ��� ������ ��� �̸� ����
        string[] allPassages = new string[] { "Passage_N", "Passage_E", "Passage_S", "Passage_W" };

        string activePassageName, activeEntranceName;
        if (direction.x > 0) // ����
        {
            activePassageName = "Passage_W";
            activeEntranceName = "Entrance_W";
        }
        else if (direction.x < 0) // ����
        {
            activePassageName = "Passage_E";
            activeEntranceName = "Entrance_E";
        }
        else if (direction.z > 0) // ����
        {
            activePassageName = "Passage_S";
            activeEntranceName = "Entrance_S";
        }
        else // ����
        {
            activePassageName = "Passage_N";
            activeEntranceName = "Entrance_N";
        }

        // Ȱ��ȭ�� ��� ����
        SetActiveStateForChildren(roomObj, activePassageName, true);

        // Ȱ��ȭ�� �Ա� ��Ȱ��ȭ
        SetActiveStateForChildren(roomObj, activeEntranceName, false);

        // ������ ��� ��� ��Ȱ��ȭ
        foreach (string passage in allPassages)
        {
            if (passage != activePassageName)
            {
                SetActiveStateForChildren(roomObj, passage, false);
            }
        }
    }

    private void SetActiveStateForChildren(GameObject parent, string childName, bool state)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == childName)
            {
                child.gameObject.SetActive(state);
            }
            // ��������� �ڽ� ������Ʈ�鵵 Ž��
            if (child.childCount > 0)
            {
                SetActiveStateForChildren(child.gameObject, childName, state);
            }
        }
    }
}

