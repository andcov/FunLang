namespace FunLang
{
    public class Token : ICloneable
    {
        public string token;
        public int position;

        public Token(string _token, int _position)
        {
            token = _token;
            position = _position;
        }

        public string Value()
        {
            return token;
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

