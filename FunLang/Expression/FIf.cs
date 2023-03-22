using System;
namespace FunLang
{

	public class FIf : IExpression
	{
		public IExpression condition;
		public IExpression then;
		public IExpression other;
		public Token? Tok { get; set; } = null;

		public FIf(IExpression _condition, IExpression _then, IExpression _other, Token? _tok)
		{
			condition = _condition;
			then = _then;
			other = _other;
			Tok = _tok;
		}

		public IExpression Eval(Env env)
		{
			var cond = condition.Eval(env);

            IExpression? res = null;

            if (cond.GetFType() == FType.FList)
			{
				if (((FList)cond).Count != 0)
				{
					res = then.Eval(env);
				}
				else
				{
					res = other.Eval(env);
				}
			}
			else if (cond.GetFType() == FType.FNumber)
			{
				var num = (FNumber)cond;

				if (!num.Equals(new FNumber(0, null)))
				{
					res = then.Eval(env);
				}
				else
				{
					res = other.Eval(env);
				}
			}

			if (res == null)
			{
                throw new InvalidFunProgram("Cannot evaluate truth value of " + cond, cond.Tok);
            } else if (res.GetFType() == FType.FList && ((FList)res).Count >= 1)
                return ((FList)res)[^1]; // return last element
            else
                return res;
		}

		public override string ToString() => $"If [{condition}] then [{then}] else [{other}]";
		
		public override bool Equals(Object? obj)
		{
            if (obj == null || obj is not IExpression)
            {
                return false;
            }
            var exp = (IExpression)obj;

            if (exp.GetFType() != FType.FIf) return false;
			var otherIf = (FIf)exp;
			return condition.Equals(otherIf.condition) && then.Equals(otherIf.then) && other.Equals(otherIf.other);
        }
        public int Compare(IExpression exp) => (this.Equals(exp)) ? 0 : -1;

        public object Clone() => new FIf((IExpression)condition.Clone(), (IExpression)then.Clone(), (IExpression)other.Clone(), (Token)Tok.Clone());
		
		public FType GetFType() => FType.FIf;
	}

}
