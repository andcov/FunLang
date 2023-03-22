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

        public IExpression Eval(Env env) => this;

        public override string ToString() => $"Char {ch}";
		public override bool Equals(Object? obj)
		{
            if (obj == null || obj is not IExpression)
            {
                return false;
            }
            var exp = (IExpression)obj;

            if (exp.GetFType() == FType.FNumber) return exp.Equals(this);
			if (exp.GetFType() != FType.FChar) return false;
            var other = (FChar)exp;
			return ch == other.ch;
		}
        public int Compare(IExpression exp)
        {
            if (this.Equals(exp)) return 0;
            else if (exp.GetFType() == FType.FChar)
            {
                var other = (FChar)exp;
                return (ch > other.ch) ? 1 : -1;
            }
            else if (exp.GetFType() == FType.FNumber)
            {
                return -1 * exp.Compare(this);
            }

            return 1;
        }

        public object Clone() => new FChar(ch, Tok);
        public FType GetFType() => FType.FChar;
    }
}
