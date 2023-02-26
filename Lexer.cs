using System;
using System.Linq;

namespace Seroost
{
    public class Lexer
    {
        private char[] _content;
        private int _current = 0;

        public Lexer(string input)
        {
            _content = input.ToCharArray();
        }

        public void TrimLeft()
        {
            while (_current < _content.Length && char.IsWhiteSpace(_content[_current]))
            {
                _current++;
            }
        }

        public char[] Chop(int n)
        {
            var token = _content[_current..(_current + n)];
            _current += n;
            return token;
        }

        public char[] ChopWhile(Predicate<char> p)
        {
            var n = 0;
            while (n + _current < _content.Length && p.Invoke(_content[n + _current]))
            {
                n++;
            }
            return Chop(n);
        }

        public string NextToken()
        {
            if (_content.Length == 0 || _content.Length <= _current)
            {
                return null;
            }

            if (char.IsNumber(_content[_current]))
            {
                return new string(ChopWhile(char.IsNumber));
            }

            if (char.IsLetter(_content[_current]))
            {
                return new string(ChopWhile(char.IsLetter));
            }

            return new string(Chop(1));
        }
    }
}
