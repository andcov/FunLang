namespace FunLang
{
    public interface IExpression : ICloneable
    {
		public FType GetFType();
        public IExpression Eval(Env env);
		public bool Equals(Object obj);
        public int Compare(IExpression exp);
		public bool IsTrue();

        Token? Tok { get; set; }
    }

	public enum FType {
		FNumber,
		FChar,
		FSymbol,
		FList,
		FFunction,
		FDefine,
		FCallable,
		FIf,
		FFunctionator,
	}
}