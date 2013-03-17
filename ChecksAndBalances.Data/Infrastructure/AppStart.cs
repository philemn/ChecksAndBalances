using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Infrastructure;
using Ninject.Extensions.Conventions;
using System.Data.Entity;
using ChecksAndBalances.Data.Storage.Context;
using ChecksAndBalances.Data.Storage.Context.SimpleMembership;
using System.Threading;

namespace ChecksAndBalances.Data.Infrastructure
{
    public class AppStart
    {
        public static void RegisterAssemblies(IKernel kernel)
        {
            if (kernel == null)
                throw new ArgumentException("can not be null", "kernel");

            kernel.Bind(x => x
                .FromThisAssembly()
                .SelectAllClasses()
                .BindAllInterfaces()
                .Configure(b => b.InTransientScope()));

            RegisterDatabaseInitializer();
        }

        public static void RegisterDatabaseInitializer()
        {
            //Database.SetInitializer<ChecksAndBalancesContext>(new ChecksAndBalancesInitializer());
            Database.SetInitializer<ChecksAndBalancesContext>(null);
        }
    }
}
