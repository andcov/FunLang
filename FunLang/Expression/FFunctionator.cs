using System;
using System.Xml.Linq;

namespace FunLang
{
	public class FFunctionator : IExpression
	{
        public Token? Tok { get; set; } = null;

        public FFunctionator(Token? _tok)
        {
            Tok = _tok;
        }

        public IExpression Eval(Env env)
        {
            return this;
        }

        public override string ToString()
        {
            return "$";
        }
        public bool Equals(IExpression exp)
        {
            return false;
        }

        public object Clone()
        {
            return new FFunctionator(Tok);
        }
        public FType GetFType()
        {
            return FType.FFunctionator;
        }
    }
}

