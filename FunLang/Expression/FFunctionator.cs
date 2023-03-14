using System;
using System.Xml.Linq;

namespace FunLang
{
	public class FFunctionator : Expression
	{
        public Token? tok { get; set; } = null;

        public FFunctionator(Token? _tok)
        {
            tok = _tok;
        }

        public Expression eval(Env env)
        {
            return this;
        }

        public override string ToString()
        {
            return "$";
        }
        public bool Equals(Expression exp)
        {
            return false;
        }

        public object Clone()
        {
            return new FFunctionator(tok);
        }
        public FType GetFType()
        {
            return FType.FFunctionator;
        }
    }
}

