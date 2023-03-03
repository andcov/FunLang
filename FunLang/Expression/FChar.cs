using System;
namespace FunLang
{
	public class FChar : Expression
	{
		public char ch;
        public Token tok { get; set; }

		public FChar(char _ch, Token _tok)
		{
			ch = _ch;
			tok = _tok;
		}

        public Expression eval(Env env)
        {
            return this;
        }

        public override string ToString()
		{
			return $"Char {ch}";
		}
		public bool Equals(Expression exp)
		{
			if (exp.GetFType() != FType.FChar) return false;
			var other = (FChar)exp;
			return ch == other.ch;
		}

		public object Clone()
		{
			return new FChar(ch, tok);
		}
        public FType GetFType()
        {
            return FType.FChar;
        }
    }
}
