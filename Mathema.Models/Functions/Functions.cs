﻿using Mathema.Enums.Functions;
using Mathema.Interfaces;
using Mathema.Models.FunctionExpressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mathema.Models.Functions
{
    public class Functions
    {
        public static Dictionary<string, Func<IExpression, IFunctionExpression>> All { get; } = GetAllFunctions();

        private static Dictionary<string, Func<IExpression, IFunctionExpression>> GetAllFunctions()
        {
            var result = new Dictionary<string, Func<IExpression, IFunctionExpression>>();
            result.Add(FunctionTypes.Sin.ToString().ToLower(), arg => new SinExpression(FunctionTypes.Sin, arg));
            result.Add(FunctionTypes.Cos.ToString().ToLower(), arg => new SinExpression(FunctionTypes.Cos, arg));
            result.Add(FunctionTypes.Tan.ToString().ToLower(), arg => new SinExpression(FunctionTypes.Tan, arg));
            result.Add(FunctionTypes.Cot.ToString().ToLower(), arg => new SinExpression(FunctionTypes.Cot, arg));
            result.Add(FunctionTypes.Log.ToString().ToLower(), arg => new SinExpression(FunctionTypes.Log, arg));

            return result;
        }

        public static IFunctionExpression Get(string function, IExpression arg)
        {
            return All[function.ToLower()](arg);
        }
    }
}
