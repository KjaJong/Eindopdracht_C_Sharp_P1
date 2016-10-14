using System;

namespace ChoHan
{
    class ChoHan
    {
        private bool _lastAnswer;

        public void ThrowDice()
        {
            Random rnd = new Random();
            int roll = rnd.Next(1, 7) + rnd.Next(1, 7);
            if (roll % 2 == 0) { _lastAnswer = true; }
            else { _lastAnswer = false; }
        }

        public bool CheckResult(bool awnser)
        {
            return awnser == _lastAnswer;
        }
    }
}
