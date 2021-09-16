using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace LoadOrderTool.Util {
    public static class Helpers {
        internal static string ThisMethod {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => CurrentMethod(2);
        }

        private static string JoinArgs(object[] args) {
            if (args.IsNullorEmpty())
                return "";
            else
                return args.Select(a => a.ToSTR()).Join(", ");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static string CurrentMethod(int i = 1, params object[] args) {
            var method = new StackFrame(i).GetMethod();
            return $"{method.DeclaringType.Name}.{method.Name}({JoinArgs(args)})";
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static string CurrentMethodFull(int i = 1, params object[] args) {
            var method = new StackFrame(i).GetMethod();
            if (args.IsNullorEmpty()) {
                var parameters = method
                    .GetParameters()
                    .Select(p => $"{p.ParameterType.Name} {p.Name}")
                    .Join(" ,");
                return $"{method.FullName()}({parameters})";
            } else {
                return $"{method.FullName()}({JoinArgs(args)})";
            }
        }
    }

    public static class TypeExtensions {
        internal static Version Version(this Assembly asm) =>
          asm.GetName().Version;

        internal static Version VersionOf(this Type t) =>
            t.Assembly.GetName().Version;

        internal static Version VersionOf(this object obj) =>
            VersionOf(obj.GetType());

        internal static string Name(this Assembly assembly) => assembly.GetName().Name;

        public static string FullName(this MethodBase m) =>
            m.DeclaringType.FullName + "." + m.Name;
    }

    public static class EnumerationExtensions {
        /// <summary>
        /// fast way of determining if collection is null or empty
        /// </summary>
        internal static bool IsNullorEmpty<T>(this ICollection<T> a)
            => a == null || a.Count == 0;

        /// <summary>
        /// generic way of determining if IEnumerable is null or empty
        /// </summary>
        internal static bool IsNullorEmpty<T>(this IEnumerable<T> a) {
            return a == null || !a.Any();
        }

        internal static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> a) where T : class =>
            a ?? Enumerable.Empty<T>();
        internal static IEnumerable<T?> EmptyIfNull<T>(this IEnumerable<T?> a) where T : struct =>
            a ?? Enumerable.Empty<T?>();
    }


    public static class StringExtensions {
        public static string Join(this IEnumerable<string> strings, string del) => string.Join(del, strings.ToArray());
        public static string Join(this string[] strings, string del) => string.Join(del, strings);

        internal static string ToSTR(this object obj) {
            if (obj is null) return "<null>";
            if (obj is string str)
                return str.ToSTR();
            if (obj is IEnumerable list)
                return list.ToSTR();
            return obj.ToString();
        }

        /// <summary>
        ///  - returns "null" if string is null
        ///  - returns "empty" if string is empty
        ///  - returns string otherwise.
        /// </summary>
        internal static string ToSTR(this string str) {
            if (str == "") return "<empty>";
            if (str == null) return "<null>";
            return str;
        }

        /// <summary>
        /// prints all items of the list with the given format.
        /// throws exception if T.ToString(format) does not exists.
        /// </summary>
        internal static string ToSTR(this IEnumerable list) {
            if (list is null)
                return "<null>";
            string ret = "{ ";
            int count = 0;
            foreach (object item in list) {
                count++;
                MethodInfo mToString = item.GetType().GetMethod("ToString", new Type[0])
                    ?? throw new Exception($"{item.GetType().Name}.ToString() was not found");
                var s = mToString.Invoke(item, null);
                ret += $"{s}, ";
            }
            if(count > 0 )
                ret = ret.Remove(ret.Length - 2, 2);
            ret += " }";
            return ret;
        }

        /// <summary>
        /// prints all items of the list with the given format.
        /// throws exception if T.ToString(format) does not exists.
        /// </summary>
        internal static string ToSTR(this IEnumerable list, string format) {
            if (list is null)
                return "<null>";
            var arg = new object[] { format };
            string ret = "{ ";
            foreach (object item in list) {
                MethodInfo mToString = item.GetType().GetMethod("ToString", new[] { typeof(string) })
                    ?? throw new Exception($"{item.GetType().Name}.ToString(string) was not found");
                var s = mToString.Invoke(item, arg);
                ret += $"{s}, ";
            }
            ret.Remove(ret.Length - 2, 2);
            ret += " }";
            return ret;
        }

        internal static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        internal static string SpaceCamelCase(this string camelCase) =>
            Regex.Replace(camelCase, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
    }
}
