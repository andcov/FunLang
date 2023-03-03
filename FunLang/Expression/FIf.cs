using System;
namespace FunLang
{

    public class FIf : Expression
    {
        public Expression condition;
        public Expression then;
        public Expression other;
        public Token tok { get; set; }

        public FIf(Expression _condition, Expression _then, Expression _other, Token _tok)
        {
            condition = _condition;
            then = _then;
            other = _other;
            tok = _tok;
        }

        public Expression eval(Env env)
        {
            var cond = condition.eval(env);

            if (cond.GetFType() == FType.FList)
            {
                Expression res;

                if (((FList)cond).Count() != 0)
                {
                    res = then.eval(env);
                }
                else
                {
                    res = other.eval(env);
                }

                if (res.GetFType() == FType.FList && ((FList)res).Count() == 1)
                    return ((FList)res)[0];
                else
                    return res;

            }
            else if (cond.GetFType() == FType.FNumber)
            {
                var num = (FNumber)cond;

                Expression res;

                if (!num.Equals(new FNumber(0)))
                {
                    res = then.eval(env);
                }
                else
                {
                    res = other.eval(env);
                }

                if (res.GetFType() == FType.FList && ((FList)res).Count() == 1)
                    return ((FList)res)[0];
                else
                    return res;
            }
            throw new InvalidOperationException("Cannot evaluate truth value of " + cond);
        }

        public override string ToString()
        {
            return $"If [{condition}] then [{then}] else [{other}]";
        }
        public bool Equals(Expression exp)
        {
            return false;
        }

        public object Clone()
        {
            return new FIf((Expression)condition.Clone(), (Expression)then.Clone(), (Expression)other.Clone(), (Token)tok.Clone());
        }
        public FType GetFType() { return FType.FIf; }
    }

}
