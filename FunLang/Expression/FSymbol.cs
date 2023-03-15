using System;
namespace FunLang
{
    public class FSymbol : IExpression
    {
        public string name;
        public Token? Tok { get; set; } = null;

        public FSymbol(string _name)
        {
            name = _name;
            Tok = null;
        }

        public FSymbol(string _name, Token? _tok)
        {
            name = _name;
            Tok = _tok;
        }

        public IExpression Eval(Env env)
        {
            if (env.TryGetValue(name, out IExpression? e))
            {
                e.Tok = Tok;
                return e;
            }
            throw new InvalidFunProgram("Cannot find the symbol " + name, Tok);
        }

        public override string ToString()
        {
            return $"Sym {name}";
        }
        public bool Equals(IExpression exp)
        {
            if (exp.GetFType() != FType.FSymbol) return false;
            return this.name == ((FSymbol)exp).name;
        }

        public object Clone()
        {
            return new FSymbol(name, Tok);
        }
        public FType GetFType()
        {
            return FType.FSymbol;
        }
    }
}
