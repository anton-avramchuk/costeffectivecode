using System.Reflection;
//using System.Runtime.Loader;

namespace CostEffectiveCode.JSAdmin
{
    public class Program
    {
        //jsadmin.exe SuperPuperProj.SmartHead.dll 'path/to/project/dir'
        public static void Main(string[] args)
        {
            //var ctx = new MyAssemblyLoadContext();
           // AssemblyLoadContext.InitializeDefaultContext(ctx);

            var asm = Assembly.Load(new AssemblyName("MyAssembly1"));
        }

        //public class MyAssemblyLoadContext : AssemblyLoadContext
        //{
        //    protected override Assembly Load(AssemblyName assemblyName)
        //    {
        //        return base.LoadFromAssemblyPath("D:/Projects/HighTech/costeffectivecode/src//Costeffectivecode.WebApi2.Example/bin/CostEffectiveCode.WebApi2.Example.dll");
        //    }
        //}
    }
}

