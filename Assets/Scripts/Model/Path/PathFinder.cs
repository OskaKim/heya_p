using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

namespace Utility
{
    public class PathUtility
    {
        public static Queue<Vector3Int> GetPath(Vector3Int startPos, Vector3Int destinationPos)
        {
            PathFinder pathFinder = new PathFinder();
            return pathFinder.FindPath(startPos, destinationPos);
        }
    }

    // NOTE : 패스 파인더. PathUtility 통해 사용됨
    public class PathFinder
    {
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
        private readonly List<Vector3Int> path = new List<Vector3Int>();
        //NOTE : 최단 경로를 분석하기 위한 상태값들이 계속 갱신
        private readonly List<Node> openList = new List<Node>();
        //NOTE : 처리가 완료된 노드를 담아둠
        private readonly Queue<Node> closeList = new Queue<Node>();
        private Node destinationNode = null;

        public Queue<Vector3Int> FindPath(Vector3Int startPos, Vector3Int destinationPos)
        {
            if (startPos == destinationPos) return null;

            var currentCloseNode = new Node(startPos, null, 0);

            // NOTE: 최대 연산 수
            int maxCount = 100;
            while (currentCloseNode != null || --maxCount <= 0)
            {
                currentCloseNode = FindNearPathFrom(currentCloseNode, destinationPos);
            }

            Queue<Vector3Int> path = new Queue<Vector3Int>();
            AddToPath(path, destinationNode);
            return path;
        }

        private void AddToPath(Queue<Vector3Int> path, Node node)
        {
            if (node == null) return;

            if (node.parent != null)
            {
                AddToPath(path, node.parent);
            }

            path.Enqueue(node.pos);
        }

        private Node FindNearPathFrom(Node currentCloseNode, Vector3Int destinationPos)
        {
            closeList.Enqueue(currentCloseNode);

            var pos = currentCloseNode.pos;
            List<Vector3Int> nearTiles = new List<Vector3Int>(4){
            new Vector3Int(pos.x,pos.y-1,0),
            new Vector3Int(pos.x-1,pos.y,0),
            new Vector3Int(pos.x+1,pos.y,0),
            new Vector3Int(pos.x,pos.y+1,0)
        };

            foreach (var currentPos in nearTiles)
            {
                // TODO : FurnitureInstallInfoHolder를 직접 사용 하지 않도록 수정. delegates등
                // NOTE: 장애물이 존재
                if(FurnitureInstallInfoHolder.IsAnyFurniture(currentPos)) continue;

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
}