using System;
namespace FunLang
{
	public class FDefine : Expression
	{
		public Token tok { get; set; }
		public FSymbol? sym;
		public FList? list;
		public Expression vals;

		public FDefine(FSymbol _sym, Expression _vals, Token _tok)
		{
			sym = _sym;
			list = null;
			vals = _vals;
			tok = _tok;
		}

		public FDefine(FList _list, Expression _vals, Token _tok)
		{
			sym = null;
			list = _list;
			vals = _vals;
			tok = _tok;
		}

		public Expression eval(Env env)
		{
			if (sym != null)
			{
				env[sym.name] = vals.eval(env);
			}
			else if (list != null && vals.GetFType() == FType.FList)
			{
				var lvals = (FList)vals;
				for (int i = 0; i < list.Count(); ++i)
				{
					var sym = (FSymbol)list[i];
					env[sym.name] = lvals[i].eval(env);
				}
			}

			return new FNull(tok);
		}

		public override string ToString()
		{
			var res = "Def ";
			if (sym != null)
			{
				res += sym + " <- " + vals;
			}
			else
			{
				res += list + " <- " + vals;
			}
			return res;
		}
		public bool Equals(Expression exp)
		{
			return false;
		}

		public object Clone()
		{
			if (sym != null)
			{
				return new FDefine((FSymbol)sym.Clone(), (Expression)vals.Clone(), (Token)tok.Clone());
			}
			else
			{
				return new FDefine((FList)list.Clone(), (Expression)vals.Clone(), (Token)tok.Clone());
			}
		}
		public FType GetFType() { return FType.FDefine; }
	}
}