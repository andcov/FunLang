using System;
using System.Xml.Linq;

namespace FunLang
{
	public class FFunctionator : IExpression
	{
        public Token? Tok { get; set; } = null;

        public FFunctionator(Token? _tok) => Tok = _tok;

        public IExpression Eval(Env env) => this;

        public override string ToString() => "$";
        public override bool Equals(Object? obj)
        {
            if (obj == null || obj is not IExpression)
            {
                return false;
            }
            var exp = (IExpression)obj;

            return exp.GetFType() == FType.FFunctionator;
        }
        public int Compare(IExpression exp) => (exp.GetFType() == FType.FFunctionator) ? 0 : -1;
        public bool IsTrue() => throw new InvalidFunProgram("Cannot evaluate truth value of FFunctionator.", Tok);

        public object Clone() => new FFunctionator(Tok);
        public FType GetFType() => FType.FFunctionator;
    }
}

