   using System.IO;                           
   using System.Reflection;                   
   using Microsoft.CodeAnalysis;              
   using Microsoft.CodeAnalysis.CSharp;       
   using Xunit;

    namespace DotNetLib                        
    {                                          
        public  class Lib                
        {                                      
                                               
	    [Fact]                                   
            public int Compile()
            {                                  

               var tree = CSharpSyntaxTree.ParseText(@"
               using System;
               public class MyClass
               {
                   public static void Main()
                   {
                       Console.WriteLine(""Hello World!"");
                   }
               }");
               
               var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
               var compilation = CSharpCompilation.Create("MyCompilation",
                   syntaxTrees: new[] { tree }, references: new[] { mscorlib });
               
               //Emit to stream
               var ms = new MemoryStream();
               var emitResult = compilation.Emit(ms);

               //Load into currently running assembly. Normally we'd probably
               //want to do this in an AppDomain
               var ourAssembly = Assembly.Load(ms.GetBuffer());
               var type = ourAssembly.GetType("MyClass");
               
               //Invokes our main method and writes "Hello World" :)
               type.InvokeMember("Main", BindingFlags.Default | BindingFlags.InvokeMethod, null, null, null);
     	       return 0;                          
	    }
	}                                      
    }
