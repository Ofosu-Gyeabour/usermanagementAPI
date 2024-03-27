using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TBarCodeOp
    {
        public TBarCodeOp()
        {
            TBarCodeGenerators = new HashSet<TBarCodeGenerator>();
        }

        /// <summary>
        /// primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// code for the operation
        /// </summary>
        public string? Opbcode { get; set; }
        /// <summary>
        /// description of operation
        /// </summary>
        public string? BcodeDescrib { get; set; }

        public virtual ICollection<TBarCodeGenerator> TBarCodeGenerators { get; set; }
    }
}
