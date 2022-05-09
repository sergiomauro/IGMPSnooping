using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace IGMPSnooping.Validation
{
    public class Validator
    {
        public Validator()
        {

        }

        private IPAddress _ip;
        private Int32 _int;

        public bool IsIpValid(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return false;
            }

            if(value.Split(".").Length != 4)
            {
                return false;
            }

            return IPAddress.TryParse(value, out _ip);
        }

        public bool IsPortValid(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return false;
            }

            if (value.Length > 5)
            {
                return false;
            }

            return Int32.TryParse(value, out _int);
        }
    }
}
