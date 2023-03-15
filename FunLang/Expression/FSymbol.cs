using System;
namespace FunLang
{
    public class FSymbol : Expression
    {
        public string name;
        public Token? tok { get; set; } = null;

        public FSymbol(string _name)
        {
            name = _name;
            tok = null;
        }

        public FSymbol(string _name, Token? _tok)
        {
            name = _name;
            tok = _tok;
        }

        public Expression eval(Env env)
        {
            if (env.TryGetValue(name, out Expression? e))
            {
                e.tok = tok;
                return e;
            }
            throw new InvalidFunProgram("Cannot find the symbol " + name, tok);
        }

        public override string ToString()
        {
            return $"Sym {name}";
        }
        public bool Equals(Expression exp)
        {
            if (exp.GetFType() != FType.FSymbol) return false;
            return this.name == ((FSymbol)exp).name;
        }

        public object Clone()
        {
            return new FSymbol(name, tok);
        }
        public FType GetFType()
        {
            return FType.FSymbol;
        }
    }
}
