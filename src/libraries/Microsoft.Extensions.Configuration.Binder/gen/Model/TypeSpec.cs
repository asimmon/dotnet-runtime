﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis;
using SourceGenerators;

namespace Microsoft.Extensions.Configuration.Binder.SourceGeneration
{
    internal abstract record TypeSpec
    {
        public TypeSpec(ITypeSymbol type)
        {
            IsValueType = type.IsValueType;
            Namespace = type.ContainingNamespace?.ToDisplayString();
            DisplayString = type.ToMinimalDisplayString();
            IdentifierCompatibleSubstring = type.ToIdentifierCompatibleSubstring(useUniqueName: true);
            Name = Namespace + "." + DisplayString.Replace(".", "+");
            IsInterface = type.TypeKind is TypeKind.Interface;
        }

        public string Name { get; }

        public string DisplayString { get; }

        public string IdentifierCompatibleSubstring { get; }

        public string? Namespace { get; }

        public bool IsValueType { get; }

        public abstract TypeSpecKind SpecKind { get; }

        public virtual InitializationStrategy InitializationStrategy { get; set; }

        public virtual string? InitExceptionMessage { get; set; }

        public virtual bool CanInitialize => true;

        public virtual bool NeedsMemberBinding { get; }

        public virtual TypeSpec EffectiveType => this;

        public bool IsInterface { get; }

        protected bool CanInitComplexObject() => InitializationStrategy is not InitializationStrategy.None && InitExceptionMessage is null;
    }

    internal enum TypeSpecKind
    {
        Unknown = 0,
        ParsableFromString = 1,
        Object = 2,
        Enumerable = 3,
        Dictionary = 4,
        IConfigurationSection = 5,
        Nullable = 6,
    }
}
