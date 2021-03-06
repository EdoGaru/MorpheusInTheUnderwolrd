﻿using System;
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
        public bool IsDefeated { get; set; }
        public bool OnCombat { get; set; }
        public float ImmuneTimer { get; set; }
    }
}
