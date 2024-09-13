using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Entities
{
     public class FileEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string FilePath { get; set; }=string.Empty ;
        public long Size { get; set; }
        public string ContentType { get; set; }= string.Empty ;

    }
}
