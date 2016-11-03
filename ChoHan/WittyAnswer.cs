using System;
using System.Collections.Generic;

namespace ChoHan
{
    public class WittyAnswer
    {
        public static string WrongAnswer()
        {
            Random random = new Random();
            var strings = new List<string> { "Be ashamed of yourself", "You disgust me", "DIPSHIT!!", "My cat can do better than you", "Having a bad time?" };
            int index = random.Next(strings.Count);
            var text = strings[index];
            return text;
        }

        public static string GoodAnswer()
        {
            Random random = new Random();
            var strings = new List<string> { "You expect a cookie or something?", "*Slow clap*", "You did kinda good", "Good job wasting your time on this game", "*Pat on the back*", "Lucky guess" };
            int index = random.Next(strings.Count);
            var text = strings[index];
            return text;
        }

        public static string Win()
        {
            Random random = new Random();
            var strings = new List<string> { "Cookie acquired.... Not", "We have a winner", "You win, now go outside", "You won...... NOTHING" };
            int index = random.Next(strings.Count);
            var text = strings[index];
            return text;
        }

        public static string Lose()
        {
            Random random = new Random();
            var strings = new List<string> { "You dishonor the family", "Looooosssseeeer!!!!", "Why so sad?" };
            int index = random.Next(strings.Count);
            var text = strings[index];
            return text;
        }

        public static string Idle()
        {
            Random random = new Random();
            var strings = new List<string> { "Dont idle dipshit", "AFK Loser", "Youre making everybody wait", "Youre worse than the losers" };
            int index = random.Next(strings.Count);
            var text = strings[index];
            return text;
        }

        public static string Tied()
        {
            Random random = new Random();
            var strings = new List<string> { "You manage to even the odds", "Try a little harder next time", "Tied, meh", "At least you didnt lose" };
            int index = random.Next(strings.Count);
            var text = strings[index];
            return text;
        }
    }
}