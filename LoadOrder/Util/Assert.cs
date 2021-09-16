namespace LoadOrderTool.Util {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class Assertion {
        static string Concat(string m1, string m2) {
            if (m2 != null)
                m2 = " : " + m2;
            return m1 + m2;
        }
        public static void Assert(bool condition, string m = null) {
            if (!condition)
                throw new Exception(Concat("Assertion failed.", m));
        }

        public static void NotNull(object obj, string m = null) {
            if (obj == null)
                throw new Exception(Concat("Assertion.NotNull failed.", m));
        }

        public static void Neq(dynamic a, dynamic b, string m = null) {
            if (a == b)
                throw new Exception(Concat($"Assertion failed : {a} != {b} ", m));
        }

        public static void Eq(dynamic a, dynamic b, string m = null) {
            if (a != b)
                throw new Exception(Concat($"Assertion failed : {a} == {b} ", m));
        }

        public static void GT(dynamic a, dynamic b, string m = null) {
            if (!(a > b))
                throw new Exception(Concat($"Assertion failed : {a} > {b} ", m));
        }
        public static void GTEq(dynamic a, dynamic b, string m = null) {
            if (!(a >= b))
                throw new Exception(Concat($"Assertion failed : {a} >= {b} ", m));
        }
        public static void LT(dynamic a, dynamic b, string m = null) {
            if (!(a < b))
                throw new Exception(Concat($"Assertion failed : {a} < {b} ", m));
        }
        public static void LTEq(dynamic a, dynamic b, string m = null) {
            if (!(a <= b))
                throw new Exception(Concat($"Assertion failed : {a} <= {b} ", m));
        }

    }
}
