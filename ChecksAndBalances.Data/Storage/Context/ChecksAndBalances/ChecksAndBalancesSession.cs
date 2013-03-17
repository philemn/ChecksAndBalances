using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecksAndBalances.Data.Storage.Context
{
    public interface IChecksAndBalancesSession : ISession { }

    public class ChecksAndBalancesSession : Sessions.EFCodeFirstSession, IChecksAndBalancesSession
    {
        public ChecksAndBalancesSession() : base(new ChecksAndBalancesContext())
        {
            
        }
    }
}
