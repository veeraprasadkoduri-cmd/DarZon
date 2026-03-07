using DarZon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DarZon.DAL
{
    public class Repository
    {

        public DARZANTESTEntities db = new DARZANTESTEntities();
        public OHEM GetUserDetails(OHEM user)
        {

            var usermaster = (from a in db.OHEMs where a.U_EMPCODE == user.U_EMPCODE && a.U_Password == user.U_Password select a).FirstOrDefault();
            return usermaster;
        }



    }

}