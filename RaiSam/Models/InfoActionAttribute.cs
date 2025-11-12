using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaiSam.Models
{
    public class InfoActionAttribute : System.Attribute
    {
        public string TableName;
        public InfoActionAttribute(string TableName)
        {
            this.TableName = TableName;
        }
    }
}