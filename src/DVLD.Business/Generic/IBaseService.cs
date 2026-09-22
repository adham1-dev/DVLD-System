using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.Generic
{
    public interface IBaseService
    {
        DataTable GetView();
        DataRow GetDataRawById(int ID);
        bool Delete(int id);
        BaseDTO GetDTOById(int id);
    }
}
