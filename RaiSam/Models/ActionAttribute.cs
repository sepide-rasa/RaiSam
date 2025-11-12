using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class ActionAttribute : System.Attribute
    {
        public string TableName;
        public ActionAttribute(string TableName)
        {
            this.TableName = TableName;
        }
    }
}