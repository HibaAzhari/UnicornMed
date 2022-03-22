using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicornMed.Common.Helpers.API.AdminHelper
{
    public interface IAdminHelper
    {
        public bool IdExists(int id);
        public bool EmailExists(string email);
    }
}
