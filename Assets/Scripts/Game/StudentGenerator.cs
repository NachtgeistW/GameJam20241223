using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StudentGenerator: MonoBehaviour
    {
        [SerializeField] private GameObject studentPrefab;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float delay = 2f;

        private void Start()
        {
            StartCoroutine(SpawnStudents());
        }

        private IEnumerator SpawnStudents()
        {
            while (true)
            {
                Instantiate(studentPrefab, spawnPoint.position, spawnPoint.rotation);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}