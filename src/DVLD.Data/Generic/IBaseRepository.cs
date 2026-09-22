using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Data.Generic
{
    public interface IBaseRepository
    {
         DataTable GetAllRecords(string FilterBy = null, object Value = null);
         DataRow GetDataRow(int ID);
         bool IsExist(int ID);
         bool Delete(int ID);

    }
}
