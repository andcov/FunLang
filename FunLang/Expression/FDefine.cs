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
            var eval_vals = vals.eval(env);
            if (sym != null)
			{
				env[sym.name] = eval_vals;
			}
			else if (list != null)
			{
				if(eval_vals.GetFType() == FType.FList && ((FList)eval_vals).Count == list.Count)
				{
                    var list_vals = (FList)eval_vals;
                    for (int i = 0; i < list.Count; ++i)
                    {
                        var sym = (FSymbol)list[i];
                        env[sym.name] = list_vals[i];
                    }
                } else
				{
                    throw new InvalidOperationException("Could not create list out of right side of definition");
                }
			}

			return new FNumber(-1000);
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
