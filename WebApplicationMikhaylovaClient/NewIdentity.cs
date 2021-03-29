using WebApplicationMikhaylovaClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace WebApplicationMikhaylovaClient
{
    public static class MyIdentity
    {
        private static PateticoRPMEntities db = new PateticoRPMEntities();

        public static string FullName(this IIdentity identity)
        {
            string FullName = db.Users.Where(u => u.email == identity.Name).Select(u => u.name + " " + u.surname + " " + u.patronymic).FirstOrDefault();
            return FullName;
        }
    }
}