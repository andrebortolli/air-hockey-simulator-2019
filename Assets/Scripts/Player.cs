using System.Collections;
using System.Collections.Generic;

namespace Scripts.Player
{
    public class Player
    {

        public Player(float speed)
        {
            this.speed = speed;
            this.isAI = true;
            score = 0;
        }
        public Player(float speed, bool isAI, string movementAxisNameX, string movementAxisNameY)
        {
            this.speed = speed;
            this.isAI = isAI;
            score = 0;
            SetAxesNames(movementAxisNameX, movementAxisNameY);
        }
        public void SetAxesNames(string x, string y)
        {
            this.movementAxisNameX = x;
            this.movementAxisNameY = y;
        }
        private string movementAxisNameX;
        public string MovementAxisNameX
        {
            get
            {
                return this.movementAxisNameX;

            }
            set
            {
                this.movementAxisNameX = value;
            }
        }
        private string movementAxisNameY;
        public string MovementAxisNameY
        {
            get
            {
                return this.movementAxisNameY;

            }
            set
            {
                this.movementAxisNameY = value;
            }
        }

        private bool isAI;
        public bool IsAI
        {
            get
            {
                return this.isAI;

            }
            set
            {
                this.isAI = value;
            }
        }
        private int score;
        public int Score
        {
            get
            {
                return this.score;
            }
            set
            {
                this.score = value;
            }
        }
        private float speed;
        public float Speed
        {
            get
            {
                return this.speed;
            }
            set
            {
                this.speed = value;
            }
        }
    }
}
