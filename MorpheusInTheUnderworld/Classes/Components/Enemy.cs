using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorpheusInTheUnderworld.Classes.Components
{
    public class Enemy
    {
        public Facing Facing { get; set; } = Facing.Left;
        public State State { get; set; }
        public bool IsAttacking => State == State.Combat || State == State.Combat;
        public bool CanJump => State == State.Idle || State == State.Walking;
        public static string GetChoice()
        {
            string chars = "abcdefghijklmnopqrstuvwxyz1234567890;ABCDEFGHIJKLMNOPQRSTUVWXYZ'#";
            Random rand = new Random();
            int num = rand.Next(0, chars.Length - 1);
            char[] bob = new char[1];
            bob[0] = chars[num];
            string s = new string(bob);
            return s;
        }
        public string choice = GetChoice();
    }
}
