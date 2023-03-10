using System;
using System.Xml.Linq;

namespace FunLang
{
	public class FDollar : Expression
	{
        public Token tok { get; set; }

        public FDollar(Token _tok)
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
            return new FDollar(tok);
        }
        public FType GetFType()
        {
            return FType.FDollar;
        }
    }
}

