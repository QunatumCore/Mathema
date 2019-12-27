﻿using Mathema.Enums.DimensionKeys;
using Mathema.Enums.Operators;
using Mathema.Interfaces;
using Mathema.Models.Dimension;
using Mathema.Models.Expressions;
using Mathema.Models.FlatExpressions;
using Mathema.Models.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathema.Models.ExpressionOperations
{
    public class FlatMultOperations
    {
        static FlatMultOperations()
        {
            BinaryOperations = new Dictionary<OperatorTypes, Func<IExpression, IExpression, IExpression>>();
            UnaryOperations = new Dictionary<OperatorTypes, Func<IExpression, IExpression>>();
            RegiterOperations();
        }

        public static Dictionary<OperatorTypes, Func<IExpression, IExpression, IExpression>> BinaryOperations { get; }

        public static Dictionary<OperatorTypes, Func<IExpression, IExpression>> UnaryOperations { get; }

        private static void RegiterOperations()
        {
            BinaryOperations.Add(OperatorTypes.Add, Add);
            BinaryOperations.Add(OperatorTypes.Subtract, Subtract);
            BinaryOperations.Add(OperatorTypes.Divide, Divide);
            BinaryOperations.Add(OperatorTypes.Multiply, Multiply);
            BinaryOperations.Add(OperatorTypes.Power, Pow);

            UnaryOperations.Add(OperatorTypes.Sign, Sign);
        }

        public static IExpression Add(IExpression lhe, IExpression rhe)
        {
            var res = (FlatMultExpression)(lhe.Clone());
            if (DimensionKey.Compare(res.DimensionKey, rhe.DimensionKey))
            {
                if (res.Expressions.ContainsKey(Dimensions.Number))
                {
                    res.Expressions[Dimensions.Number][0].Count.Add(rhe.Count);
                }
                else
                {
                    var num = new NumberExpression(rhe.Count);
                    num.Count.Add(new Fraction(1, 1));
                    res.Add(num);
                }
                res.Count.Add(rhe.Count);
                return res;
            }

            return null;
        }

        public static IExpression Subtract(IExpression lhe, IExpression rhe)
        {
            var res = lhe.Clone();
            if (DimensionKey.Compare(res.DimensionKey, rhe.DimensionKey))
            {
                res.Count.Subtract(rhe.Count);
                return res;
            }

            return null;
        }

        public static IExpression Multiply(IExpression lhe, IExpression rhe)
        {
            var res = lhe.Clone();
            var lc = (IFlatExpression)res;
            if (rhe is INumberExpression)
            {
                foreach (var key in rhe.DimensionKey.Key)
                {
                    lc.Expressions[Dimensions.Number].Add(rhe);
                }

                return res;
            }
            else if (rhe is IVariableExpression)
            {
                var tmp = rhe.DimensionKey.Key.ElementAt(0).Key;
                if (lc.Expressions.ContainsKey(tmp))
                {
                    lc.Expressions[tmp].Add(rhe);
                }
                else
                {
                    lc.Expressions.Add(tmp, new List<IExpression>() { rhe });
                }

                foreach (var key in rhe.DimensionKey.Key)
                {
                    res.DimensionKey.Add(key.Key, key.Value);
                }

                return res;
            }
            else if (rhe is IFlatMultExpression)
            {
                foreach (var key in rhe.DimensionKey.Key)
                {
                    res.DimensionKey.Add(key.Key, key.Value);
                }

                return res;
            }

            return null;
        }

        public static IExpression Divide(IExpression lhe, IExpression rhe)
        {
            var res = lhe.Clone();
            if (rhe is IVariableExpression)
            {
                res.Count.Divide(rhe.Count);

                foreach (var key in rhe.DimensionKey.Key)
                {
                    res.DimensionKey.Remove(key.Key, key.Value);
                }

                return res;
            }

            return null;
        }

        public static IExpression Pow(IExpression lhe, IExpression rhe)
        {
            var res = lhe.Clone();
            var lc = (IFlatExpression)res;
            if (rhe is INumberExpression)
            {
                foreach (var kv in lc.Expressions)
                {
                    for (int i = 0; i < kv.Value.Count; i++)
                    {
                        var expr = kv.Value[i];
                        kv.Value[i] = expr.BinaryOperations[OperatorTypes.Power](expr, rhe);
                    }
                }

                return res;
            }

            return null;
        }

        public static IExpression Sign(IExpression rhe)
        {
            var res = rhe.Clone();
            res.Count.Numerator *= -1;
            return res;
        }
    }
}
