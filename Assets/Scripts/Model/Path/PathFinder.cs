using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;

public class PathFinderManager : MonoBehaviour
{
    public static List<Vector3Int> StartPathFinding(Vector3Int startPos, Vector3Int destinationPos)
    {
        PathFinder pathFinder = new PathFinder();
        return pathFinder.FindPath(startPos, destinationPos);
    }
}

// NOTE : 패스 파인더. PathFinderManager를 통해 사용됨
public class PathFinder
{
    private Grid grid;

    private void Awake()
    {
        grid = GameObject.FindObjectOfType<Grid>();
    }

    private class Node
    {
        public Vector3Int pos;
        public Node parent;
        public int f;
        public Node(Vector3Int pos, Node parent, int f)
        {
            this.pos = pos;
            this.parent = parent;
            this.f = f;
        }
    }
    public Vector3Int StartPos { get; private set; }
    public Vector3Int DestinationPos { get; private set; }
    public bool IsFinish { get => isFinish; }
    private bool isFinish = false;
    public List<Vector3Int> Path { get => path; }

    private readonly List<Vector3Int> path = new List<Vector3Int>();
    //NOTE : 최단 경로를 분석하기 위한 상태값들이 계속 갱신
    private readonly List<Node> openList = new List<Node>();
    //NOTE : 처리가 완료된 노드를 담아둠
    private readonly List<Node> closeList = new List<Node>();

    private Node destinationNode;
    private Vector3Int destinationPos;
    private int calculCount = 0;
    // NOTE : 연산 최대 수. 이를 넘어가면 경로탐색 실패로 간주함
    private readonly static int MaxCalculCount = 1500;
    // NOTE : 인접 패스 연산은 코스트가 높기 때문에 한 프레임에 해당 횟수만큼만 처리
    private readonly static int MaxNearPathLoopInOneCicle = 10;

    private void Clear()
    {
        isFinish = false;
        path.Clear();
        openList.Clear();
        closeList.Clear();
        destinationNode = null;
        calculCount = 0;
    }

    public List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int destinationPos)
    {
        Clear();

        StartPos = startPos;
        DestinationPos = destinationPos;

        if (startPos == destinationPos) return null;

        var currentCloseNode = new Node(startPos, null, 0);
        this.destinationPos = destinationPos;
        return FindNearPathLoop(currentCloseNode);
    }

    private void AddToPath(List<Vector3Int> path, Node node)
    {
        if (node == null) return;

        if (node.parent != null)
        {
            AddToPath(path, node.parent);
        }

        path.Add(node.pos);
    }

    private List<Vector3Int> FindNearPathLoop(Node firstCloseNode)
    {
        Node currentCloseNode = firstCloseNode;

        while (true)
        {
            currentCloseNode = FindNearPathFrom(currentCloseNode);

            // NOTE : 탐색 종료
            if (currentCloseNode == null)
            {
                List<Vector3Int> path = new List<Vector3Int>();
                AddToPath(path, destinationNode);
                return path;
            }
        }
    }

    int count = 0;
    private Node FindNearPathFrom(Node currentCloseNode)
    {
        count++;

        closeList.Add(currentCloseNode);

        var pos = currentCloseNode.pos;
        List<Vector3Int> nearTiles = new List<Vector3Int>(4){
            new Vector3Int(pos.x,pos.y-1,0),
            new Vector3Int(pos.x-1,pos.y,0),
            new Vector3Int(pos.x+1,pos.y,0),
            new Vector3Int(pos.x,pos.y+1,0)
        };

        foreach (var currentPos in nearTiles)
        {
            // TODO : 장애물 처리
            // TEST : 장애물을 코드로 임시 배치
            if(currentPos == new Vector3Int(1, 1, 0) ||
            currentPos == new Vector3Int(2, 2, 0))
            {
                continue;
            }

            var parent = currentCloseNode;
            // NOTE : 시작 노드에서 해당 노드까지의 실제 소요 경비값
            var g = parent.f + 1;
            // NOTE : 휴리스틱 수정 값.해당 노드에서 최종 목적지까지의 추정 값(거리)
            var h = Mathf.Abs(destinationPos.x - currentPos.x) + Mathf.Abs(destinationPos.y - currentPos.y);
            var f = g + h;
            // NOTE : close list에 있을 경우엔 open list에 추가 안함
            if (closeList.Any(x => x.pos == currentPos)) continue;

            var sameNodeInOpenList = openList.FirstOrDefault(x => x.pos == currentPos);
            if (sameNodeInOpenList != null)
            {
                // NOTE : 동일 노드가 open list에 있을 경우, f치가 더 낮은 경우에 한해 값 갱신
                if (sameNodeInOpenList.f > f)
                {
                    sameNodeInOpenList.f = f;
                    sameNodeInOpenList.parent = parent;
                }
                continue;
            }
            openList.Add(new Node(currentPos, parent, f));
            ++calculCount;
        }

        if (openList.Count == 0)
        {
            // NOTE : 경로 탐색 실패
            Debug.Log("failed to find path");
            return null;
        }

        var nextCloseNode = openList[0];
        openList.ForEach(x =>
        {
            if (nextCloseNode.f > x.f)
            {
                nextCloseNode = x;
            }
        });
        if (nextCloseNode.pos == destinationPos)
        {
            destinationNode = nextCloseNode;
            return null;
        }

        openList.Remove(nextCloseNode);

        return nextCloseNode;
    }
}