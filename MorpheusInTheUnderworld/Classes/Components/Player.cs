using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorpheusInTheUnderworld.Classes.Components
{
    public enum Facing
    {
        Left, Right
    }

    public enum State
    {
        Idle,
        Walking,
        Combat,
        Guard
    }

    public class Player
    {
        public Facing Facing { get; set; } = Facing.Right;
        public State State { get; set; }
        public bool IsAttacking => State == State.Combat;
        public bool CanJump => State == State.Idle || State == State.Walking;
        public List<Shard> Shards { get; set; }
        public bool IsDefeated { get; set; }
        public float ImmuneTimer { get; set; }
    }
}
