using System;
namespace FunLang
{
    public class Token : ICloneable
    {
        public string? token = null;
        public int? position = null;

        public Token()
        {
            token = null;
            position = null;
        }

        public Token(string? _token, int? _position)
        {
            token = _token;
            position = _position;
        }

        public string Value()
        {
            if (token != null)
            {
                return token;
            }
            return "";
        }

        public override string ToString()
        {
            return $"<{token}, {position}>";
        }

        public object Clone()
        {
            return new Token(token, position);
        }
    }
}

