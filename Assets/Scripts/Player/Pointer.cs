using System;
using Interface;
using UnityEngine;

namespace Player
{
    public class Pointer : MonoBehaviour, IPauseable
    {
        public bool IsPaused { get; set; }

        private void Update()
        {
            if(IsPaused) return;
            
            
        }


        public void Pause()
        {
            
        }

        public void Resume()
        {
            
        }
    }
}
