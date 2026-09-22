using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.Generic
{
    public interface IUpdateAddService
    {
        DataRow Update(BaseDTO DTO);
        DataRow AddNew(BaseDTO DTO);
    }
}
