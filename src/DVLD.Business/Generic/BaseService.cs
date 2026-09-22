using DVLD.Business.DTOs;
using DVLD.Data.Entities;
using DVLD.Data.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.Generic
{
    public abstract class BaseService: IBaseService 
    {

        protected IBaseRepository _baseRepo;

        protected BaseService(IBaseRepository repo)
        {
            _baseRepo = repo;
        }


        public DataTable GetView()
        {
            return _baseRepo.GetAllRecords();
        }

        public DataRow GetDataRawById(int ID)
        {
            if (ID > 0)
                return _baseRepo.GetDataRow(ID);
            else return null;
        }

        public bool Delete(int id)
        {
            if (id <= 0) throw new Exception("Invalid id to delete");
            return _baseRepo.Delete(id);
        }
        public DataRow GetRowById(int id)
        {
            if (id <= 0) throw new Exception("Invalid id to Get");
            return _baseRepo.GetDataRow(id);
        }
        public abstract BaseDTO GetDTOById(int id);

    }
}
