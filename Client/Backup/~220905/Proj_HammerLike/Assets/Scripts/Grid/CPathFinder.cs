using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hger;

public class CPathFinder
{
    public CPathFinder()
    {
        Debug.LogError("CGridMap lost");
        return;
    }
    public CPathFinder(CGridMap _gridMap, bool _allowDiagonal, bool _allowThrow)
    {
        gridMap = _gridMap;
        width = _gridMap.width;
        height = _gridMap.height;
        cellSize = _gridMap.cellSize;
        originPos = _gridMap.originPos;
        allowDiagonal = _allowDiagonal;
        allowThrow = _allowThrow;
    }

    CGridMap gridMap;
    int width, height;
    float cellSize;
    Vector2 originPos;

    CNode startNode, targetNode, currentNode; // ����, ��ǥ, ����
    CNode[,] nodeArray; // ��ü ��� �迭
    List<CNode> openList, closeList; // ���� ����Ʈ, ���� ����Ʈ

    public List<CNode> finalNodeList;
    
    bool allowDiagonal, allowThrow;

    public void Initialize(Vector2 _startPos, Vector2 _targetPos)
    {
        int startX, startY, targetX, targetY;
        gridMap.GetXY(_startPos, out startX, out startY);
        gridMap.GetXY(_targetPos, out targetX, out targetY);

        startNode = nodeArray[startX, startY];
        targetNode = nodeArray[targetX, targetY];
    }

    public void PathFinding(Vector2 _startPos, Vector2 _targetPos)
    {
        if (nodeArray == null)
            nodeArray = new CNode[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                bool isWall = false;

                foreach (Collider2D col in Physics2D.OverlapCircleAll(gridMap.GetWorldPosition(i, j) + new Vector2(0.5f, 0.5f), 0.4f * cellSize))
                {
                    // ���� ��� ����
                    if (col.OverlapPoint(_startPos)) continue;

                    // �� & ���� �ݸ��� Block ó��
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall") ||
                        col.gameObject.layer == LayerMask.NameToLayer("Unit"))
                        isWall = true;
                    else
                        isWall = false;
                }

                nodeArray[i, j] = new CNode(isWall, i, j);
            }
        }
        //SetStartGrid(startPos);
        //SetTargetGrid(targetPos);

        int startX, startY, targetX, targetY;
        gridMap.GetXY(_startPos, out startX, out startY);
        gridMap.GetXY(_targetPos, out targetX, out targetY);

        // Ÿ���� ���� ��Ż �Ǻ�
        if (width <= startX ||
            height <= startY ||
            0 > startX ||
            0 > startY ||
            width <= targetX ||
            height <= targetY ||
            0 > targetX ||
            0 > targetY)
        {
            //Debug.LogWarning("Target Pos Get Out of Range");
            //finalNodeList = new List<CNode>(); // �ֱ� Ž�� ��� �ʱ�ȭ
            return;
        }

        startNode = nodeArray[startX, startY];
        targetNode = nodeArray[targetX, targetY];

        if (startNode.isWall || targetNode.isWall)
        {
            //Debug.LogWarning("Start or Target node is not correct");
            return;
        }

        if (startNode == null)
            startNode = nodeArray[0, 0];
        if (targetNode == null)
            targetNode = nodeArray[width - 1, height - 1];

        openList = new List<CNode>() { startNode };
        closeList = new List<CNode>();
        finalNodeList = new List<CNode>();

        while (openList.Count > 0)
        {
            currentNode = openList[0];

            for(int i = 0; i < openList.Count; i++)
            {
                // fCost(total cost) ���ų� �۰�, hCost(�����Ÿ�) �� ���� ��
                if (openList[i].fCost <= currentNode.fCost && openList[i].hCost < currentNode.hCost)
                    currentNode = openList[i];
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            // Last Loop
            if(currentNode == targetNode)
            {
                CNode currentTargetNode = targetNode;
                
                // �ξ��� �θ� ����(���� ����)�� �Ž��� �ö󰡸� finalNodeList�� �߰�
                while(currentTargetNode != startNode)
                {
                    finalNodeList.Add(currentTargetNode);
                    currentTargetNode = currentTargetNode.parent;
                }

                finalNodeList.Add(startNode);
                finalNodeList.Reverse(); // ����� ���� ����

                for (int i = 0; i < finalNodeList.Count - 1; i++)
                {
                    Vector2 start = gridMap.GetWorldPosition(finalNodeList[i].x, finalNodeList[i].y);
                    //Debug.Log(start);
                    Vector2 end = gridMap.GetWorldPosition(finalNodeList[i + 1].x, finalNodeList[i + 1].y);
                    //Debug.Log(end);
                    Debug.DrawLine(start, end, Color.white, 1f);
                }

                return;
            }


            // �֢آע�
            if (allowDiagonal)
            {
                AddOpenList(currentNode.x + 1, currentNode.y + 1);
                AddOpenList(currentNode.x - 1, currentNode.y + 1);
                AddOpenList(currentNode.x - 1, currentNode.y - 1);
                AddOpenList(currentNode.x + 1, currentNode.y - 1);
            }

            // �� �� �� ��
            AddOpenList(currentNode.x, currentNode.y + 1);
            AddOpenList(currentNode.x + 1, currentNode.y);
            AddOpenList(currentNode.x, currentNode.y - 1);
            AddOpenList(currentNode.x - 1, currentNode.y);
        }



    }

    void AddOpenList(int x, int y)
    {
        int straightCost = 10;
        int diagonalCost = 14;

        // 1. �׸��� �� �ȿ� �����ϴ°�?
        // 2. ���ΰ�?
        // 3. closeList�� ���� �Ǿ� �ִ°�?
        if (x <= width - 1 && 
            y <= height - 1 && 
            x >= 0 && 
            y >= 0 && 
            !nodeArray[x, y].isWall&& 
            !closeList.Contains(nodeArray[x, y]))
        {
            // �밢 �̵�
            // ���� & ������ �� ���� ������ �� ��� �Ұ�
            if (allowDiagonal)
                if (nodeArray[currentNode.x, y].isWall && 
                    nodeArray[x, currentNode.y].isWall)
                    return;

            // �ڳ� �̵�
            // �� �νĽ� �밢 �̵� �Ұ�
            if (!allowThrow)
                if (nodeArray[currentNode.x, y].isWall ||
                    nodeArray[x, currentNode.y].isWall)
                    return;

            CNode neighborNode = nodeArray[x, y];

            int moveCost = currentNode.gCost;

            // ���� Ȥ�� ���� dir
            if (currentNode.x == x ||
                currentNode.y == y) 
            {
                moveCost += straightCost;
            }
            // �밢�� dir
            else
            {
                moveCost += diagonalCost;
            }

            // 1. ���� �̵� ����� �̿� ����� ��뺸�� ���� ��°�?
            // 2. Ȥ�� ���� ����Ʈ�� �̿� ��尡 ���ԵǾ� ���� �ʴ°�?
            if (moveCost < neighborNode.gCost ||
                !openList.Contains(neighborNode))
            {
                neighborNode.gCost = moveCost;
                neighborNode.hCost = (
                    Mathf.Abs(neighborNode.x - targetNode.x) 
                    + Mathf.Abs(neighborNode.y - targetNode.y)
                    ) * straightCost;
                neighborNode.parent = currentNode;

                openList.Add(neighborNode);
            }

        }
    }

}
