using DVLD.Business.Generic;
using DVLD.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.DTOs
{
    public class ApplicationTypeDTO : BaseDTO
    {
        public string AppTitle { get; set; }
        public decimal AppFees { get; set; }
        public ApplicationTypeDTO() { }
        public ApplicationTypeDTO(int AppTypeID)
        {
            this.ID = AppTypeID;

            if (this.ID != default)
            {
                var cachedData = ApplicationTypesService.DTOById(this.ID);

                if (cachedData != null)
                {
                    this.AppTitle = cachedData.AppTitle;
                    this.AppFees = cachedData.AppFees;
                }
            }
        }
    }
}
