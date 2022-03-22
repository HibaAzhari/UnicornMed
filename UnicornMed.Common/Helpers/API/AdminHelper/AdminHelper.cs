using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicornMed.Common.Context;

namespace UnicornMed.Common.Helpers.API.AdminHelper
{
    public class AdminHelper : IAdminHelper
    {
        private readonly AppDbContext context;
        public AdminHelper(AppDbContext context)
        {
            this.context = context;
        }

        public bool IdExists(int id)
        {
            return context.Admins.Where(p => p.Id == id).Any();
        }

        public bool EmailExists(string email)
        {
            return context.Admins.Where(context => context.Email == email).Any();
        }
    }
}
