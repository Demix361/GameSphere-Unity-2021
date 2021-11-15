using System.Linq;
using Game;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AI;

namespace Input
{
    public class MyInput : PlayerInput
    {
        [SerializeField] private ZombieMap _zombieMap;
        [SerializeField] private Transform _player;
        [SerializeField] private float _fireDistance;

        private NavMeshPath path;
        
        private void Start()
        {
            path = new NavMeshPath();
        }
        
        public override (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput()
        {
            var alivePositions = _zombieMap.AlivePositions();
            if (alivePositions.Count == 0)
            {
                return (Vector3.zero, Quaternion.identity, false);
            }
            
            var target = alivePositions.First();
            NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            var shortestPath = PathLength(path);
            
            for (int i = 0; i < alivePositions.Count; i++)
            {
                NavMesh.CalculatePath(transform.position, alivePositions[i], NavMesh.AllAreas, path);
                var newPath = PathLength(path);
                if (newPath < shortestPath)
                {
                    shortestPath = newPath;
                    target = alivePositions[i];
                }
            }
            
            NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            
            Vector3 direction;
            if ((path.corners[1] - transform.position).magnitude < 0.2f)
            {
                print("here");
                direction = (path.corners[2] - _player.position);
            }
            else
            {
                direction = (path.corners[1] - _player.position);
            }
            direction.y = transform.position.y;
            
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            }
            
            return (direction, Quaternion.LookRotation(direction), (transform.position - target).magnitude <= _fireDistance);
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