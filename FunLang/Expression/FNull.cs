using System;
namespace FunLang
{
	public class FNull : Expression
	{
		public Token tok { get; set; }

		public FNull(Token _tok)
		{
			tok = _tok;
		}

		public Expression eval(Env env)
		{
			return this;
		}

		public override string ToString()
		{
			return "NULL";
		}
		public bool Equals(Expression exp)
		{
			return false;
		}

		public object Clone()
		{
			return new FNull((Token)tok.Clone());
		}
		public FType GetFType() { return FType.FNull; }
	}
}