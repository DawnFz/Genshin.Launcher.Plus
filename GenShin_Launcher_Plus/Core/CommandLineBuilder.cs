using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenShin_Launcher_Plus.Core
{
    public class CommandLineBuilder
    {
        readonly Dictionary<string, string> _options = new();
        public void AddOption(string name, string value)
        {
            _options.Add(name, value);
        }
        public override string ToString()
        {
            var s = new StringBuilder();
            foreach (var e in _options)
            {
                s.Append(" ");
                s.Append(e.Key);
                s.Append(' ');
                s.Append(e.Value);
                s.Append(' ');
            }
            return s.ToString();
        }
    }
}
