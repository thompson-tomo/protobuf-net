﻿#nullable enable
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using ProtoBuf.BuildTools.Internal;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ProtoBuf.Generators.ServiceGenerator;

namespace ProtoBuf.BuildTools.Generators
{
    /// <summary>
    /// Generates protobuf-net types from .proto schemas
    /// </summary>
    [Generator(LanguageNames.CSharp)]
    public sealed partial class ServiceGenerator : IIncrementalGenerator, ILoggingAnalyzer
    {
        private event Action<string>? Log;
        event Action<string>? ILoggingAnalyzer.Log
        {
            add => this.Log += value;
            remove => this.Log -= value;
        }

        void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
        {
            var nodes = context.SyntaxProvider.CreateSyntaxProvider(PreFilter, Parse)
                .Where(x => x is not null)
                .Select((x, _) => x!);
            var combined = context.CompilationProvider.Combine(nodes.Collect());
            context.RegisterImplementationSourceOutput(combined, Generate);
        }

        private bool PreFilter(SyntaxNode node, CancellationToken token)
            => node is InterfaceDeclarationSyntax;

        private ServiceDeclaration? Parse(GeneratorSyntaxContext context, CancellationToken token)
        {
            try
            {
                return context.Node is InterfaceDeclarationSyntax interfaceDeclaration
                    && context.SemanticModel.GetDeclaredSymbol(context.Node, token) is INamedTypeSymbol { TypeKind: TypeKind.Interface } type
                    ? ServiceDeclaration.TryCreate(context.Node, type) : null;
            }
            catch (Exception ex)
            {
                Log?.Invoke($"Unexpected error: '{ex.Message}' parsing {context.Node?.GetType()?.Name}");
                return null;
            }
        }

        private static bool ImplementsIGrpcService(ITypeSymbol type)
        {
            foreach (var i in type.Interfaces)
            {
                if (i.Name == "IGrpcService" && IsProtobufGrpcConfigurationNamespace(i.ContainingNamespace)
                    || ImplementsIGrpcService(i))
                {
                    return true;
                }
            }
            return false;
        }

        static bool IsProtobufGrpcConfigurationNamespace(INamespaceSymbol ns) => ns is
        {
            Name: "Configuration",
            ContainingNamespace:
            {
                Name: "Grpc",
                ContainingNamespace:
                {
                    Name: "ProtoBuf",
                    ContainingNamespace.IsGlobalNamespace: true
                }
            }
        };

        private void Generate(SourceProductionContext context, (Compilation Compilation, ImmutableArray<ServiceDeclaration> Right) tuple)
        {
            const string ServiceProxyName = "GeneratedServiceProxy";

            if (tuple.Right.IsDefaultOrEmpty) return; // nothing to do

            // check whether [Proxy] is available to us
            if (tuple.Compilation.GetTypeByMetadataName("ProtoBuf.Grpc.Configuration.ProxyAttribute") is null) return;

            var sb = CodeWriter.Create();

            int nextToken = 0;
            Dictionary<IMethodSymbol, (int Token, MethodDeclaration Method)> methodTokens = new(SymbolEqualityComparer.Default);
            foreach (var grp in tuple.Right.GroupBy(x => x.Service.Type.ContainingSymbol, SymbolEqualityComparer.Default))
            {
                if (HasNonPartialType(grp.Key, context, out var ns)) continue;

                if (ns is not null)
                {
                    sb.Append("namespace ").Append(ns).Indent();
                }

                sb.IndentToContainingType(grp.Key, out int nestLevels);
                if (ns is not null) nestLevels++;

                foreach (var svc in grp)
                {
                    int token = nextToken++;
                    var primaryType = svc.Service.Type;
                    if (!IsPartial(primaryType))
                    {
                        RequirePartialType(context, primaryType);
                        continue;
                    }

                    sb.Append("[global::ProtoBuf.Grpc.Configuration.ProxyAttribute(typeof(" + ServiceProxyName).Append(token).Append("))]").NewLine();
                    sb.Append("partial ").Append(CodeWriter.TypeLabel(primaryType)).Append(" ").Append(primaryType.Name);
                    if (IsAtLeast(svc.Syntax, LanguageVersion.CSharp8))
                    {
                        // write nested
                        sb.Indent();
                        WriteProxy(sb, svc, methodTokens, token, "private");
                        sb.Outdent();
                    }
                    else
                    {
                        // write alongside
                        sb.Append(" { }").NewLine().NewLine();
                        WriteProxy(sb, svc, methodTokens, token, "private");
                    }
                }
                sb.Outdent(nestLevels);
            }

            static bool IsAtLeast(SyntaxNode syntax, LanguageVersion version) =>
                syntax?.SyntaxTree.Options is CSharpParseOptions options
                    && options.LanguageVersion >= version;

            static void WriteProxy(CodeWriter sb, ServiceDeclaration proxy, Dictionary<IMethodSymbol, (int Token, MethodDeclaration Method)> methodTokens, int token, string accessibility)
            {
                methodTokens.Clear();

                var primaryType = proxy.Service.Type;
                sb.Append(accessibility).Append(string.IsNullOrEmpty(accessibility) ? "" : " ").Append("sealed").Append(" class " + ServiceProxyName).Append(token).Append(" : global::Grpc.Core.ClientBase<" + ServiceProxyName).Append(token).Append(">, ").Append(primaryType);

                sb.Indent();

                sb.Append("// public " + ServiceProxyName).Append(token).Append("() : base() {}").NewLine()
                    .Append("// public " + ServiceProxyName).Append(token).Append("(global::Grpc.Core.ChannelBase channel) : base(channel) {}").NewLine()
                    .Append("public " + ServiceProxyName).Append(token).Append("(global::Grpc.Core.CallInvoker callInvoker) : base(callInvoker) {}").NewLine()
                    .Append("private " + ServiceProxyName).Append(token).Append("(global::Grpc.Core.ClientBase.ClientBaseConfiguration configuration) : base(configuration) {}").NewLine()
                    .Append("protected override " + ServiceProxyName).Append(token).Append(" NewInstance(global::Grpc.Core.ClientBase.ClientBaseConfiguration configuration) => new " + ServiceProxyName).Append(token).Append("(configuration);").NewLine()
                    .NewLine();

                // write all the static service key/method members
                int subServiceIndex = 0, methodIndex = 0;
                foreach (var subService in proxy.Methods.GroupBy(x => x.Service))
                {
                    sb.NewLine().Append("private const string _pbn_Service").Append(subServiceIndex).Append(" = ").AppendVerbatimLiteral(subService.Key.Name).Append(";").NewLine();
                    foreach (var method in subService)
                    {
                        sb.Append("private static readonly global::Grpc.Core.Method<").Append(method.RequestType).Append(", ").Append(method.ResponseType).Append("> _pbn_Method").Append(methodIndex).Append(" = new global::Grpc.Core.Method<").Append(method.RequestType).Append(", ").Append(method.ResponseType).Append(">(")
                            .Append("global::Grpc.Core.MethodType.").Append(method.MethodKind).Append(", _pbn_Service").Append(subServiceIndex).Append(", ").AppendVerbatimLiteral(method.MethodName)
                            .Append(", null!, null!);").NewLine();
                        methodTokens.Add(method.Method, (methodIndex++, method));
                    }
                }

                // write the methods
                AddServiceImplementations(sb, methodTokens, primaryType);

                sb.Outdent();

            }

            var s = sb.ToStringRecycle();
            if (!string.IsNullOrWhiteSpace(s))
            {
                context.AddSource("protobuf-net.services.generated.cs", SourceText.From(s, Encoding.UTF8));
            }

            static void RequirePartialType(in SourceProductionContext context, INamedTypeSymbol type)
            {
                context.ReportDiagnostic(Diagnostic.Create(Diagnostics.PartialTypeRequired, type.Locations.FirstOrDefault(), type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
            }
            static bool IsPartial(INamedTypeSymbol type)
            {
                foreach (var syntax in type.DeclaringSyntaxReferences)
                {
                    if (syntax.GetSyntax() is TypeDeclarationSyntax decl && decl.Modifiers.Any(x => x.IsKind(SyntaxKind.PartialKeyword)))
                    {
                        return true;
                    }
                }
                return false;
            }

            static bool HasNonPartialType(ISymbol container, in SourceProductionContext context, out INamespaceSymbol? ns)
            {
                if (container is INamespaceSymbol containerNs)
                {
                    ns = containerNs;
                    return false;
                }
                var type = container as INamedTypeSymbol;
                bool hasAnyNonPartial = false;
                ns = null;
                while (type is not null)
                {
                    if (!IsPartial(type))
                    {
                        RequirePartialType(context, type);
                        hasAnyNonPartial = true;
                    }
                    ns = type.ContainingNamespace;
                    type = type.ContainingType;
                }
                if (ns is { IsGlobalNamespace: true })
                {
                    // export as null
                    ns = null;
                }
                return hasAnyNonPartial;
            }
        }

        static bool SkipMember(ISymbol symbol, out bool silently, bool forInterface)
        {
            silently = false;
            if (symbol is IMethodSymbol method)
            {
                if (!method.IsAbstract)
                {
                    // DIM exists; lean on it
                    silently = true;
                    return true;
                }
                switch (method.MethodKind)
                {
                    case MethodKind.EventAdd:
                    case MethodKind.EventRaise:
                    case MethodKind.EventRemove:
                    case MethodKind.PropertyGet:
                    case MethodKind.PropertySet:
                    case MethodKind.UserDefinedOperator when !forInterface:
                    case MethodKind.BuiltinOperator when !forInterface:
                        // handled separately
                        silently = true;
                        return true;
                }
            }
            return false;
        }

        private static void AddServiceImplementations(CodeWriter sb,
            Dictionary<IMethodSymbol, (int Token, MethodDeclaration Method)> tokens, INamedTypeSymbol type)
        {
            sb.NewLine().Append("// implement ").Append(type).NewLine();
            foreach (var member in type.GetMembers())
            {
                if (SkipMember(member, out bool silently, true))
                {
                    if (!silently)
                    {
                        sb.Append("// skip ").Append(member.Name).NewLine();
                    }
                    continue; // only want regular instance members, not default implementations etc
                }
                switch (member)
                {
                    case IMethodSymbol method:
                        sb.Append(method.IsStatic ? "static " : "").Append(method.ReturnType).Append(" ").Append(type).Append(".");
                        switch (method.MethodKind)
                        {
                            case MethodKind.BuiltinOperator:
                            case MethodKind.UserDefinedOperator:
                                sb.Append("operator ").Append(GetOperatorName(method));
                                break;
                            default:
                                sb.Append(method.Name);
                                break;
                        }
                        bool first;
                        if (method.Arity != 0)
                        {
                            sb.Append("<");
                            first = true;
                            foreach (var t in method.TypeArguments)
                            {
                                sb.Append(first ? "" : ", ").Append(t.Name);
                                first = false;
                            }
                            sb.Append(">");
                        }
                        sb.Append("(");
                        first = true;
                        foreach (var p in method.Parameters)
                        {
                            sb.Append(first ? "" : ", ").Append(p.Type).Append(" ").Append(p.Name);
                            first = false;
                        }
                        sb.Append(")");

                        if (tokens.TryGetValue(method, out var pair))
                        {
                            sb.Indent();
                            sb.Append("throw new global::System.NotImplementedException(").AppendVerbatimLiteral(pair.Method.MethodName).Append("); // via _pbn_Method").Append(pair.Token);
                            sb.Outdent();
                        }
                        else if (method is { Name: nameof(IDisposable.Dispose) } && type is
                        {
                            Name: nameof(IDisposable), ContainingType: null,
                            ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true }
                        })
                        {
                            sb.Append(" { }").NewLine();
                        }
                        else if (method is { Name: nameof(IAsyncDisposable.DisposeAsync) } && type is
                        {
                            Name: nameof(IAsyncDisposable), ContainingType: null,
                            ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true }
                        })
                        {
                            sb.Append(" => default;").NewLine();
                        }
                        else
                        {
                            sb.Append(" => throw new global::System.NotSupportedException();").NewLine();
                        }
                        break;
                    case IPropertySymbol prop:
                        sb.Append(prop.Type).Append(" ").Append(type).Append(".").Append(member.Name);
                        if (prop.GetMethod is not null && prop.SetMethod is null)
                        {
                            sb.Append(" => throw new global::System.NotSupportedException();").NewLine();
                        }
                        else
                        {
                            sb.Indent();
                            if (prop.GetMethod is not null) sb.Append("get => throw new global::System.NotSupportedException();").NewLine();
                            if (prop.SetMethod is not null) sb.Append("set => throw new global::System.NotSupportedException();").NewLine();
                            sb.Outdent();
                        }
                        break;
                    case IEventSymbol evt:
                        sb.Append("event ").Append(evt.Type).Append(" ").Append(type).Append(".").Append(member.Name);
                        sb.Indent();
                        if (evt.AddMethod is not null) sb.Append("add => throw new global::System.NotSupportedException();").NewLine();
                        if (evt.RemoveMethod is not null) sb.Append("remove => throw new global::System.NotSupportedException();").NewLine();
                        sb.Outdent();
                        break;
                    default:
                        sb.Append($"#error {member.Kind} {member.Name}").NewLine();
                        break;
                }
            }


            foreach (var iType in type.Interfaces)
            {
                AddServiceImplementations(sb, tokens, iType);
            }
        }

        private static string GetOperatorName(IMethodSymbol method)
            => method.Name switch
            {
                "op_Addition" => "+",
                "op_Subtraction" => "-",
                "op_Multiply" => "*",
                "op_Division" => "/",
                "op_CheckedAddition" => "checked +",
                "op_CheckedSubtraction" => "checked -",
                "op_CheckedMultiply" => "checked *",
                "op_CheckedDivision" => "checked /",

                "op_UnaryNegation" => "-",
                "op_Increment" => "++",
                "op_Decrement" => "--",
                "op_CheckedUnaryNegation" => "checked -",
                "op_CheckedIncrement" => "checked ++",
                "op_CheckedDecrement" => "checked --",

                "op_True" => "true",
                "op_False" => "false",
                "op_UnaryPlus" => "+",
                "op_LogicalNot" => "!",
                "op_OnesComplement" => "~",

                "op_Modulus" => "%",
                "op_BitwiseAnd" => "&",
                "op_BitwiseOr" => "|",
                "op_ExclusiveOr" => "^",
                "op_LeftShift" => "<<",
                "op_RightShift" => ">>",
                "op_UnsignedRightShift" => ">>>",
                "op_Equality" => "==",
                "op_Inequality" => "!=",
                "op_GreaterThan" => ">",
                "op_LessThan" => "<",
                "op_GreaterThanOrEqual" => ">=",
                "op_LessThanOrEqual" => "<=",
                _ => method.Name,
            };

        private sealed class ServiceDeclaration
        {
            static void AddMethods(List<MethodDeclaration> methods, in ServiceEndpoint service)
            {
                foreach (var member in service.Type.GetMembers())
                {
                    if (member is IMethodSymbol method &&
                        MethodDeclaration.TryCreate(service, method, out var svcMethod))
                    {
                        methods.Add(svcMethod);
                    }
                }

                // cascade sub-services
                foreach (var iType in service.Type.Interfaces)
                {
                    if (ServiceEndpoint.TryCreate(iType, out var iService))
                    {
                        AddMethods(methods, iService);
                    }
                }
            }

            public static ServiceDeclaration? TryCreate(SyntaxNode syntax, INamedTypeSymbol type)
                => ServiceEndpoint.TryCreate(type, out var service) ? new(syntax, service) : null;


            private ServiceDeclaration(SyntaxNode syntax, in ServiceEndpoint service)
            {
                Service = service;
                Syntax = syntax;

                List<MethodDeclaration> methods = new();
                AddMethods(methods, service);
                Methods = ImmutableArray.CreateRange(methods);
            }

            public SyntaxNode Syntax { get; }
            public ServiceEndpoint Service { get; }
            public ImmutableArray<MethodDeclaration> Methods { get; }
        }

        public readonly struct ServiceEndpoint : IEquatable<ServiceEndpoint>
        {
            static bool IsService(INamedTypeSymbol type, out string serviceName)
            {
                AttributeData? sa = null, sca = null;
                foreach (var attrib in type.GetAttributes())
                {
                    var ac = attrib.AttributeClass;

                    if (ac is null || ac.ContainingType is not null) continue; // we don't expect any known attributes to be inner types
                    if (ac.Name == "ServiceAttribute" &&
                        IsProtobufGrpcConfigurationNamespace(ac.ContainingNamespace))
                    {
                        sa = attrib;
                    }
                    else if (ac.Name == "ServiceContractAttribute" &&
                        ac.ContainingNamespace is
                        {
                            Name: "ServiceModel",
                            ContainingNamespace:
                            {
                                Name: "System",
                                ContainingNamespace.IsGlobalNamespace: true
                            }
                        })
                    {
                        sca = attrib;
                    }
                }

                if (sa is null && sca is null && !ImplementsIGrpcService(type))
                {
                    serviceName = "";
                    return false;
                }

                serviceName = type.Name;
                return true;
            }

            public readonly INamedTypeSymbol Type;
            public readonly string Name;

            internal static bool TryCreate(INamedTypeSymbol type, out ServiceEndpoint service)
            {
                if (IsService(type, out var name))
                {
                    service = new(type, name);
                    return true;
                }
                service = default;
                return false;
            }
            private ServiceEndpoint(INamedTypeSymbol type, string name)
            {
                Type = type;
                Name = name;
            }
            public override string ToString() => Name;
            public override int GetHashCode() => SymbolEqualityComparer.Default.GetHashCode(Type);
            public override bool Equals(object obj) => obj is ServiceEndpoint other && Equals(other);
            public bool Equals(ServiceEndpoint other) => SymbolEqualityComparer.Default.Equals(Type, other.Type);
        }

        private readonly struct MethodDeclaration
        {
            public static bool TryCreate(in ServiceEndpoint service, IMethodSymbol method, out MethodDeclaration decl)
            {
                if (!(SkipMember(method, out _, false) || method.ReturnsVoid || method.Parameters.IsDefaultOrEmpty))
                {
                    var req = method.Parameters[0].Type;
                    var resp = method.ReturnType;
                    var flags = MethodFlags.None;
                    if (req is INamedTypeSymbol namedReq && resp is INamedTypeSymbol namedResp)
                    {
                        decl = new(service, method, namedReq, namedResp, flags);
                        return true;
                    }
                }
                decl = default;
                return false;
            }

            [Flags]
            public enum MethodFlags
            {
                None = 0,
                RequestStreaming = 1 << 0,
                ResponseStreaming = 2 << 0,
            }

            private MethodDeclaration(in ServiceEndpoint service, IMethodSymbol method, INamedTypeSymbol requestType, INamedTypeSymbol responseType, MethodFlags flags)
            {
                Service = service;
                Method = method;

                // TODO: lots more
                MethodName = method.Name;
                RequestType = requestType;
                ResponseType = responseType;
                Flags = flags;
            }
            public IMethodSymbol Method { get; }
            public ServiceEndpoint Service { get; }
            public string MethodName { get; }
            public INamedTypeSymbol RequestType { get; }
            public INamedTypeSymbol ResponseType { get; }
            public MethodFlags Flags { get; }

            public string MethodKind => (Flags & (MethodFlags.RequestStreaming | MethodFlags.ResponseStreaming)) switch
            {
                MethodFlags.None => "Unary",
                MethodFlags.RequestStreaming => "ClientStreaming",
                MethodFlags.ResponseStreaming => "ServerStreaming",
                _ => "DuplexStreaming",
            };
        }
    }
}