using System;
namespace FunLang
{
	public class FChar : IExpression
	{
		public char ch;
		public Token? Tok { get; set; } = null;

		public FChar(char _ch, Token? _tok)
		{
			ch = _ch;
			Tok = _tok;
		}

        public IExpression Eval(Env env)
        {
            return this;
        }

        public override string ToString()
		{
			return $"Char {ch}";
		}
		public bool Equals(IExpression exp)
		{
			if (exp.GetFType() != FType.FChar) return false;
			var other = (FChar)exp;
			return ch == other.ch;
		}

		public object Clone()
		{
			return new FChar(ch, Tok);
		}
        public FType GetFType()
        {
            return FType.FChar;
        }
    }
}
