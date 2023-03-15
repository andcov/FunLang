using System;
namespace FunLang
{
    [Serializable]
    public class InvalidFunProgram : Exception
    {
        public Token? tok = null;

        public InvalidFunProgram(Token? Tok) => tok = Tok;

        public InvalidFunProgram(string message, Token? Tok)
        : base(message) { tok = Tok; }

        public InvalidFunProgram(string message, Exception inner, Token? Tok)
        : base(message, inner) { tok = Tok; }
    }
}

