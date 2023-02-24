using Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers
{
    public class ImageManager : MonoBehaviour, IProvidable
    {
        public Sprite Cube;
        public Sprite RightOutward;
        public Sprite RightInward;
        public Sprite LeftOutward;
        public Sprite LeftInward;
        public Sprite UpOutward;
        public Sprite UpInward;
        public Sprite DownOutward;
        public Sprite DownInward;

        private void Awake()
        {
            ServiceProvider.Register(this);
        }
    }
}
