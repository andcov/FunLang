using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FunLang
{
	public class Env : Dictionary<string, IExpression>, ICloneable
    {
		public void AddStandard() {
			this["+"] = new Add();
            this["-"] = new Substract();
            this["*"] = new Multiply();
            this["/"] = new Divide();
            this["%"] = new Modulo();

            this["=="] = new Equal();
            this["!="] = new Different();

            this["length"] = new Length();
            this["first"] = new First();
            this["second"] = new Second();
            this["third"] = new Third();
            this["last"] = new Last();
            this["head"] = new Head();
            this["tail"] = new Tail();
            this["empty"] = new Empty();
            this["push"] = new Push();
            this["append"] = new Append();
            this["range"] = new Range();

            this["println"] = new Println();
            this["error"] = new Error();
            this["readln"] = new Readln();

            this["num"] = new Num();
        }

		public void AddArguments(List<FSymbol> parameters, FList arguments)
		{
			for(int i = 0; i < parameters.Count; ++i)
			{
                this[parameters[i].name] = arguments[i];
            }
		}

        public object Clone()
        {
            Env clone = new();
            foreach (var (k, v) in this)
            {
                clone[k] = (IExpression) (v.Clone());
            }
            return clone;
        }
    }
}

