﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace silkveil.net.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Exceptions {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Exceptions() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("silkveil.net.Resources.Exceptions", typeof(Exceptions).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Der von Ihnen angegebene Link ist ungültig. Bitte wenden Sie sich an Ihren Administrator, um die Verfügbarkeit des Links zu prüfen..
        /// </summary>
        public static string ConstraintViolationMessage {
            get {
                return ResourceManager.GetString("ConstraintViolationMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ungültiger Link.
        /// </summary>
        public static string ConstraintViolationTitle {
            get {
                return ResourceManager.GetString("ConstraintViolationTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Es ist ein interner Serverfehler aufgetreten. Bitte wenden Sie sich an Ihren Administrator, wenn das Problem erneut auftritt..
        /// </summary>
        public static string InternalServerErrorMessage {
            get {
                return ResourceManager.GetString("InternalServerErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Interner Serverfehler.
        /// </summary>
        public static string InternalServerErrorTitle {
            get {
                return ResourceManager.GetString("InternalServerErrorTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Der von Ihnen angegebene Link wurde nicht gefunden. Bitte wenden Sie sich an Ihren Administrator, um die Gültigkeit des Links zu prüfen..
        /// </summary>
        public static string MappingNotFoundMessage {
            get {
                return ResourceManager.GetString("MappingNotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ungültiger Link.
        /// </summary>
        public static string MappingNotFoundTitle {
            get {
                return ResourceManager.GetString("MappingNotFoundTitle", resourceCulture);
            }
        }
    }
}
