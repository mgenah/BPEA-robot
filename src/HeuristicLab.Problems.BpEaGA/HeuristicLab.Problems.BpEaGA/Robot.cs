﻿namespace HeuristicLab.Problems.BpEaGA
{
    public struct Robot
    {
        public string Name;
        public string FullName;

        public Robot(string name, string fullName)
        {
            this.Name = name;
            this.FullName = fullName;
        }
    }
}
