using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class ZombieComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _aliveView;

        [SerializeField] private GameObject _diedView;

        [SerializeField] private float _speed = 5f;

        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private Vector3[] _deltaPath;

        private int _currentPoint = 0;
        private Vector3 _initPosition;
        private GameObject _player;
        private NavMeshPath _path;
        public NavMeshSurface a;

        private void Awake()
        {
            _initPosition = transform.position;
        }

        private void OnEnable()
        {
            _path = new NavMeshPath();
            SetState(true);
            _player = FindObjectOfType<PlayerController>().gameObject;
        }

        private void Update()
        {
            /*
            if (_deltaPath == null || _deltaPath.Length < 2)
                return;

            var direction = _initPosition + _deltaPath[_currentPoint] - transform.position;
            _rigidbody.velocity = IsAlive ? direction.normalized * _speed : Vector3.zero;

            if (direction.magnitude <= 0.1f)
            {
                _currentPoint = (_currentPoint + 1) % _deltaPath.Length;
            }
            */
            NavMesh.CalculatePath(transform.position, _player.transform.position, 1, _path);
            
            Vector3 direction;
            if ((_path.corners[1] - transform.position).magnitude < 0.2f)
            {
                direction = (_path.corners[2] - transform.position);
            }
            else
            {
                direction = (_path.corners[1] - transform.position);
            }
            direction.y = transform.position.y;
            
            _rigidbody.velocity = IsAlive ? direction.normalized * _speed : Vector3.zero;
        }

        private void OnCollisionEnter(Collision other)
        {
            var playerController = other.gameObject.GetComponentInParent<PlayerController>();
            
            if (playerController && IsAlive)
            {
                playerController.Hitpoints = 0;
            }
        }

        public void SetState(bool alive)
        {
            _aliveView.SetActive(alive);
            _diedView.SetActive(!alive);
        }

        public bool IsAlive => _aliveView.activeInHierarchy;
    }
}