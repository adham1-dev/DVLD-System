using DVLD.Business.DTOs;
using DVLD.Business.Generic;
using DVLD.Business.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Presentation.Generic
{
    public enum Mode { AddNew, Updated }
    public class UpdateAdd
    {

        private IUpdateAddService _service;

        public Mode currentMode { get; private set; }
        private BaseDTO _Info;

        private DataRow _row = null;
        public UpdateAdd(BaseDTO info, IUpdateAddService service)
        {
            _service = service;
            _Info = info ?? throw new Exception("Value Cant be null");

            if (_Info.ID == default)
                currentMode = Mode.AddNew;
            else
                currentMode = Mode.Updated;

        }



        public DataRow Save()
        {
            return UIHelper.Execute(
                action: () =>
                {
                    switch (currentMode)
                    {
                        case Mode.AddNew:
                            _row = _service.AddNew(_Info);
                            _Info.ID = (int)_row[0];
                            currentMode = Mode.Updated;

                            MessageBox.Show(
                                "Saved successfully",
                                "Operation",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            return _row;

                        case Mode.Updated:
                            _row = _service.Update(_Info);

                            MessageBox.Show(
                                "Updated successfully",
                                "Operation",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            return _row;

                        default:
                            throw new InvalidOperationException("Invalid mode.");
                    }
                });
        }

    }
}
