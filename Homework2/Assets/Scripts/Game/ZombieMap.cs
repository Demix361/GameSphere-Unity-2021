using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class ZombieMap : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private NavMeshSurface _navMeshSurface;
        [SerializeField] private int activityMapN;
        public int mapLevels = 6;
        
        private List<ZombieComponent> _zombieComponents = new List<ZombieComponent>();
        private int[,] activityMap;
        private float minX = -11f;
        private float maxX = 11f;
        private float minZ = -11f;
        private float maxZ = 11f;
        private float deltaX;
        private float deltaZ;
        
        private NavMeshPath path;

        
        private void Start()
        {
            path = new NavMeshPath();
        }

        private void Awake()
        {
            _zombieComponents = _root.gameObject.GetComponentsInChildren<ZombieComponent>().ToList();
            foreach (var z in _zombieComponents)
            {
                z.a = _navMeshSurface;
            }

            activityMap = new int[activityMapN, activityMapN];
            deltaX = (maxX - minX) / activityMapN;
            deltaZ = (maxZ - minZ)  / activityMapN;
        }

        public int[,] GetActivityMap()
        {
            var positions = AlivePositions();

            for (int i = 0; i < activityMapN; i++)
            {
                for (int j = 0; j < activityMapN; j++)
                {
                    activityMap[i, j] = 0;
                    
                    foreach (var pos in positions)
                    {
                        if (pos.x >= minX + i * deltaX && pos.x < minX + (i + 1) * deltaX &&
                            pos.z >= minZ + j * deltaZ && pos.z < minZ + (j + 1) * deltaZ)
                        {
                            activityMap[i, j] += 1;
                        }
                    }

                    activityMap[i, j] = GetLevel(activityMap[i, j]);
                }
            }
            
            DrawActivityMap();
            
            return activityMap;
        }

        private int GetLevel(int amount)
        {
            var k = (float)amount / CountAlive();
            var res = 0;
            
            if (k >= 0 && k < 0.05f)
            {
                res = 0;
            }
            else if (k >= 0.05f && k < 0.1f)
            {
                res = 1;
            }
            else if (k >= 0.1f && k < 0.15f)
            {
                res = 2;
            }
            else if (k >= 0.15f && k < 0.2f)
            {
                res = 3;
            }
            else if (k >= 0.2f && k < 0.3f)
            {
                res = 4;
            }
            else
            {
                res = 5;
            }

            return res;
        }

        private void DrawActivityMap()
        {
            for (int i = 0; i < activityMapN; i++)
            {
                for (int j = 0; j < activityMapN; j++)
                {
                    var k = (float)activityMap[i, j] / mapLevels;
                    Color c;
                    if (k <= 0.5f)
                    {
                        c = new Color(k * 2, 1, 0, 1);
                    }
                    else
                    {
                        c = new Color(1, 1 - (k - 0.5f) * 2, 0, 1);
                    }

                    var offset = 0.1f;
                    Debug.DrawLine(new Vector3(minX + i * deltaX + offset, 0f,  minZ + (j + 1) * deltaZ - offset), new Vector3(minX + (i + 1) * deltaX - offset, 0f,  minZ + (j + 1) * deltaZ - offset), c);
                    Debug.DrawLine(new Vector3(minX + i * deltaX + offset, 0f,  minZ + j * deltaZ + offset), new Vector3(minX + (i + 1) * deltaX - offset, 0f,  minZ + j * deltaZ + offset), c);
                    Debug.DrawLine(new Vector3(minX + i * deltaX + offset, 0f,  minZ + j * deltaZ + offset), new Vector3(minX + i * deltaX + offset, 0f,  minZ + (j + 1) * deltaZ - offset), c);
                    Debug.DrawLine(new Vector3(minX + (i + 1) * deltaX - offset, 0f,  minZ + j * deltaZ + offset), new Vector3(minX + (i + 1) * deltaX - offset, 0f,  minZ + (j + 1) * deltaZ - offset), c);
                }
            }
        }

        public bool GoodPosition(Vector3 pos)
        {
            int posX = 0, posZ = 0;

            for (int i = 0; i < activityMapN; i++)
            {
                for (int j = 0; j < activityMapN; j++)
                {
                    if (pos.x >= minX + i * deltaX && pos.x < minX + (i + 1) * deltaX &&
                        pos.z >= minZ + j * deltaZ && pos.z < minZ + (j + 1) * deltaZ)
                    {
                        posX = i;
                        posZ = j;
                    }
                }
            }

            for (int i = 0; i < activityMapN; i++)
            {
                for (int j = 0; j < activityMapN; j++)
                {
                    if (activityMap[i, j] < activityMap[posX, posZ])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Vector3 ChangePosition(Vector3 pos)
        {
            int posX = 0, posZ = 0;
            var res = new Vector3(0, pos.y, 0);
            
            for (int i = 0; i < activityMapN; i++)
            {
                for (int j = 0; j < activityMapN; j++)
                {
                    if (pos.x >= minX + i * deltaX && pos.x < minX + (i + 1) * deltaX &&
                        pos.z >= minZ + j * deltaZ && pos.z < minZ + (j + 1) * deltaZ)
                    {
                        posX = i;
                        posZ = j;
                    }
                }
            }

            List<Vector2Int> higherPositions = new List<Vector2Int>();
            
            for (int i = 0; i < activityMapN; i++)
            {
                for (int j = 0; j < activityMapN; j++)
                {
                    if (activityMap[i, j] < activityMap[posX, posZ])
                    {
                        higherPositions.Add(new Vector2Int(i, j));
                    }
                }
            }
            
            // сортируем массив подходящих позиций по их рейтингу
            for (int i = 0; i < higherPositions.Count; i++)
            {
                for (int j = 0; j < higherPositions.Count - 1; j++)
                {
                    if (activityMap[higherPositions[j].x, higherPositions[j].y] <
                        activityMap[higherPositions[j + 1].x, higherPositions[j + 1].y])
                    {
                        (higherPositions[j], higherPositions[j + 1]) = (higherPositions[j + 1], higherPositions[j]);
                    }
                }
            }
            print("HELLO");
            print(higherPositions.Count);

            NavMesh.CalculatePath(pos, GetPos(higherPositions[0].x, higherPositions[0].y), NavMesh.AllAreas, path);
            var shortestPath = PathLength(path);
            var shortestPathIndex = 0;

            for (int i = 0; i < higherPositions.Count; i++)
            {
                NavMesh.CalculatePath(pos, GetPos(higherPositions[i].x, higherPositions[i].y), NavMesh.AllAreas, path);
                var curPathLength = PathLength(path);

                if (curPathLength < shortestPath)
                {
                    shortestPath = curPathLength;
                    shortestPathIndex = i;
                }
            }

            res = GetPos(higherPositions[shortestPathIndex].x, higherPositions[shortestPathIndex].y);
            //res = GetPos(higherPositions[0].x, higherPositions[0].y);
            res.y = pos.y;

            return res;
        }

        private Vector3 GetPos(int x, int z)
        {
            var sourcePos = new Vector3(minX + (x + 0.5f) * deltaX, 0, minZ + (z + 0.5f) * deltaZ);

            NavMesh.SamplePosition(sourcePos, out var hit, deltaX, NavMesh.AllAreas);
            
            print(hit.position);
            return hit.position;
        }

        public int CountAlive() => _zombieComponents.Count(z => z.IsAlive);
        
        public List<Vector3> AlivePositions() => _zombieComponents
            .Where(z => z.IsAlive)
            .Select(z=>z.gameObject.transform.position)
            .ToList();
        
        public List<GameObject> AliveGameObjects() => _zombieComponents
            .Where(z => z.IsAlive)
            .Select(z => z.gameObject)
            .ToList();
        
        private float PathLength(NavMeshPath newPath)
        {
            var res = 0f;
            
            for (int i = 0; i < newPath.corners.Length - 1; i++)
            {
                res += (newPath.corners[i] - newPath.corners[i + 1]).magnitude;
            }

            return res;
        }
    }
}