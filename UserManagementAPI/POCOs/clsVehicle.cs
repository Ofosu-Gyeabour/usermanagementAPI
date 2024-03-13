namespace UserManagementAPI.POCOs
{
    public record DriverVehicleRecord
    {
        public int id { get; set; }
        public string driveremail { get; set; }
        public string vehicleRegistration { get; set; }

    }
    public class clsVehicle
    {
        
        public clsVehicle()
        {
            
        }

        public int Id { get; set; }
        public string? registrationNo { get; set; } = string.Empty;
        public string? vehicleMake { get; set; } = string.Empty;
        public string? isHired { get; set; } = @"No";
        public string? hiredCompany { get; set; } = string.Empty;
        public DateTime? hiredDate { get; set; } = DateTime.Now;
        public string? inUse { get; set; } = @"Yes";
        public string? isAssigned { get; set; } = @"Yes";

        public async Task<clsVehicle> Get(string vregNo)
        {
            //TODO: gets a vehicle using the vehicle registration number
            clsVehicle obj = null; 
            
            try
            {
                var config = new swContext();
                using (config)
                {
                    var o = await config.TVehiclePools.Where(v => v.RegNo == vregNo).FirstOrDefaultAsync();
                    obj = o != null ? new clsVehicle() { 
                        Id = o.Id,
                        registrationNo = vregNo != null ? vregNo : string.Empty,
                        vehicleMake = o.VehicleMake != null ? o.VehicleMake: string.Empty,
                        isHired = o.IsHired == true? @"Yes": @"No",
                        isAssigned = o.IsAssigned == true? @"Yes": @"No"
                    } : null;
                }

                return obj;
            }
            catch(Exception x)
            {
                throw x;
            }
        }
    }
}
