using System;
using System.Collections;
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
            NavMesh.CalculatePath(transform.position, _player.transform.position, 1, _path);
            
            Vector3 direction;
            direction.y = transform.position.y;
            direction = (_path.corners[1] - transform.position);
            
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

        private IEnumerator DissapearBody()
        {
            yield return new WaitForSeconds(2f);
            
            _diedView.SetActive(false);
        }
        
        public void SetState(bool alive)
        {
            _aliveView.SetActive(alive);
            _diedView.SetActive(!alive);

            if (!alive)
            {
                StartCoroutine(DissapearBody());
            }
        }

        public bool IsAlive => _aliveView.activeInHierarchy;
    }
}