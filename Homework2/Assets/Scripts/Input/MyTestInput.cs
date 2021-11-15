using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Game;

namespace Input
{
    public class MyTestInput : PlayerInput
    {
        [SerializeField] private ZombieMap _zombieMap;
        [SerializeField] private Transform target;
        
        private NavMeshPath path;
        private ZombieComponent farEnemy = null;
        
        private void Start()
        {
            path = new NavMeshPath();
        }
        
        public override (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput()
        {
            var alivePositions = _zombieMap.AlivePositions();
            var aliveZombies = _zombieMap.AliveGameObjects();
            if (alivePositions.Count == 0)
            {
                return (Vector3.zero, Quaternion.identity, false);
            }
            
            var targetIndex = 0;
            NavMesh.CalculatePath(transform.position, alivePositions[targetIndex], NavMesh.AllAreas, path);
            var shortestPath = PathLength(path);
            var longestPath = shortestPath;
            var longestIndex = 0;

            for (int i = 0; i < alivePositions.Count; i++)
            {
                NavMesh.CalculatePath(transform.position, alivePositions[i], NavMesh.AllAreas, path);
                var newPath = PathLength(path);
                if (newPath < shortestPath)
                {
                    shortestPath = newPath;
                    targetIndex = i;
                }

                if (newPath > longestPath)
                {
                    longestPath = newPath;
                    longestIndex = i;
                }
            }

            if (farEnemy == null || !farEnemy.IsAlive)
            {
                farEnemy = aliveZombies[longestIndex].GetComponent<ZombieComponent>();
            }
            
            
            //NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
            NavMesh.CalculatePath(transform.position, farEnemy.transform.position, NavMesh.AllAreas, path);
            Vector3 direction;
            if ((path.corners[1] - transform.position).magnitude < 0.2f)
            {
                direction = (path.corners[2] - transform.position);
            }
            else
            {
                direction = (path.corners[1] - transform.position);
            }
            direction.y = transform.position.y;
            
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            }

            // avoiding zombies
            var numberOfRays = 30;
            var deltaPosition = Vector3.zero;
            var angle = 270;
            
            for (int i = 0; i < numberOfRays; i++)
            {
                var rotation = Quaternion.LookRotation(direction);
                var rotationMod = Quaternion.AngleAxis((i / (float)numberOfRays) * angle - angle / 2, transform.up);
                var dirr = rotation * rotationMod * Vector3.forward * 1.5f;
                var pos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                Debug.DrawRay(pos, dirr, Color.blue);

                if (Physics.Raycast(pos, dirr, out var hit, 1.5f))
                {
                    if (hit.transform.parent && hit.transform.GetComponentInParent<ZombieComponent>() && hit.transform.GetComponentInParent<ZombieComponent>().IsAlive)
                    {
                        deltaPosition -= (1f / numberOfRays) * dirr;
                    }
                    else if (!hit.transform.gameObject.CompareTag("Obstacle"))
                    {
                        deltaPosition += (1f / numberOfRays) * dirr;
                    }
                }
                else
                {
                    deltaPosition += (1f / numberOfRays) * dirr;
                }
            }

            _zombieMap.GetActivityMap();

            var points = CalculateBulletTrajectory2();
            var look = new Vector3(points[targetIndex].x - transform.position.x, transform.position.y, points[targetIndex].z - transform.position.z);
            return (
                deltaPosition,//new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0f, UnityEngine.Input.GetAxis("Vertical")),
                Quaternion.LookRotation(look),
                true); //UnityEngine.Input.GetButtonDown("Fire1"));
        }

        private List<Vector3> CalculateBulletTrajectory2()
        {
            var alivePositions = _zombieMap.AliveGameObjects();
            RaycastHit hit;
            var rayLength = 100;
            var rayNumber = 360;
            var points = new List<Vector3>();
            var playerPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            
            for (int i = 0; i < alivePositions.Count; i ++)
            {
                points.Add(Vector3.zero);
                var enemyPos = new Vector3(alivePositions[i].transform.position.x, alivePositions[i].transform.position.y + 1f, alivePositions[i].transform.position.z);

                Physics.Raycast(playerPos, enemyPos - playerPos, out hit, Mathf.Infinity);

                if (hit.collider && hit.collider.transform.parent && hit.collider.transform.parent.gameObject == alivePositions[i])
                {
                    Debug.DrawLine(playerPos, hit.point, Color.green);
                    points[points.Count - 1] = enemyPos;
                }
                else
                {
                    for (int j = 0; j < rayNumber; j++)
                    {
                        var angle = 2 * Math.PI / rayNumber * j;
                        var destination = new Vector3((float)Math.Cos(angle) * rayLength, 0, (float)Math.Sin(angle) * rayLength);
                        
                        Physics.Raycast(playerPos, playerPos + destination, out hit, Mathf.Infinity);
                        var firstHit = hit;
                        
                        // добавить прогнозирование движения врага по навмешу и  выбирать траекторию, которая столкнется с врагом через время полета пули
                        
                        if (hit.collider.gameObject.CompareTag("Obstacle"))
                        {
                            Physics.Raycast(hit.point, Vector3.Reflect(destination, hit.normal), out hit, Mathf.Infinity);

                            if (hit.collider && hit.collider.transform.parent && hit.collider.transform.parent.gameObject == alivePositions[i])
                            {
                                Debug.DrawLine(playerPos, firstHit.point, Color.yellow);
                                Debug.DrawLine(firstHit.point, hit.point, Color.green);
                                points[points.Count - 1] = firstHit.point;
                                break;
                            }
                        }
                    }
                }
            }
            
            return points;
        }

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