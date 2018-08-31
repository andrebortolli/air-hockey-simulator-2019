using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    //Criar classe AI e herdar para utilizar o isAI.
    class Jogador
    {
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
    }
}
