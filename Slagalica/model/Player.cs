using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Slagalica
{
    class Player
    {
        public Player()
        {

        }
        public Player(string name, int points)
        {
            Name = name;
            Points = points;
        }

        public string Name { get; set; }
        public int Points { get; set; }

        public void WriteInFile()
        {
            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = projectPath + "/resources/score.txt";
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(Name+ "#"+ Points);
            }
        }

        public static Boolean CheckIfNameExists(string input)
        {
            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string scoreFilePath = projectPath + "/resources/score.txt";

            foreach (string line in System.IO.File.ReadLines(scoreFilePath))
            {
                string[] parts = line.Split(new[] { '#' });
                Player p = new Player(parts[0], Int32.Parse(parts[1]));
                if (p.Name.Equals(input))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
