using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

namespace LoadOrderIPatch {
    internal static class TypeDefinitionExtensions {

        /// <summary>
        /// Test if type is subclass. Only first base type 
        /// </summary>
        /// <param name="childType"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static bool IsSubclass(this TypeDefinition childType, IEnumerable<TypeDefinition> parent) {
            return childType.BaseType != null && parent.Any(p => childType.BaseType.FullName.Equals(p.FullName));
        }

        public static bool ExtendsAnyOfBaseClasses(this TypeDefinition childType, IEnumerable<TypeDefinition> parent) {
            if (childType.BaseType == null) {
                return false;
            }

            IEnumerable<TypeDefinition> typeDefinitions = parent as TypeDefinition[] ?? parent.ToArray();
            return typeDefinitions.Any(p => childType.BaseType.FullName.Equals(p.FullName)) ||
                childType.BaseType.Resolve().ExtendsAnyOfBaseClasses(typeDefinitions);
        }

        public static bool ExtendsAnyOfBaseClasses(this TypeDefinition childType, IEnumerable<string> parent) {
            if (childType.BaseType == null) {
                return false;
            }

            IEnumerable<string> typeDefinitions = parent as string[] ?? parent.ToArray();
            return typeDefinitions.Any(p => childType.BaseType.Name.Equals(p)) ||
                childType.BaseType.Resolve().ExtendsAnyOfBaseClasses(typeDefinitions);
        }

        /// <summary>
        /// Returns base class name.
        /// </summary>
        /// <param name="childType"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static string ExtendedParentTypeName(this TypeDefinition childType, IEnumerable<TypeDefinition> parent) {
            if (childType.BaseType == null) {
                return childType.Name;
            }


            IEnumerable<TypeDefinition> typeDefinitions = parent as TypeDefinition[] ?? parent.ToArray();
            if (!typeDefinitions.Any(p => childType.BaseType.FullName.Equals(p.FullName))) {
                return childType.BaseType.Resolve().ExtendedParentTypeName(typeDefinitions);
            }

            return childType.BaseType.Name;
        }

        #region singular versions

        public static bool IsSubclass(this TypeDefinition childType, TypeDefinition parent) {
            return childType.BaseType != null
                && childType.BaseType.FullName.Equals(parent.FullName);
        }

        public static bool Extends(this TypeDefinition childType, string parent) {
            return childType.BaseType.Name.Equals(parent)
                || childType.BaseType.Resolve().Extends(parent);
        }

        public static bool Extends(this TypeDefinition childType, TypeDefinition parent) {
            return childType.BaseType.FullName.Equals(parent.FullName)
                || childType.BaseType.Resolve().Extends(parent);
        }

        #endregion

    }
}