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

        public override string ToString() => $"Sym {name}";
        public override bool Equals(Object? obj)
        {
            if (obj == null || obj is not IExpression)
            {
                return false;
            }
            var exp = (IExpression)obj;

            if (exp.GetFType() != FType.FSymbol) return false;
            return this.name == ((FSymbol)exp).name;
        }
        public int Compare(IExpression exp) => (this.Equals(exp)) ? 0 : -1;

        public object Clone() => new FSymbol(name, Tok);
        public FType GetFType() => FType.FSymbol;
    }
}
