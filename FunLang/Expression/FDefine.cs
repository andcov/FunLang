using System;
namespace FunLang
{
	public class FDefine : IExpression
	{
		public Token? Tok { get; set; } = null;
		public FSymbol? sym;
		public FList? list;
		public IExpression vals;

		public FDefine(FSymbol _sym, IExpression _vals, Token? _tok)
		{
			sym = _sym;
			list = null;
			vals = _vals;
			Tok = _tok;
		}

		public FDefine(FList _list, IExpression _vals, Token? _tok)
		{
			sym = null;
			list = _list;
			vals = _vals;
			Tok = _tok;
		}

		public IExpression Eval(Env env)
		{
            var eval_vals = vals.Eval(env);
            if (sym != null)
			{
                env[sym.name] = eval_vals;
			}
			else if (list != null)
			{
				if (eval_vals.GetFType() == FType.FList && ((FList)eval_vals).Count == list.Count)
				{
                    var list_vals = (FList)eval_vals;
                    for (int i = 0; i < list.Count; ++i)
                    {
                        var sym = (FSymbol)list[i];
                        env[sym.name] = list_vals[i];
                    }
                } else
				{
                    throw new InvalidFunProgram("Could not create list out of right side of definition", vals.Tok);
                }
			}

			return new FNumber(-1000, null);
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
		public bool Equals(IExpression exp)
		{
			return false;
		}

		public object Clone()
		{
			if (sym != null)
			{
				return new FDefine((FSymbol)sym.Clone(), (IExpression)vals.Clone(), (Token)Tok.Clone());
			}
			else
			{
				return new FDefine((FList)list.Clone(), (IExpression)vals.Clone(), (Token)Tok.Clone());
			}
		}
		public FType GetFType() { return FType.FDefine; }
	}
}
