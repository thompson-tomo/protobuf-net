﻿using Microsoft.CodeAnalysis;
using ProtoBuf.Internal.CodeGen;
using ProtoBuf.Reflection.Internal.CodeGen;
using System;
using System.Linq;

namespace ProtoBuf.BuildTools.Analyzers;

[Generator(LanguageNames.CSharp)]
public sealed class DataContractGenerator : ISourceGenerator
{
    void ISourceGenerator.Initialize(GeneratorInitializationContext context) { }

    void ISourceGenerator.Execute(GeneratorExecutionContext context)
    {
        try
        {
            var parser = new CodeGenSemanticModelParser(in context);
            foreach (var tree in context.Compilation.SyntaxTrees)
            {
                parser.Parse(context.Compilation, tree);
            }
            var parsedFromCode = parser.Process();
            if (parsedFromCode.Files.Count == 0) return;
            var generator = new CodeGenCSharpCodeGenerator();
            foreach (var codeFile in generator.Generate(parsedFromCode))
            {
                context.AddSource(codeFile.Name, codeFile.Text);
            }
        }
        catch (Exception ex)
        {
            context.ReportDiagnostic(Diagnostic.Create(DataContractAnalyzer.UnknownError, null, ex.Message, ex?.GetType()?.Name));
        }
    }

}