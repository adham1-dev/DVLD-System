using DVLD.Business.DTOs;
using DVLD.Business.Generic;
using DVLD.Business.Services;
using DVLD.Presentation.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Presentation.Generic
{
    public class UIHelper
    {
        IBaseService _service;
        public UIHelper(IBaseService s) 
        { 
            _service = s;
        }

        public BaseDTO GetDTO(int id) 
        {
            return Execute
            (
                action: () => _service.GetDTOById(id)
            );

        }

        //Ready to abstract
        public void FillDGV(DataGridView gridView, DataView view, string filterKeyWord, string searchValue = "")
        {
            Execute
            (
                action: () =>
                {
                    view.RowFilter = $"convert({filterKeyWord}, 'System.String') Like '{searchValue}%'";
                    gridView.DataSource = view;
                }
            );
        }
        //dosnt fill ?!!
        public bool Delete(int id)
        {
            return Execute
            (
                action: () => _service.Delete(id)
            );

            
        }
        public DataRow GetRow(int id)
        {

            return Execute
            (
                action: () => _service.GetDataRawById(id)
            );

        }

        public static void Execute( Action action, string ErrorMessage = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ErrorMessage, ex);

                MessageBox.Show(
                    ErrorMessage ?? ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public static T Execute<T>( Func<T> action, string ErrorMessage = null)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ErrorMessage, ex);

                MessageBox.Show(
                    ErrorMessage ?? ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return default;
            }
        }

    }
}
