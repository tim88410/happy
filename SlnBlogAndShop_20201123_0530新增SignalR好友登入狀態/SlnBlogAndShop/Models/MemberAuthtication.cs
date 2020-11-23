using prjBlog.ViewModel;
using SlnBlogAndShop;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace prjBlog.Models
{
    public class MemberAuthtication
    {

        //public List<TMember> isAuthticated(string email, string password)
        //{
        //    happyEntities db = new happyEntities();
        //    var p = from q in db.tMembers
        //            where q.fEmail == email && q.fPassword == password
        //            select q;
        //    List<TMember> list = new List<TMember>();
        //    TMember member = new TMember();
        //    if (p.Count() > 0)
        //    {
        //        member.fEmail = p.fEmail;
        //        member.fPassword = p.fPassword;
        //        member.fName = p.fName;
        //        member.fUsername = p.fUsername;
        //        member.fRegion = p.fRegion;
        //        member.fAddress = p.fAddress;
        //        member.fCity = p.fCity;
        //        member.fCountry = p.fCountry;
        //        member.fCoverPhoto = p.fCoverPhoto;
        //        member.fFax = p.fFax;
        //        member.fFirstname = p.fFirstname;
        //        member.fLastname = p.fLastname;
        //        member.fIdPhoto = p.fIdPhoto;
        //        member.fPhone = p.fPhone;
        //        member.fPostalcode = p.fPostalcode;
        //        list.Add(member);
        //        return list;
        //    }
        //    return null;
        //}
        public TMember isAuthticated(string email, string password)
        {
            happyEntities db = new happyEntities();
            tMember p = (from q in db.tMembers
                     where q.fEmail == email && q.fPassword == password
                     select q).FirstOrDefault();
            TMember member = new TMember();
            if (p!=null)
            {
                member.fMemberId = p.fMemberId;
                member.fEmail = p.fEmail;
                member.fPassword = p.fPassword;
                member.fName = p.fName;
                member.fUsername = p.fUsername;
                member.fRegion = p.fRegion;
                member.fAddress = p.fAddress;
                member.fCity = p.fCity;
                member.fCountry = p.fCountry;
                member.fCoverPhoto = p.fCoverPhoto;
                member.fFax = p.fFax;
                member.fFirstname = p.fFirstname;
                member.fLastname = p.fLastname;
                member.fIdPhoto = p.fIdPhoto;
                member.fPhone = p.fPhone;
                member.fPostalcode = p.fPostalcode;               
                return member;
            }
            return null;
        }
    }
}