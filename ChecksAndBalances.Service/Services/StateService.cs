using ChecksAndBalances.Data.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecksAndBalances.Service.Services
{
    public class StateService
    {
        public static IEnumerable<State> States { get; private set; }
        
        static StateService()
        {
            States = Enum.GetValues(typeof(State)).Cast<State>();
        }
    }
}
