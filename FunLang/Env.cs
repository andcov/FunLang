using System;
namespace FunLang
{
	public class Env : Dictionary<string, Expression>, ICloneable
    {
		public void AddStandard() {
			this["+"] = new Add();
            this["-"] = new Substract();
            this["*"] = new Multiply();

            this["=="] = new Equal();
            this["!="] = new Different();

            this["length"] = new Length();
            this["first"] = new First();
            this["second"] = new Second();
            this["push"] = new Push();
            this["rest"] = new Rest();

            this["println"] = new Println();
        }

		public void AddArguments(List<FSymbol> parameters, FList arguments)
		{
			for(int i = 0; i < parameters.Count(); ++i)
			{
                this[parameters[i].name] = arguments[i];
            }
		}

        public object Clone()
        {
            Env clone = new Env();
            foreach (var (k, v) in this)
            {
                clone[k] = (Expression) (v.Clone());
            }
            return clone;
        }
    }
}

