#nullable disable

namespace UserManagementAPI.POCOs
{
    public class clsDuty
    {
        public clsDuty()
        {
            
        }

        #region properties

        public int Id { get; set; }
        public decimal frgtBar1Duty { get; set; } = 0m;
        public decimal frgtBar2Duty { get; set; } = 0m;
        public decimal frgtBar3Duty { get; set; } = 0m;
        public decimal frgtBar4Duty { get; set; } = 0m;
        public decimal frgtBar5Duty { get; set; } = 0m;

        #endregion

        #region methods

        public async Task<clsDuty> getDutyRecordAsync()
        {
            //gets the parameters used in computing duty during shipping
            clsDuty obj = null;

            try
            {
                var config = new swContext();

                using (config)
                {
                    try
                    {
                        var q = (from tdt in config.Tduties
                                 select new
                                 {
                                     id = tdt.Id,
                                     f1duty = tdt.Frgtbar1,
                                     f2duty = tdt.Frgtbar2,
                                     f3duty = tdt.Frgtbar3,
                                     f4duty = tdt.Frgtbar4,
                                     f5duty = tdt.Frgtbar5
                                 });

                        var qasyn = await q.ToListAsync().ConfigureAwait(false);
                        return obj = qasyn
                                   .Select(a => new clsDuty()
                                   {
                                       Id = a.id,
                                       frgtBar1Duty = (decimal)a.f1duty,
                                       frgtBar2Duty = (decimal)a.f2duty,
                                       frgtBar3Duty = (decimal)a.f3duty,
                                       frgtBar4Duty = (decimal)a.f4duty,
                                       frgtBar5Duty = (decimal)a.f5duty
                                   }).FirstOrDefault();
                    }
                    catch(Exception configErr)
                    {
                        throw configErr;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<clsDuty> getJTSDutyRecord()
        {
            //TODO: gets the duty parameters for JTS
            clsDuty obj = null;

            try
            {
                var config = new swContext();

                using (config)
                {
                    try
                    {
                        var q = (from tdt in config.Tdutyjts
                                 select new
                                 {
                                     id = tdt.Id,
                                     f1duty = tdt.Frgtbar1,
                                     f2duty = tdt.Frgtbar2,
                                     f3duty = tdt.Frgtbar3,
                                     f4duty = tdt.Frgtbar4,
                                     f5duty = tdt.Frgtbar5
                                 });

                        var qasyn = await q.ToListAsync().ConfigureAwait(false);
                        return obj = qasyn
                                   .Select(a => new clsDuty()
                                   {
                                       Id = a.id,
                                       frgtBar1Duty = (decimal)a.f1duty,
                                       frgtBar2Duty = (decimal)a.f2duty,
                                       frgtBar3Duty = (decimal)a.f3duty,
                                       frgtBar4Duty = (decimal)a.f4duty,
                                       frgtBar5Duty = (decimal)a.f5duty
                                   }).FirstOrDefault();
                    }
                    catch (Exception configErr)
                    {
                        throw configErr;
                    }
                }
            }
            catch(Exception x)
            {
                throw x;
            }
        }

        #endregion

    }
}
