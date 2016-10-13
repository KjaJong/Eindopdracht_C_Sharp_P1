using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoHan
{
    class ChoHan
    {
        private bool lastAwnser;

        public bool ThrowDice()
        {
            Random rnd = new Random();
            int roll = rnd.Next(1, 7) + rnd.Next(1, 7);
            if (roll % 2 == 0) { return true; }
            else { return false; }
        }

        public bool CheckResult(bool awnser)
        {
            return awnser == lastAwnser;
        }
    }
}
